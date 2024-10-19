using Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Function.Dtos
{
    public class AddEntryDto
    {
        [Required]
        [JsonPropertyName("userGuid")]
        public Guid UserGuid { get; set; }
        [Required]
        [JsonPropertyName("entries")]
        public List<string> Entries { get; set; } = new List<string>();
        [Required]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
