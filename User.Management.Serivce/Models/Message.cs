
using MimeKit;

namespace User.Management.Serivce.Models
{
internal class Message
    {

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message (IEnumerable<string> to,string subject,string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(s => new MailboxAddress("email",s)));
            Subject = subject;
            Content = content;
        }
    }
}
