using Microsoft.EntityFrameworkCore;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;

namespace UrlShortener.Tests;

public class UrlDatabaseTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void AddUrl_ShouldSaveToDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var newUrl = new ShortUrl
        {
            OriginalUrl = "https://google.com",
            ShortCode = "test",
            CreatedByUserId = "user1",
            CreatedDate = DateTime.UtcNow
        };

        // Act
        context.ShortUrls.Add(newUrl);
        context.SaveChanges();

        // Assert
        var savedUrl = context.ShortUrls.FirstOrDefault(u => u.ShortCode == "test");
        Assert.NotNull(savedUrl);
        Assert.Equal("https://google.com", savedUrl.OriginalUrl);
    }

    [Fact]
    public void DeleteUrl_ShouldRemoveFromDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var url = new ShortUrl { OriginalUrl = "https://delete.me", ShortCode = "del", CreatedByUserId = "u1" };
        context.ShortUrls.Add(url);
        context.SaveChanges();

        // Act
        context.ShortUrls.Remove(url);
        context.SaveChanges();

        // Assert
        Assert.Equal(0, context.ShortUrls.Count());
    }
}