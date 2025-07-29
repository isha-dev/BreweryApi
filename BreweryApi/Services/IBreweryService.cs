using BreweryApi.Models;

namespace BreweryApi.Services
{
    public interface IBreweryService
    {
        Task<IEnumerable<BreweryDto>> GetBreweriesAsync(string? search = null, string? sortBy = null);

    }
}
