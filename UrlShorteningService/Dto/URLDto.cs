using System.ComponentModel.DataAnnotations;

namespace UrlShorteningService.Dto
{
    public class URLDto
    {
        public required string url { get; set; } 
    }

    public class URLStandar
    {
        public int id { get; set; }
        public required string url { get; set; }

        public string? shortCode { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}
