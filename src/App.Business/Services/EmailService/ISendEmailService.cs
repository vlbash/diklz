using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Business.Services.EmailService
{
    public interface ISendEmailService
    {
        Task SendAsync(List<string> recipientList, string body, string subject = null);
    }
}
