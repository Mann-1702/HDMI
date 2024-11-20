using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class FixtureResponse
{

    [JsonProperty("fixture")]
    public FixtureDetails Fixture { get; set; } // New property for fixture details

    [JsonProperty("league")]
    public LeagueInfo League { get; set; }

    [JsonProperty("season")]
    public int Season { get; set; }

    [JsonProperty("teams")]
    public FixtureTeams Teams { get; set; }

    [JsonProperty("score")]
    public FixtureScores Scores { get; set; }

    [JsonProperty("date")]
    public FixtureDate Date { get; set; }

    [JsonProperty("status")]
    public FixtureStatus Status { get; set; }

    [JsonProperty("stage")]
    public string Stage { get; set; }
}

public class FixtureDetails
{
    [JsonProperty("id")]
    public int FixtureId { get; set; }


    [JsonProperty("status")]
    public FixtureStatus Status { get; set; }
}

public class LeagueInfo
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }
}

public class FixtureDate
{
    [JsonProperty("start")]
    public string Start { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    [JsonProperty("datetime")]

    public DateTimeOffset DateTime { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }
}

public class FixtureStatus
{
    [JsonProperty("short")]
    public string Short { get; set; }

    [JsonProperty("long")]
    public string Long { get; set; }

    [JsonProperty("clock")]
    public string Clock { get; set; }

    [JsonProperty("halftime")]
    public bool Halftime { get; set; }
}

public class FixtureTeams
{
    [JsonProperty("home")]
    public TeamE Home { get; set; }

    [JsonProperty("away")]
    public TeamE Visitors { get; set; }
}

public class TeamE
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("logo")]
    public string Logo { get; set; }
}

public class FixtureScores
{
    [JsonProperty("halftime")]
    public ScoreDetails1 Halftime { get; set; }

    [JsonProperty("fulltime")]
    public ScoreDetails1 Fulltime { get; set; }

    [JsonProperty("extratime")]
    public ScoreDetails1 ExtraTime { get; set; }

    [JsonProperty("penalty")]
    public ScoreDetails1 Penalty { get; set; }
}

public class ScoreDetails1
{
    [JsonProperty("home")]
    public int? Home { get; set; }

    [JsonProperty("away")]
    public int? Away { get; set; }
}

