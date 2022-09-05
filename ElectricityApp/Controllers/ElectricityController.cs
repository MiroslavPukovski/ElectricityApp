using ElectricityApp.Data;
using ElectricityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerilogTimings;

namespace ElectricityApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElectricityController : ControllerBase
    {
        private readonly ILogger<ElectricityController> _logger;
        private readonly ApplicationDbContext _db;

        public ElectricityController(ApplicationDbContext db, ILogger<ElectricityController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("{pageNr:int}/{pageSize:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgregatedElectricityData(int pageNr = 1, int pageSize = 20)
        {
            _logger.LogInformation("Accessed to electricity agregated data 2022-04 to 2022-05");
            
            try
            {
                using (Operation.Time("Work with DB Query"))
                {
                    var electricityList = await _db.Electricities
                       .Skip((pageNr - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();

                    return Ok(electricityList);
                }   
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong in the {nameof(GetAgregatedElectricityData)}");

                return StatusCode(500, "Internal Server Error.");
            }
        }   
    }
}
