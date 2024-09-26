namespace PB303Pronia.ViewModels
{
    public class LoginVM
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
