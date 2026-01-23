using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs.Auth;

public class RegisterRequest
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}