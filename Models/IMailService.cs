namespace BharatMirror.Models
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task SendWelcomeEmailAsync(MailRequest mailRequest);
    }
}
