using DentaEquip.BL.Repositories.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
