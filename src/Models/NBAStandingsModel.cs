using Newtonsoft.Json;

/// <summary>
/// Represents the standings information for an NBA team within a league and season.
/// Contains details about the team's performance, conference, and division rankings.
/// </summary>
public class NBAStanding
{
    /// <summary>
    /// The league to which the team belongs (e.g., "NBA").
    /// </summary>
    public string League { get; set; }

    /// <summary>
    /// The season year for the standings (e.g., 2023-2024).
    /// </summary>
    public int Season { get; set; }

    /// <summary>
    /// Information about the team, including name, nickname, logo, etc.
    /// </summary>
    [JsonProperty("team")]
    public Team2 Team2 { get; set; }

    /// <summary>
    /// The conference the team belongs to (e.g., Eastern, Western).
    /// </summary>
    public Conference Conference { get; set; }

    /// <summary>
    /// The division the team is a part of (e.g., Pacific, Atlantic).
    /// </summary>
    public Division Division { get; set; }

    /// <summary>
    /// The team's win record for the season.
    /// </summary>
    public WinLoss Win { get; set; }

    /// <summary>
    /// The team's loss record for the season.
    /// </summary>
    public WinLoss Loss { get; set; }

    /// <summary>
    /// The number of games behind the leading team in the standings.
    /// </summary>
    public string GamesBehind { get; set; }

    /// <summary>
    /// The team's current winning streak.
    /// </summary>
    public int Streak { get; set; }

    /// <summary>
    /// Whether the team is on a winning streak.
    /// </summary>
    public bool WinStreak { get; set; }

    /// <summary>
    /// Points used to break ties in the standings, if applicable.
    /// </summary>
    public string TieBreakerPoints { get; set; }
}

/// <summary>
/// Represents a team in the NBA standings, including basic details like name, logo, and team ID.
/// </summary>
public class Team2
{
    /// <summary>
    /// Unique identifier for the team.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the team (e.g., "Los Angeles Lakers").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Nickname of the team (e.g., "Lakers").
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Short code or abbreviation for the team (e.g., "LAL").
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// URL to the team's logo image.
    /// </summary>
    public string Logo { get; set; }
}

/// <summary>
/// Represents the conference standings for a team, including win-loss records and rankings.
/// </summary>
public class Conference
{
    /// <summary>
    /// The name of the conference (e.g., "Eastern", "Western").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The rank of the team within the conference.
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// Number of wins for the team within the conference.
    /// </summary>
    public int Win { get; set; }

    /// <summary>
    /// Number of losses for the team within the conference.
    /// </summary>
    public int Loss { get; set; }
}

/// <summary>
/// Represents the division standings for a team, including win-loss records and rankings.
/// </summary>
public class Division
{
    /// <summary>
    /// The name of the division (e.g., "Pacific", "Atlantic").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The rank of the team within the division.
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// Number of wins for the team within the division.
    /// </summary>
    public int Win { get; set; }

    /// <summary>
    /// Number of losses for the team within the division.
    /// </summary>
    public int Loss { get; set; }

    /// <summary>
    /// The number of games the team is behind the division leader.
    /// </summary>
    public string GamesBehind { get; set; }
}

/// <summary>
/// Represents the win-loss record for a team in a specific context (e.g., overall, conference, division).
/// </summary>
public class WinLoss
{
    /// <summary>
    /// The number of wins for the team at home.
    /// </summary>
    public int Home { get; set; }

    /// <summary>
    /// The number of wins for the team away from home.
    /// </summary>
    public int Away { get; set; }

    /// <summary>
    /// The total number of wins for the team.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// The win percentage for the team.
    /// </summary>
    public string Percentage { get; set; }

    /// <summary>
    /// The team's performance in the last ten games (e.g., 7-3).
    /// </summary>
    public int LastTen { get; set; }
}