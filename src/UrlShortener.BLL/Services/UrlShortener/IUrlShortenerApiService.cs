using System.Security.Claims;
using UrlShortener.BLL.DTOs.UrlShortener;

namespace UrlShortener.BLL.Services.UrlShortener;

public interface IUrlShortenerApiService
{
    Task<List<ShortUrlResponse>> GetAsync(ClaimsPrincipal user);

    Task<ShortUrlDetailsResponse?> GetDetailsAsync(int id);

    Task<ShortUrlResponse> CreateAsync(ClaimsPrincipal user, CreateShortUrlRequest request);

    Task DeleteAsync(ClaimsPrincipal user, int id);

    Task<string?> GetUrlByCodeAsync(string code);
}