using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Models
{
    public enum ProductTypeEnum
    {
        Undefined = 0,
        Sport = 1,
        Team = 2,
    }

    public static class ProductTypeEnumExtensions
    {
        public static string DisplayName(this ProductTypeEnum data)
        {
            return data switch
            {
                ProductTypeEnum.Sport => "Sport / Sport League",
                ProductTypeEnum.Team => "Sport Team",
 
                // Default, Unknown
                _ => "",
            };

        }

    }

}