public class GameResponse
{
    public GameDetails Game { get; set; }
    public League League { get; set; }
    public Teams Teams { get; set; }
    public Scores Scores { get; set; }
}

public class GameDetails
{
    public int Id { get; set; }
    public string Stage { get; set; }
    public string Week { get; set; }
    public GameDate Date { get; set; }
    public Venue Venue { get; set; }
    public Status Status { get; set; }
}

public class GameDate
{
    public string Timezone { get; set; }
    public string Date { get; set; }  // The "date" property in JSON
    public string Time { get; set; }
    public long Timestamp { get; set; }
}

public class Venue
{
    public string Name { get; set; }
    public string City { get; set; }
}

public class Status
{
    public string Short { get; set; }
    public string Long { get; set; }
    public string Timer { get; set; }
}

public class League
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Season { get; set; }
    public string Logo { get; set; }
    public Country Country { get; set; }
}

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Flag { get; set; }
}

public class Teams
{
    public Team Home { get; set; }
    public Team Away { get; set; }
}

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
}

public class Scores
{
    public ScoreDetails Home { get; set; }
    public ScoreDetails Away { get; set; }
}

public class ScoreDetails
{
    public int? Quarter_1 { get; set; }
    public int? Quarter_2 { get; set; }
    public int? Quarter_3 { get; set; }
    public int? Quarter_4 { get; set; }
    public int? Overtime { get; set; }
    public int? Total { get; set; }
}