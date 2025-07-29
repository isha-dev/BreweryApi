using BreweryApi.Models;
using BreweryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BreweryApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BreweriesController : ControllerBase
    {
        private readonly IBreweryService _breweryService;
        private readonly ILogger<BreweriesController> _logger;

        public BreweriesController(IBreweryService breweryService, ILogger<BreweriesController> logger)
        {
            _breweryService = breweryService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("GetBreweries")]
        public async Task<IActionResult> GetBreweries([FromBody] SearchRequest searchRequest)
        {
            try
            {
                var breweries = await _breweryService.GetBreweriesAsync(searchRequest.Search, searchRequest.SortBy);
                breweries.Where(b => b.Name.StartsWith(searchRequest.Search, StringComparison.OrdinalIgnoreCase)).Take(10);

                return Ok(breweries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching breweries");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }


      
    }
}
