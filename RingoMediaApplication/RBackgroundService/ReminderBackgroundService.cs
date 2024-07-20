
using Microsoft.EntityFrameworkCore;
using RingoMediaApplication.Data;
using RingoMediaApplication.MailService;

namespace RingoMediaApplication.RBackgroundService
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEmailService _emailService;

        public ReminderBackgroundService(IServiceScopeFactory serviceScopeFactory, IEmailService emailService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckRemindersAsync();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); 
            }
        }

        private async Task CheckRemindersAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var now = DateTime.Now;
                var reminders = await dbContext.Reminders
                    .Where(r => r.DateTime <= now && !r.IsSent)
                    .ToListAsync();

                foreach (var reminder in reminders)
                {
                    await _emailService.SendEmailAsync("recipient@example.com", reminder.Title, "Reminder message");
                    reminder.IsSent = true;
                    dbContext.Reminders.Update(reminder);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
