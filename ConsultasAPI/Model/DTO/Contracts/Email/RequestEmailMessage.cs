using System.Net.Mail;
using System.Net.Mime;

namespace ConsultasAPI.Model.DTO.Contracts.Email
{
    public class RequestEmailMessage
    {
        public List<string> To { get; set; } = new();
        public List<string> Cc { get; set; } = new();
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsHtml { get; set; } = true;
    }
      
}