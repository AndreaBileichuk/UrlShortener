using System.ComponentModel.DataAnnotations;

namespace UrlShortener.BLL.DTOs;

public class LoginRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}