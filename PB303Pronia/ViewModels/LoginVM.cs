namespace PB303Pronia.ViewModels;

public class LoginVM
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? ReturnUrl { get; set; }
}


public class RegisterVM
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}