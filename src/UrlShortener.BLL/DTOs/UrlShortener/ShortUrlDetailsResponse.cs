namespace UrlShortener.BLL.DTOs.UrlShortener;

public class ShortUrlDetailsResponse
{
    public int Id { get; set; }

    public string OriginalUrl { get; set; } = string.Empty;
    
    public string ShortUrl { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; }
}