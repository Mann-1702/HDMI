using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Enumeration representing different types of products.
    /// </summary>
    public enum ProductTypeEnum
    {
        /// <summary>
        /// Undefined product type, used as the default.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Represents products related to sports or sport leagues.
        /// </summary>
        Sport = 1,

        /// <summary>
        /// Represents products related to sports teams.
        /// </summary>
        Team = 2,
    }

    /// <summary>
    /// Extension methods for the ProductTypeEnum enum, used to display friendly names.
    /// </summary>
    public static class ProductTypeEnumExtensions
    {
        /// <summary>
        /// Gets the display name for a given ProductTypeEnum value.
        /// </summary>
        /// <param name="data">The ProductTypeEnum value.</param>
        /// <returns>A string representing the friendly display name for the enum value.</returns>
        public static string DisplayName(this ProductTypeEnum data)
        {
            return data switch
            {
                ProductTypeEnum.Sport => "Sport / Sport League",  // Friendly name for Sport
                ProductTypeEnum.Team => "Sport Team",             // Friendly name for Team

                // Default case for undefined or unknown values
                _ => "",
            };
        }
    }
}