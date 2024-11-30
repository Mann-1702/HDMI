using Newtonsoft.Json;

/// <summary>
/// Represents the standing of a team within a league, including its position, performance, and related statistics.
/// </summary>
public class TeamStanding
{
    /// <summary>
    /// The conference in which the team competes (e.g., "Eastern", "Western").
    /// </summary>
    public string Conference { get; set; }

    /// <summary>
    /// The division in which the team competes (e.g., "Atlantic", "Pacific").
    /// </summary>
    public string Division { get; set; }

    /// <summary>
    /// The current position or rank of the team in the standings.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Information about the team, including the team ID, name, and logo.
    /// </summary>
    [JsonProperty("team")]
    public Team1 Team1 { get; set; }

    /// <summary>
    /// The number of games the team has won.
    /// </summary>
    public int Won { get; set; }

    /// <summary>
    /// The number of games the team has lost.
    /// </summary>
    public int Lost { get; set; }

    /// <summary>
    /// The number of games that ended in a tie for the team.
    /// </summary>
    public int Ties { get; set; }

    /// <summary>
    /// The points scored by the team, including goals for, goals against, and goal difference.
    /// </summary>
    public Points Points { get; set; }

    /// <summary>
    /// The team's performance in different contexts, such as home games, away games, conference, and division.
    /// </summary>
    public Records Records { get; set; }

    /// <summary>
    /// The team's current winning or losing streak.
    /// </summary>
    public string Streak { get; set; }
}

/// <summary>
/// Represents the basic information about a team, including its ID, name, and logo.
/// </summary>
public class Team1
{
    /// <summary>
    /// Unique identifier for the team.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The full name of the team (e.g., "Los Angeles Lakers").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// URL to the team's logo image.
    /// </summary>
    public string Logo { get; set; }
}

/// <summary>
/// Represents the points scored by the team, including goals for, goals against, and goal difference.
/// </summary>
public class Points
{
    /// <summary>
    /// The total number of goals the team has scored.
    /// </summary>
    public int For { get; set; }

    /// <summary>
    /// The total number of goals the team has conceded.
    /// </summary>
    public int Against { get; set; }

    /// <summary>
    /// The goal difference (goals for minus goals against).
    /// </summary>
    public int Difference { get; set; }
}

/// <summary>
/// Represents the team's performance in different contexts: home games, away games, conference, and division.
/// </summary>
public class Records
{
    /// <summary>
    /// The team's performance in home games (e.g., "W-W-L").
    /// </summary>
    public string Home { get; set; }

    /// <summary>
    /// The team's performance in away games (e.g., "L-W-W").
    /// </summary>
    public string Road { get; set; }

    /// <summary>
    /// The team's performance within its conference (e.g., "W-W-L").
    /// </summary>
    public string Conference { get; set; }

    /// <summary>
    /// The team's performance within its division (e.g., "L-W-W").
    /// </summary>
    public string Division { get; set; }
}