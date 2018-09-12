using olmelabs.battleship.api.Models.Entities;
using System.Net.Mail;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Services
{
    public interface INotificationService
    {
        Task SendResetPasswordMailAsync(string code, User user);

        Task SendConfirmEmailMailAsync(string code, User user);

        MailMessage TryDequeueEmail();

        void EnqueueEmail(MailMessage msg);
    }
}
