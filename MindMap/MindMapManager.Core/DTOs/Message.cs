using MimeKit;

namespace MindMapManager.Core.DTOs
{
    public class Message
    {
        public List<MailboxAddress> To {  get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Message(IEnumerable<EmailAddress> to,string subject,string body )
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.DisplayName,x.Email)));
            Subject = subject;
            Body = body;
        }

    }
}
