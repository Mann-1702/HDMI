using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Maker { get; set; }

        [JsonPropertyName("img")]
        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string Image { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public int[] Ratings { get; set; }

        public ProductTypeEnum ProductType { get; set; } = ProductTypeEnum.Undefined;

        public string Sport { get; set; }

        [Range(1800, 2024, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int FoundingYear { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Trophies { get; set; }

        // Store the Comments entered by the users on this product
        public List<CommentModel> CommentList { get; set; } = new List<CommentModel>();

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}
