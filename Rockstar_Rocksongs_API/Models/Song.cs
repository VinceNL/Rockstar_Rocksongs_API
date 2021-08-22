using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Rockstar_RockSongs_API.Models
{
    public class Song
    {       
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        public int? Year { get; set; }

        [Required]
        public string Artist { get; set; }

        public string Shortname { get; set; }

        public int? Bpm { get; set; }

        public int Duration { get; set; }

        [Required]
        public string Genre { get; set; }

        public string SpotifyId { get; set; }

        public string Album { get; set; }

        [JsonIgnore]
        public DateTime? InsertDate { get; set; }
    }
}
