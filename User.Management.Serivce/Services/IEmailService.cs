using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Management.Serivce.Models;

namespace User.Management.Serivce.Services
{
    internal interface IEmailService
    {
        void SendEmail(Message message);
    }
}
