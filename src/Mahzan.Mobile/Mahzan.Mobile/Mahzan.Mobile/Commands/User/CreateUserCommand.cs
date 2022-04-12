namespace Mahzan.Mobile.Commands.User
{
    public class CreateUserCommand
    {
        public string CompanyName { get; set; }
        
        public string ContactName { get; set; }
        
        public string WhatsappPhone { get; set; }
        
        public string Email { get; set; }
        
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string PasswordEncrypted { get; set; }
    }
}