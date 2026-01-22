using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs;
using UrlShortener.BLL.Services.AboutContent;
using UrlShortener.DAL.Entities;
using UrlShortener.Web.Models;

namespace UrlShortener.Web.Controllers;

public class HomeController(IAboutContentService service) : Controller
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}