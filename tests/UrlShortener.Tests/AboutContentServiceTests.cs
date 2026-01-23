using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs;
using UrlShortener.BLL.Services.AboutContent;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;

namespace UrlShortener.Tests;

public class AboutContentServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefaultMessage_WhenDatabaseIsEmpty()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new AboutContentService(context);

        // Act
        var result = await service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("No description found.", result.Text);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnExistingText_WhenDatabaseHasContent()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.AboutContents.Add(new AboutContent { Text = "Existing description" });
        await context.SaveChangesAsync();

        var service = new AboutContentService(context);

        // Act
        var result = await service.GetAsync();

        // Assert
        Assert.Equal("Existing description", result.Text);
    }

    [Fact]
    public async Task ChangeAboutContext_ShouldCreateNewRecord_WhenDatabaseIsEmpty()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new AboutContentService(context);
        var request = new AboutContentRequest("New created text");

        // Act
        await service.ChangeAboutContext(request);

        // Assert
        var savedRecord = await context.AboutContents.FirstOrDefaultAsync();
        Assert.NotNull(savedRecord);
        Assert.Equal("New created text", savedRecord.Text);

        // Перевіряємо, що запис всього один
        var record = await context.AboutContents.CountAsync();
        Assert.Equal(1, record);
    }

    [Fact]
    public async Task ChangeAboutContext_ShouldUpdateExistingRecord_WhenDatabaseIsNotEmpty()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        // Спочатку додаємо "старий" запис
        context.AboutContents.Add(new AboutContent { Text = "Old text" });
        await context.SaveChangesAsync();

        var service = new AboutContentService(context);
        var request = new AboutContentRequest("Updated new text");

        // Act
        await service.ChangeAboutContext(request);

        // Assert
        var savedRecord = await context.AboutContents.FirstOrDefaultAsync();
        Assert.NotNull(savedRecord);
        Assert.Equal("Updated new text", savedRecord.Text);

        // Головна перевірка: чи не створився дублікат? Має залишитись 1 запис.
        Assert.Equal(1, await context.AboutContents.CountAsync());
    }
}