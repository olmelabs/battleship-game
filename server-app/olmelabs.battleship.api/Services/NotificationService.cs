using System.Collections.Concurrent;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Models.ViewModels;

namespace olmelabs.battleship.api.Services
{
    public class NotificationService : INotificationService
    {
        private static ConcurrentQueue<MailMessage> _mailQueue;
        private readonly IRazorViewRendererService _renderer;
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config, IRazorViewRendererService renderer)
        {
            _config = config;
            _renderer = renderer;
            _mailQueue = new ConcurrentQueue<MailMessage>();
        }

        public async Task SendResetPasswordMailAsync(string code, User user)
        {
            var model = new ResetPasswordViewModel()
            {
                FristName = user.FirstName,
                SiteUrl = _config["Mail:SiteUrl"],
                Code = code,
            };

            MailMessage msg = new MailMessage(_config["Mail:From"], user.Email)
            {
                Subject = "🚢[battleship-game] Reset Password",
                IsBodyHtml = true,
                Body = await _renderer.RenderViewToStringAsync("EmailTemplates/ResetPassword", model)
            };

            _mailQueue.Enqueue(msg);
        }

        public async Task SendConfirmEmailMailAsync(string code, User user)
        {
            var model = new ConfirmEmailViewModel()
            {
                FristName = user.FirstName,
                SiteUrl = _config["Mail:SiteUrl"],
                Code = code,
            };

            MailMessage msg = new MailMessage(_config["Mail:From"], user.Email)
            {
                Subject = "🚢[battleship-game] Confirm Email",
                IsBodyHtml = true,
                Body = await _renderer.RenderViewToStringAsync("EmailTemplates/ConfirmEmail", model)
            };

            _mailQueue.Enqueue(msg);
        }

        public MailMessage TryDequeueEmail()
        {
            bool res = _mailQueue.TryDequeue(out MailMessage msg);
            if (res)
                return msg;
            return null;
        }

        public void EnqueueEmail(MailMessage msg)
        {
            _mailQueue.Enqueue(msg);
        }
    }
}
