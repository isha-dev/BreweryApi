using BreweryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BreweryApi.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly BreweryDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BreweryService> _logger;

        private const string CacheKey = "BreweryListCache";

        public BreweryService(
            BreweryDbContext context,
            HttpClient httpClient,
            IMemoryCache cache,
            ILogger<BreweryService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweriesAsync(string? search = null, string? sortBy = null)
        {
            try
            {
                if (!_cache.TryGetValue(CacheKey, out List<BreweryDto> breweries))
                {
                    breweries = await FetchAndCacheBreweriesAsync();
                }

                breweries = FilterBreweries(breweries, search);
                breweries = SortBreweries(breweries, sortBy);

                return breweries;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while calling the brewery API.");
                throw new ApplicationException("Unable to retrieve brewery data at this time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetBreweriesAsync.");
                throw;
            }
        }

        private async Task<List<BreweryDto>> FetchAndCacheBreweriesAsync()
        {
            // Populate DB if needed
            if (!_context.Breweries.Any())
            {
                var apiResponse = await _httpClient.GetFromJsonAsync<List<ApiBreweryResponse>>(
                    "https://api.openbrewerydb.org/v1/breweries");

                if (apiResponse == null || !apiResponse.Any())
                    throw new Exception("Failed to fetch brewery data.");

                var entities = apiResponse.Select(b => new BreweryDto
                {
                    Name = b.name ?? "Unknown",
                    City = b.city ?? "Unknown",
                    Phone = string.IsNullOrWhiteSpace(b.phone) ? null : b.phone,
                    Distance = b.distance
                }).ToList();

                await _context.Breweries.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }

            var allBreweries = await _context.Breweries.ToListAsync();
            var dtoList = allBreweries.Select(MapToDto).ToList();

            _cache.Set(CacheKey, dtoList, TimeSpan.FromMinutes(10));

            return dtoList;
        }

        private List<BreweryDto> FilterBreweries(List<BreweryDto> breweries, string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return breweries;

            return breweries
                .Where(b => b.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private List<BreweryDto> SortBreweries(List<BreweryDto> breweries, string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => breweries.OrderBy(b => b.Name).ToList(),
                "city" => breweries.OrderBy(b => b.City).ToList(),
                "distance" => breweries.OrderBy(b => b.Distance).ToList(),
                _ => breweries
            };
        }

        private BreweryDto MapToDto(BreweryDto entity) => new()
        {
            Name = entity.Name,
            City = entity.City,
            Phone = entity.Phone,
            Distance = entity.Distance
        };
    }


}
