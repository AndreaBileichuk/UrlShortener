using System.Security.Claims;
using UrlShortener.BLL.DTOs.UrlShortener;

namespace UrlShortener.BLL.Services.UrlShortener;

public interface IUrlShortenerApiService
{
    Task<List<ShortUrlResponse>> GetAsync(ClaimsPrincipal user);

    Task<ShortUrlResponse> CreateAsync(ClaimsPrincipal user, CreateShortUrlRequest request);

    Task DeleteAsync(ClaimsPrincipal user, int id);
}