using Kolokwium2.Services;



using Microsoft.AspNetCore.Mvc;
namespace Kolokwium2.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EnrollmentsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEnrollmentsDetails()
    {
        return Ok(await service.GetAllEnrollmentsWithDetailsAsync());
    }
    
    
}