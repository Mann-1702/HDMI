using System;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents a match in a sports event, containing information about teams, location, date, and scores.
    /// </summary>
    public class MatchModel
    {
        /// <summary>
        /// The name or identifier of the match.
        /// Required field.
        /// </summary>
        [Required]
        public string Match { get; set; }

        /// <summary>
        /// The date when the match is scheduled or took place.
        /// This property is required and uses a Date data type for validation.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        /// <summary>
        /// The location where the match is held.
        /// This property is required and should have a length between 3 and 100 characters.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Location should have a length of more than {2} and less than {1}")]
        public string Location { get; set; }

        /// <summary>
        /// Name of the first team participating in the match.
        /// Optional field.
        /// </summary>
        public string Team1 { get; set; }

        /// <summary>
        /// Name of the second team participating in the match.
        /// Optional field.
        /// </summary>
        public string Team2 { get; set; }

        /// <summary>
        /// Score of the first team in the match.
        /// </summary>
        public int Team1_Score { get; set; }

        /// <summary>
        /// Score of the second team in the match.
        /// </summary>
        public int Team2_Score { get; set; }

        /// <summary>
        /// Serializes the current MatchModel object to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the MatchModel object.</returns>
        public override string ToString() => JsonSerializer.Serialize<MatchModel>(this);
    }
}