using System.ComponentModel.DataAnnotations;

namespace BreweryApi.Models
{
   
    public class BreweryDto
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public double? Distance { get; set; }
    }
  

    public class SearchRequest
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
    }
   
}
