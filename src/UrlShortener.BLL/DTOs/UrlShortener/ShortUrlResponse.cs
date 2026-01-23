using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.DTOs.UrlShortener;

public class ShortUrlResponse
{
    public int Id { get; set; }

    public string OriginalUrl { get; set; } = string.Empty;

    public string ShortCode { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }

    public bool IsOwner { get; set; }
}