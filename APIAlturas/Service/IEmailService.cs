using System.Threading.Tasks;

namespace APIAlturas.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}