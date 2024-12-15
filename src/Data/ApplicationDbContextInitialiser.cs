using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Data
{
    public class ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger = logger;
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<Role> _roleManager = roleManager;

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
                {
                    await _context.Database.EnsureDeletedAsync();
                    await _context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default roles
            var clientRole = new Role("Client");
            if (_roleManager.Roles.All(r => r.Name != clientRole.Name))
            {
                var role = await _roleManager.CreateAsync(clientRole);
            }

            var trainerRole = new Role("Trainer");
            if (_roleManager.Roles.All(r => r.Name != trainerRole.Name))
            {
                var role = await _roleManager.CreateAsync(trainerRole);
            }

            var administratorRole = new Role("Administrator");
            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                var role = await _roleManager.CreateAsync(administratorRole);
                if (role != null)
                {
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleView"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleAdd"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleEdit"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleDelete"));
                }
            }

            // Default users
            var administrator = new ApplicationUser { UserName = "UnifiedAppAdmin", Email = "UnifiedAppAdmin" };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "UnifiedAppAdmin1! ");
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                }
            }

            var client = new ApplicationUser { UserName = "client", Email = "client" };

            if (_userManager.Users.All(u => u.UserName != client.UserName))
            {
                await _userManager.CreateAsync(client, "Client1!");
                if (!string.IsNullOrWhiteSpace(clientRole.Name))
                {
                    await _userManager.AddToRolesAsync(client, new[] { clientRole.Name });
                }
            }

            var sprint = new Sprint
            {
                Id = Guid.Parse("6b5a7b83-371a-4033-a3c5-f4cf656855d8"),
                User = client,
                WeeksCount = 3,
                BeginDate = new DateOnly(2024, 12, 09),
                EndDate = new DateOnly(2024, 12, 29),
                Weeks = "{\"1\":{\"Begin\":\"2024-12-09\",\"End\":\"2024-12-15\"},\"2\":{\"Begin\":\"2024-12-16\",\"End\":\"2024-12-22\"},\"3\":{\"Begin\":\"2024-12-23\",\"End\":\"2024-12-29\"}}"
            };

            if (await _context.Sprints.AllAsync(s => s.Id != sprint.Id))
            {
                await _context.Sprints.AddAsync(sprint);
            }

            var project = new Project
            {
                Id = Guid.Parse("5090bd89-8d18-4948-a8b0-25dcd391a788"),
                Sprint = sprint,
                SectionName = SectionName.Life,
                Title = "Прогулки перед сном",
                PlanningTimes = "{\"1\":{\"Hours\":7,\"Minutes\":0},\"2\":{\"Hours\":7,\"Minutes\":0},\"3\":{\"Hours\":7,\"Minutes\":0}}"
            };

            if (await _context.Projects.AllAsync(p => p.Id != project.Id))
            {
                await _context.Projects.AddAsync(project);
            }

            await _context.SaveChangesAsync();
        }
    }
}