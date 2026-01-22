using UrlShortener.BLL.DTOs;

namespace UrlShortener.BLL.Services.AboutContent;

public interface IAboutContentService
{
    Task<AboutContentResponse> GetAsync();

    Task ChangeAboutContext(AboutContentRequest request);
}