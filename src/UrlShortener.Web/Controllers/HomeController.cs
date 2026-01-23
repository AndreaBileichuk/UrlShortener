using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs;
using UrlShortener.BLL.DTOs.UrlShortener;
using UrlShortener.BLL.Services.AboutContent;
using UrlShortener.BLL.Services.UrlShortener;
using UrlShortener.DAL.Entities;
using UrlShortener.Web.Models;

namespace UrlShortener.Web.Controllers;

public class HomeController(IAboutContentService service, IUrlShortenerApiService apiService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> About()
    {
        var content = await service.GetAsync();
        return View(content);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> About(string newText)
    {
        await service.ChangeAboutContext(new AboutContentRequest(newText));
        return RedirectToAction("About");
    }
    
    [HttpGet]
    [Authorize]
    [Route("UrlInfo/{id}")]
    public async Task<IActionResult> Info(int id)
    {
        var url = await apiService.GetDetailsAsync(id);
        
        if (url == null)
        {
            return NotFound();
        }

        return View(url);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> RedirectToOriginal(string code)
    {
        var result = await apiService.GetUrlByCodeAsync(code);

        if (result == null) return NotFound();

        try 
        {
            var uri = new Uri(result);
            result = uri.AbsoluteUri; 
        }
        catch
        {
            return NotFound();
        }

        return Redirect(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}