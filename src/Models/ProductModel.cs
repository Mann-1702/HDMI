using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents a product with various details, including its maker, images, URLs, ratings, and associated metadata.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// URL of the product image.
        /// </summary>
        [Required(ErrorMessage = "Image URL is required.")]
        [JsonPropertyName("img")]
        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string Image { get; set; }

        /// <summary>
        /// URL of the product's website or information page.
        /// </summary>
        [Required(ErrorMessage = "URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Url { get; set; }

        /// <summary>
        /// The title of the product.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}.")]
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        /// <summary>
        /// Array storing ratings provided for the product.
        /// </summary>
        public int[] Ratings { get; set; }

        /// <summary>
        /// Type of the product, specified as an enum.
        /// Defaults to Undefined if not specified.
        /// </summary>
        public ProductTypeEnum ProductType { get; set; } = ProductTypeEnum.Undefined;

        /// <summary>
        /// The sport category to which this product is related.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SportsEnum Sport { get; set; } = SportsEnum.Undefined;

        /// <summary>
        /// The founding year of the product or company.
        /// </summary>
        [Range(1800, 2024, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int FoundingYear { get; set; }

        /// <summary>
        /// The number of trophies or awards the product has won.
        /// </summary>
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Trophies { get; set; }

        /// <summary>
        /// List of comments provided by users for this product.
        /// </summary>
        public List<CommentModel> CommentList { get; set; } = new List<CommentModel>();

        /// <summary>
        /// Serializes the current ProductModel object to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the ProductModel object.</returns>
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}