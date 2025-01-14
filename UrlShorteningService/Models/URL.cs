﻿using System.ComponentModel.DataAnnotations;

namespace UrlShorteningService.Models
{
    public class URL
    {
        [Key]
        public int id { get; set; }
        public required string url { get; set; }

        public string? shortCode { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        public int accessCount { get; set; } = 0;

    }
}
