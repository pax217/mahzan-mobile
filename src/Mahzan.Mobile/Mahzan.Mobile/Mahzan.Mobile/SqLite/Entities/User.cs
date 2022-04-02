using System;

namespace Mahzan.Mobile.SqLite.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid TaxRegimeCode { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string WhatsappPhone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordEncrypted { get; set; }
        public bool ConfirmEmail { get; set; }
        public Guid TokenConfirmEmail { get; set; } 
    }
}