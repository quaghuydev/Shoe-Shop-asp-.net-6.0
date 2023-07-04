namespace ShoeShop.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string username, string subject, string message);
    }
}
