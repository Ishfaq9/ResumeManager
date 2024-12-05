using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace ResumeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeapiController : ControllerBase
    {
        private readonly ResumeDbContext _dbContext;

        public ResumeapiController(ResumeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var results = _dbContext.Applicants.ToList();
            if(results.Any())
            {
                return Ok(results);
            }
            return BadRequest(new { Message ="No Applicants found"  });
        }
    }
}
