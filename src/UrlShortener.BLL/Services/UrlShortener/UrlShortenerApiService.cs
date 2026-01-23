using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs.UrlShortener;
using UrlShortener.BLL.Exceptions;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Services.UrlShortener;

public class UrlShortenerApiService(ApplicationDbContext context) : IUrlShortenerApiService
{
    public async Task<List<ShortUrlResponse>> GetAsync(ClaimsPrincipal user)
    {
        var urls = await context.ShortUrls
            .Include(u => u.CreatedBy)
            .Select(u => new ShortUrlResponse 
            {
                Id = u.Id,
                OriginalUrl = u.OriginalUrl,
                ShortCode = u.ShortCode,
                CreatedBy = u.CreatedBy!.UserName,
                IsOwner = user.Identity!.IsAuthenticated && u.CreatedBy.UserName == user.Identity.Name
            })
            .ToListAsync();

        return urls;
    }

    public async Task<ShortUrlResponse> CreateAsync(ClaimsPrincipal user, CreateShortUrlRequest request)
    {
        if (await context.ShortUrls.AnyAsync(s => s.OriginalUrl == request.OriginalUrl))
        {
            throw new ArgumentException("Duplicate url.");
        }
        
        var newUrl = new ShortUrl
        {
            OriginalUrl = request.OriginalUrl,
            CreatedByUserId = user.FindFirstValue(ClaimTypes.NameIdentifier),
            CreatedDate = DateTime.UtcNow,
            ShortCode = "temp" 
        };

        context.ShortUrls.Add(newUrl);
        await context.SaveChangesAsync();

        newUrl.ShortCode = UrlShortenetAlgorithm.GenerateShortCode(newUrl.Id);
        await context.SaveChangesAsync();

        return new ShortUrlResponse()
        {
            Id = newUrl.Id,
            OriginalUrl = newUrl.OriginalUrl,
            ShortCode = newUrl.ShortCode,
            CreatedBy = user.Identity?.Name ?? "Unknown",
            IsOwner = true
        };
    }

    public async Task DeleteAsync(ClaimsPrincipal user, int id)
    {
        var url = await context.ShortUrls.Include(u => u.CreatedBy).FirstOrDefaultAsync(u => u.Id == id);
        if (url == null) throw new NotFound();

        var isOwner = url.CreatedByUserId == user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = user.IsInRole("Admin");

        if (!isOwner && !isAdmin)
        {
            throw new Forbid();
        }

        context.ShortUrls.Remove(url);
        await context.SaveChangesAsync();
    }
}