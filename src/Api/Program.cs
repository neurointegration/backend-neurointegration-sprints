using AspNetCore.Yandex.ObjectStorage.Extensions;
using Dapper;
using Data;
using Data.DapperHandlers;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using Service.UserGroup;
using System.Data;
using System.Text;
using Ydb.Sdk.Ado;
using Ydb.Sdk.Auth;
using Ydb.Sdk.Yc;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var port = Environment.GetEnvironmentVariable("PORT");
            if (!string.IsNullOrEmpty(port))
            {
                builder.WebHost.UseUrls($"http://*:{port}");
            }

            var appSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>() ?? default!;
            builder.Services.AddSingleton(appSettings);

            var loggerFactory = LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });

            var saFilePath = builder.Configuration["YDB:SaFilePath"];
            ICredentialsProvider credentialsProvider;
            if (saFilePath != null)
                credentialsProvider = new ServiceAccountProvider(saFilePath, loggerFactory);
            else
                credentialsProvider = new MetadataProvider(loggerFactory);

            var ydbConnectionBuilder = new YdbConnectionStringBuilder
            {
                Host = builder.Configuration["YDB:Host"] ?? throw new InvalidOperationException("Env  'YDB:Host' not found."),
                Port = int.Parse(builder.Configuration["YDB:Port"] ?? throw new InvalidOperationException("Env  'YDB:Port' not found.")),
                Database = builder.Configuration["YDB:Database"] ?? throw new InvalidOperationException("Env  'YDB:Database' not found."),
                LoggerFactory = loggerFactory,
                CredentialsProvider = credentialsProvider,
                UseTls = true
            };

            builder.Services.AddTransient<IDbConnection>(sp =>
            {
                return new YdbConnection(ydbConnectionBuilder);
            });

            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime);
            SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new SqlDictionaryTypeHandler<string, PlanningTime>());
            SqlMapper.AddTypeHandler(new SqlDictionaryTypeHandler<string, FactTime>());

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidIssuer = appSettings.Issuer,
                        ValidAudience = appSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey)),
                        ClockSkew = TimeSpan.FromSeconds(0)
                    };
                });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
            builder.Services.AddScoped<ISprintRepository, SprintRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            builder.Services.AddScoped<IUserManagementService, UserManagementService>();
            builder.Services.AddScoped<ISprintService, SprintService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ITrainerCommentService, TrainerCommentService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddTransient<UserService>();

            builder.Services.AddYandexObjectStorage(builder.Configuration);
            builder.Services.AddScoped<AvatarService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("webAppRequests", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(appSettings.Audience)
                    .AllowCredentials();
                });
            });

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "App Api", Version = "1.0.0" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(
                    new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("webAppRequests");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}