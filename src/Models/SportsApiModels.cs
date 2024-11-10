

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
    public Period Periods { get; set; }
}

public class GameDate
{
    public string Timezone { get; set; }
    public string Date { get; set; }  
    public string Time { get; set; }
    public long Timestamp { get; set; }
    public string Start {  get; set; }
    public string End { get; set; } 
    public string Duration { get; set; }
}

public class Venue
{
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}

public class Status
{
    public string Short { get; set; }
    public string Long { get; set; }
    public string Timer { get; set; }
    public string Clock { get; set; }
    public bool HalfTime { get; set; }
}
public class Period
{
    public int Current { get; set; }     
    public int Total { get; set; }        
    public bool EndOfPeriod { get; set; } 
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
    public Team Visitors { get; set; }
}

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Nickname { get; set; }  
    public string Code { get; set; }
}

public class Scores
{
    public ScoreDetails Home { get; set; }
    public ScoreDetails Away { get; set; }
    public ScoreDetails Visitors { get; set; }
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