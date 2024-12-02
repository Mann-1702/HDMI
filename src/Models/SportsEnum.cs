namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Enumeration representing different types of sports.
    /// </summary>
    public enum SportsEnum
    {
        Undefined = 0,
        NFL = 1,
        NBA = 2,
        Soccer = 3
    }

    public static class SportsEnumExtensions
    {
        public static string ToDisplayString(this SportsEnum sport) =>
            sport switch
            {
                SportsEnum.NFL => "NFL",
                SportsEnum.NBA => "NBA",
                SportsEnum.Soccer => "Soccer",
                _ => "Undefined"
            };
    }

}