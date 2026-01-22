using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs;
using UrlShortener.DAL.Data;

namespace UrlShortener.BLL.Services.AboutContent;

public class AboutContentService(ApplicationDbContext dbContext) : IAboutContentService
{
    public async Task<AboutContentResponse> GetAsync()
    {
        var result = await dbContext.AboutContents.FirstOrDefaultAsync();
        return new AboutContentResponse(result?.Text ?? "No description found.");
    }

    public async Task ChangeAboutContext(AboutContentRequest request)
    {
        var result = await dbContext.AboutContents.FirstOrDefaultAsync();

        if (result is null)
        {
            dbContext.AboutContents.Add(new DAL.Entities.AboutContent()
            {
                Text = request.Text
            });
        }
        else
        {
            result.Text = request.Text;
        }

        await dbContext.SaveChangesAsync();
    }
}