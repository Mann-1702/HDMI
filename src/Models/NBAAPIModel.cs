using System.Collections.Generic;

/// <summary>
/// Represents the response structure for an NBA game.
/// Contains details about the game, teams, scores, and status.
/// </summary>
public class NbaGameResponse
{
    /// <summary>
    /// Unique identifier for the game.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The league where the game is played (e.g., "NBA").
    /// </summary>
    public string League { get; set; }

    /// <summary>
    /// The season year for the game.
    /// </summary>
    public int Season { get; set; }

    /// <summary>
    /// Date-related details for the game, including start and end times.
    /// </summary>
    public NbaGameDate Date { get; set; }

    /// <summary>
    /// The stage of the competition (e.g., regular season, playoffs).
    /// </summary>
    public string Stage { get; set; }

    /// <summary>
    /// The current or final status of the game.
    /// </summary>
    public NbaStatus Status { get; set; }

    /// <summary>
    /// Information about the home and visiting teams.
    /// </summary>
    public NbaTeams Teams { get; set; }

    /// <summary>
    /// Scores for the home and visiting teams.
    /// </summary>
    public NbaScores Scores { get; set; }
}

/// <summary>
/// Contains details about the date and time of an NBA game.
/// </summary>
public class NbaGameDate
{
    /// <summary>
    /// The start time of the game.
    /// </summary>
    public string Start { get; set; }

    /// <summary>
    /// The end time of the game.
    /// </summary>
    public string End { get; set; }

    /// <summary>
    /// The total duration of the game.
    /// </summary>
    public string Duration { get; set; }
}

/// <summary>
/// Represents the status of an NBA game.
/// Includes information about the game's progress and halftime status.
/// </summary>
public class NbaStatus
{
    /// <summary>
    /// Short status representation (e.g., "FT" for full-time).
    /// </summary>
    public string Short { get; set; }

    /// <summary>
    /// Detailed description of the game's status.
    /// </summary>
    public string Long { get; set; }

    /// <summary>
    /// The current game clock time, if applicable.
    /// </summary>
    public string Clock { get; set; }

    /// <summary>
    /// Indicates whether the game is at halftime.
    /// </summary>
    public bool Halftime { get; set; }
}

/// <summary>
/// Contains details about the home and visiting teams in an NBA game.
/// </summary>
public class NbaTeams
{
    /// <summary>
    /// Information about the home team.
    /// </summary>
    public NbaTeam Home { get; set; }

    /// <summary>
    /// Information about the visiting team.
    /// </summary>
    public NbaTeam Visitors { get; set; }
}

/// <summary>
/// Represents a team participating in an NBA game.
/// Includes details such as ID, name, and logo.
/// </summary>
public class NbaTeam
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
    /// URL to the team's logo.
    /// </summary>
    public string Logo { get; set; }
}

/// <summary>
/// Represents the scores for both home and visiting teams in an NBA game.
/// Includes win-loss records, points, and quarter-by-quarter scores.
/// </summary>
public class NbaScores
{
    /// <summary>
    /// Scores for the home team.
    /// </summary>
    public NbaScoreDetails Home { get; set; }

    /// <summary>
    /// Scores for the visiting team.
    /// </summary>
    public NbaScoreDetails Visitors { get; set; }
}

/// <summary>
/// Contains detailed scoring information for a team in an NBA game.
/// Includes points, win-loss records, and a breakdown of scores per quarter.
/// </summary>
public class NbaScoreDetails
{
    /// <summary>
    /// Number of games won by the team in the series or season.
    /// Nullable to handle cases where data is unavailable.
    /// </summary>
    public int? Win { get; set; }

    /// <summary>
    /// Number of games lost by the team in the series or season.
    /// Nullable to handle cases where data is unavailable.
    /// </summary>
    public int? Loss { get; set; }

    /// <summary>
    /// Total points scored by the team in the game.
    /// </summary>
    public int? Points { get; set; }

    /// <summary>
    /// Quarter-by-quarter scores as a list of strings.
    /// </summary>
    public List<string> Linescore { get; set; }

    /// <summary>
    /// Series win-loss record for the team.
    /// </summary>
    public SeriesRecord Series { get; set; }
}

/// <summary>
/// Represents the win-loss record for a team in a series.
/// </summary>
public class SeriesRecord
{
    /// <summary>
    /// Number of wins in the series.
    /// Nullable to handle cases where the series is not applicable.
    /// </summary>
    public int? Win { get; set; }

    /// <summary>
    /// Number of losses in the series.
    /// Nullable to handle cases where the series is not applicable.
    /// </summary>
    public int? Loss { get; set; }
}