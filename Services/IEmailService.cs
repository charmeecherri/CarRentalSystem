using CarRentalSystem.Utilities;

namespace CarRentalSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
