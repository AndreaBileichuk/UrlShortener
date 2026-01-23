using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.DAL.Entities;

public class ShortUrl
{
    public int Id { get; set; }

    [Required]
    [Url(ErrorMessage = "Invalid URL format")]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)] 
    public string ShortCode { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? CreatedByUserId { get; set; }

    [ForeignKey("CreatedByUserId")]
    public ApplicationUser? CreatedBy { get; set; }
}