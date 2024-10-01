namespace PB303Pronia.Services.Abstactions;

public interface IEmailService
{
    void SendEmail(string toEmail, string subject, string emailBody);

}
