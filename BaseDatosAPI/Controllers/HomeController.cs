using EBGBackend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public HomeController(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    [HttpPost("UpdateBD")]
    public async Task<IActionResult> RunMigrations()
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();
            }
            return Ok(new { message = "Migraciones ejecutadas exitosamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
