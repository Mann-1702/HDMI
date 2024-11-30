using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents the main response structure for a fixture.
/// Contains details about the fixture, league, teams, scores, and more.
/// </summary>
public class FixtureResponse
{
    /// <summary>
    /// Details of the fixture (e.g., fixture ID and status).
    /// </summary>
    [JsonProperty("fixture")]
    public FixtureDetails Fixture { get; set; }

    /// <summary>
    /// Information about the league the fixture is part of.
    /// </summary>
    [JsonProperty("league")]
    public LeagueInfo League { get; set; }

    /// <summary>
    /// The season year for the fixture.
    /// </summary>
    [JsonProperty("season")]
    public int Season { get; set; }

    /// <summary>
    /// Information about the home and away teams participating in the fixture.
    /// </summary>
    [JsonProperty("teams")]
    public FixtureTeams Teams { get; set; }

    /// <summary>
    /// Scores recorded for the fixture at various stages (e.g., halftime, fulltime).
    /// </summary>
    [JsonProperty("score")]
    public FixtureScores Scores { get; set; }

    /// <summary>
    /// Details about the date and time of the fixture.
    /// </summary>
    [JsonProperty("date")]
    public FixtureDate Date { get; set; }

    /// <summary>
    /// The status of the fixture (e.g., live, completed, or postponed).
    /// </summary>
    [JsonProperty("status")]
    public FixtureStatus Status { get; set; }

    /// <summary>
    /// The stage of the competition the fixture belongs to (e.g., group stage, knockout stage).
    /// </summary>
    [JsonProperty("stage")]
    public string Stage { get; set; }
}

/// <summary>
/// Provides basic details about a fixture, such as its ID and status.
/// </summary>
public class FixtureDetails
{
    /// <summary>
    /// Unique identifier for the fixture.
    /// </summary>
    [JsonProperty("id")]
    public int FixtureId { get; set; }

    /// <summary>
    /// Status information about the fixture.
    /// </summary>
    [JsonProperty("status")]
    public FixtureStatus Status { get; set; }
}

/// <summary>
/// Represents league-specific information for a fixture.
/// </summary>
public class LeagueInfo
{
    /// <summary>
    /// Unique identifier for the league.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the league.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// The country where the league is based.
    /// </summary>
    [JsonProperty("country")]
    public string Country { get; set; }
}

/// <summary>
/// Contains details about the fixture's date and time.
/// </summary>
public class FixtureDate
{
    /// <summary>
    /// The start date and time of the fixture in string format.
    /// </summary>
    [JsonProperty("start")]
    public string Start { get; set; }

    /// <summary>
    /// The timezone of the fixture's start time.
    /// </summary>
    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    /// <summary>
    /// The fixture's start time as a DateTimeOffset object.
    /// </summary>
    [JsonProperty("datetime")]
    public DateTimeOffset DateTime { get; set; }

    /// <summary>
    /// Timestamp representation of the fixture's start time.
    /// </summary>
    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }
}

/// <summary>
/// Represents the status of a fixture, including its progress and specific clock time.
/// </summary>
public class FixtureStatus
{
    /// <summary>
    /// Short representation of the fixture's status (e.g., "FT" for full-time).
    /// </summary>
    [JsonProperty("short")]
    public string Short { get; set; }

    /// <summary>
    /// Detailed description of the fixture's status.
    /// </summary>
    [JsonProperty("long")]
    public string Long { get; set; }

    /// <summary>
    /// Current time on the match clock, if applicable.
    /// </summary>
    [JsonProperty("clock")]
    public string Clock { get; set; }

    /// <summary>
    /// Indicates whether the fixture is currently at halftime.
    /// </summary>
    [JsonProperty("halftime")]
    public bool Halftime { get; set; }
}

/// <summary>
/// Details about the home and away teams in a fixture.
/// </summary>
public class FixtureTeams
{
    /// <summary>
    /// Information about the home team.
    /// </summary>
    [JsonProperty("home")]
    public TeamE Home { get; set; }

    /// <summary>
    /// Information about the away (visiting) team.
    /// </summary>
    [JsonProperty("away")]
    public TeamE Visitors { get; set; }
}

/// <summary>
/// Represents a team entity with details like ID, name, and logo.
/// </summary>
public class TeamE
{
    /// <summary>
    /// Unique identifier for the team.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the team.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// City where the team is based.
    /// </summary>
    [JsonProperty("city")]
    public string City { get; set; }

    /// <summary>
    /// Short code or abbreviation for the team.
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    /// <summary>
    /// URL to the team's logo.
    /// </summary>
    [JsonProperty("logo")]
    public string Logo { get; set; }
}

/// <summary>
/// Represents the scores recorded for the fixture at various stages.
/// </summary>
public class FixtureScores
{
    /// <summary>
    /// Scores at halftime.
    /// </summary>
    [JsonProperty("halftime")]
    public ScoreDetails1 Halftime { get; set; }

    /// <summary>
    /// Scores at fulltime.
    /// </summary>
    [JsonProperty("fulltime")]
    public ScoreDetails1 Fulltime { get; set; }

    /// <summary>
    /// Scores at the end of extra time, if applicable.
    /// </summary>
    [JsonProperty("extratime")]
    public ScoreDetails1 ExtraTime { get; set; }

    /// <summary>
    /// Scores after penalties, if applicable.
    /// </summary>
    [JsonProperty("penalty")]
    public ScoreDetails1 Penalty { get; set; }
}

/// <summary>
/// Represents scores for the home and away teams at a specific stage of the match.
/// </summary>
public class ScoreDetails1
{
    /// <summary>
    /// Score for the home team.
    /// Nullable to account for unplayed stages.
    /// </summary>
    [JsonProperty("home")]
    public int? Home { get; set; }

    /// <summary>
    /// Score for the away team.
    /// Nullable to account for unplayed stages.
    /// </summary>
    [JsonProperty("away")]
    public int? Away { get; set; }
}