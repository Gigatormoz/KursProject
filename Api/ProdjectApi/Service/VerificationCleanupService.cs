using ProdjectApi.Data;

namespace ProdjectApi.Service
{
    public class VerificationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public VerificationCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(60 * 1000, stoppingToken);
                using var scope = _serviceProvider.CreateScope();

                var context = scope.ServiceProvider.
                    GetRequiredService<ProjectDbContext>();

                var unverified = context.Users
                    .Where(u => !u.IsEmailVerified && u.RegistrationDate <= DateTime.UtcNow.AddMinutes(-3)).ToList();

                context.Users.RemoveRange(unverified);
                await context.SaveChangesAsync();

                var oldCodes = context.EmailVerifications
                   .Where(v => v.ExpiresAt <= DateTime.UtcNow.AddMinutes(-3)).ToList();

                context.EmailVerifications.RemoveRange(oldCodes);
                await context.SaveChangesAsync();
            }
        }
    }
}
