using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Dto;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection;
using System.Net;
using Service.Exceptions;

namespace Service
{
    public class BackgroundSprintsUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BackgroundSprintsUpdateService> _logger;
        private int _updateIntervalMinutes;

        public BackgroundSprintsUpdateService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<BackgroundSprintsUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
            _updateIntervalMinutes = int.Parse(_configuration["BotIntegration:SprintsUpdateTimeInMinutes"]!);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Sprints Update Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var sprintUpdater = scope.ServiceProvider.GetRequiredService<ISprintUpdaterService>();
                        await sprintUpdater.UpdateSprintsAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating sprints.");
                }

                _logger.LogInformation("Waiting for {Interval} minutes before next update.", _updateIntervalMinutes);

                await Task.Delay(TimeSpan.FromMinutes(_updateIntervalMinutes), stoppingToken);
            }

            _logger.LogInformation("Sprints Update Service is stopping.");
        }
    }

    public interface ISprintUpdaterService
    {
        Task UpdateSprintsAsync(CancellationToken cancellationToken);
    }

    public class SprintUpdaterService : ISprintUpdaterService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _sprintsUrl;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SprintUpdaterService> _logger;

        public SprintUpdaterService(ApplicationDbContext context, IConfiguration configuration, HttpClient httpClient, ILogger<SprintUpdaterService> logger)
        {
            _context = context;
            _sprintsUrl = configuration["BotIntegration:SprintsUrl"]!;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task UpdateSprintsAsync(CancellationToken cancellationToken)
        {
            var users = await _context.Users.ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                try
                {
                    if (user.UserName == null)
                        continue;
                    var botSprints = await GetSprintsForUserAsync(user.UserName, cancellationToken);
                    var lastSprintInDb = await _context.Sprints
                        .Where(s => s.UserId == user.Id)
                        .OrderByDescending(s => s.BeginDate)
                        .FirstOrDefaultAsync(cancellationToken);
                    var lastBeginDateInDb = lastSprintInDb?.BeginDate ?? DateOnly.MinValue;
                    var newSprints = botSprints
                        .Where(s => DateOnly.FromDateTime(s.SprintStartDate) > lastBeginDateInDb)
                        .ToList();
                    foreach (var tmpSprintDto in newSprints)
                    {
                        var sprint = MapToSprint(tmpSprintDto, user);
                        await _context.Sprints.AddAsync(sprint, cancellationToken);
                    }
                    if (newSprints.Any())
                    {
                        await _context.SaveChangesAsync(cancellationToken);
                        _logger.LogInformation(
                            "Added {Count} new sprints for user with username {Username}.",
                            newSprints.Count,
                            user.UserName
                        );
                    }
                }
                catch ( HttpRequestException )
                {
                    _logger.LogInformation("User with username {Username} was not found in the bot.", user.UserName);
                    continue;
                }
            }
        }

        private Sprint MapToSprint(TmpSprintDto tmpSprintDto, ApplicationUser user)
        {
            var beginDate = DateOnly.FromDateTime(tmpSprintDto.SprintStartDate);
            var weeksCount = user.SprintWeeksCount == 3 ? 3 : 4;
            var weeks = new Dictionary<int, SprintWeekDto>();
            for (var i = 0; i < weeksCount; i++)
            {
                weeks.Add(
                    i + 1,
                    new SprintWeekDto
                    {
                        Begin = beginDate.AddDays(i * 7),
                        End = beginDate.AddDays(i * 7 + 6)
                    }
                );
            }
            var sprint = new Sprint
            {
                Id = Guid.NewGuid(),
                WeeksCount = weeksCount,
                BeginDate = beginDate,
                EndDate = beginDate.AddDays(weeksCount * 7),
                Weeks = JsonSerializer.Serialize(weeks),
                UserId = user.Id
            };
            return sprint;
        }

        private async Task<List<TmpSprintDto>> GetSprintsForUserAsync(string username, CancellationToken cancellationToken)
        {
            var requestBody = new { username = "@" + username };
            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_sprintsUrl),
                Content = new StringContent(jsonRequestBody, null, MediaTypeNames.Application.Json)
            };
            var response = await _httpClient.SendAsync(request, cancellationToken);
            var tmpSprintResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(tmpSprintResponse);
            response.EnsureSuccessStatusCode();
            var sprintResponse = await response.Content.ReadFromJsonAsync<TmpSprintResponse>();
            return sprintResponse?.Sprints ?? new List<TmpSprintDto>();
        }

        private class TmpSprintResponse
        {
            public List<TmpSprintDto> Sprints { get; set; } = new List<TmpSprintDto>();
        }

        private class TmpSprintDto
        {
            public int SprintNumber { get; set; }
            public DateTime SprintStartDate { get; set; }
        }
    }
}
