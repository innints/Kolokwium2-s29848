using Kolokwium2.Services;



using Microsoft.AspNetCore.Mvc;
namespace Kolokwium2.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatientsDetails()
    {
        return Ok(await service.GetAllPatientsDetailsAsync());
    }
    
    
}