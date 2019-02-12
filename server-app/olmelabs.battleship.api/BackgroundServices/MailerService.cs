using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using olmelabs.battleship.api.Services;
using olmelabs.battleship.api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.BackgroundServices
{
    public class MailerService : BackgroundService
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<MailerService> _logger;
        private readonly IConfiguration _config;

        public MailerService(ILogger<MailerService> logger,
            INotificationService notificationService,
            IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"MailerService is starting.");

            stoppingToken.Register(() =>
            {
                _logger.LogDebug($"MailerService is stopping.");
            });

            var res = int.TryParse(_config["Mail:Port"], out int port);
            if (!res)
                port = 587;

            res = bool.TryParse(_config["Mail:EnableSsl"], out bool enableSsl);
            if (!res)
                enableSsl = true;

            List<MailMessage> failedEmails = new List<MailMessage>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = _notificationService.TryDequeueEmail();

                using (var smtpClient = new SmtpClient
                {
                    Host = _config["Mail:Host"],
                    Port = port,
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(_config["Mail:User"], _config["Mail:Password"])
                })
                {
                    while (msg != null)
                    {
                        try
                        {
                            await smtpClient.SendMailAsync(msg);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "MailerService: Email Send fail", null);

                            failedEmails.Add(msg);
                        }

                        msg = _notificationService.TryDequeueEmail();
                    }
                }

                //re-enqueue failed messages
                foreach (var failedMsg in failedEmails)
                {
                    _notificationService.EnqueueEmail(failedMsg);
                }

                failedEmails.Clear();

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
