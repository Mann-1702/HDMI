using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    public class MatchModel
    {
        [Required]
        public string Match { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Location should have a length of more than {2} and less than {1}")]
        public string Location { get; set; }

        public string Team1 { get; set; }

        public string Team2 { get; set; }

        public int Team1_Score { get; set; }

        public int Team2_Score { get; set; }


        public override string ToString() => JsonSerializer.Serialize<MatchModel>(this);
    }

}