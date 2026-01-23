using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.DTOs.UrlShortener;
using UrlShortener.BLL.Exceptions;
using UrlShortener.BLL.Services.UrlShortener;

namespace UrlShortener.Web.Controllers.Api;

[Route("api/urls")]
[ApiController]
public class UrlApiController(IUrlShortenerApiService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAsync(User));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateShortUrlRequest request)
    {
        try
        {
            var newUrl = await service.CreateAsync(User, request);
            return Ok(newUrl);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        } 
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteAsync(User, id);
            return Ok();
        }
        catch (NotFound)
        {
            return this.NotFound();
        }
        catch (Forbid)
        {
            return this.Forbid();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}