using AFHOfficeFeedApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AFH_Locations_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AFHLocationsController : ControllerBase
    {
        private readonly LocationService _locationService;

        public AFHLocationsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int take = 4)
        {
            try
            {
                var allOffices = await _locationService.GetLocationsAsync();
                var paginated = allOffices.Skip(skip).Take(take);
                return Ok(paginated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching locations.");
            }
        }
    }
}
