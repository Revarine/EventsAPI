using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    [HttpGet("test1")]
    public IActionResult TestEndpoint1()
    {
        return Ok(new { message = "You have access to admin endpoint 1!" });
    }

    [HttpGet("test2")]
    public IActionResult TestEndpoint2()
    {
        return Ok(new { message = "You have access to admin endpoint 2!" });
    }
} 