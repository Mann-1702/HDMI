using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class FixtureResponse
{
    public LeagueInfo League { get; set; } 
    public int Season { get; set; }
    public FixtureTeams Teams { get; set; }
    public FixtureScores Scores { get; set; }

    [JsonProperty("date")]
    public FixtureDate Date { get; set; }

    [JsonProperty("status")]
    public FixtureStatus Status { get; set; }
    public string Stage { get; set; }
}

public class LeagueInfo 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
}

public class FixtureDate
{
    public string Start { get; set; }
    public string Timezone { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public long Timestamp { get; set; }
}

public class FixtureStatus
{
    public string Short { get; set; }
    public string Long { get; set; }
    public string Clock { get; set; }
    public bool Halftime { get; set; }
}

public class FixtureTeams
{
    public TeamE Home { get; set; }
    public TeamE Visitors { get; set; }
}

public class TeamE
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Code { get; set; }
    public string Logo { get; set; }
}

public class FixtureScores
{
    public ScoreDetails1 Home { get; set; }
    public ScoreDetails1 Visitors { get; set; }
}

public class ScoreDetails1
{
    public int? Points { get; set; }                   // Total points scored by the team
    public List<string> Linescore { get; set; }        // Period-by-period scores as a list of strings
    public SeriesRecord1 Series { get; set; }          // Series win-loss record, if applicable
}

public class SeriesRecord1
{
    public int? Win { get; set; }
    public int? Loss { get; set; }
}
