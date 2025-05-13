using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/health")]
    public class HealthController : BaseController
    {
        private TrackerDbContext _context;

        public HealthController(TrackerDbContext context)
        {
            _context = context;
        }

        [HttpHead]
        [HttpGet]
        public IActionResult HealthCheck()
        {
            if (_context.Database.CanConnect())
            {
                var response = new
                {
                    Status = "healthy",
                    Checks = new
                    {
                        Database = "connected"
                    }
                };

                return Ok(response);
            }

            var errorResponse = new
            {
                Status = "unhealthy",
                Error = "Database not connected."
            };

            return StatusCode(500, errorResponse);
        }
    }
}
