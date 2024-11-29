using Newtonsoft.Json;

public class TeamStanding
{
    public string Conference { get; set; }
    public string Division { get; set; }
    public int Position { get; set; }
    [JsonProperty("team")]
    public Team1 Team1 { get; set; }
    public int Won { get; set; }
    public int Lost { get; set; }
    public int Ties { get; set; }
    public Points Points { get; set; }
    public Records Records { get; set; }
    public string Streak { get; set; }
}

public class Team1
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
}

public class Points
{
    public int For { get; set; }
    public int Against { get; set; }
    public int Difference { get; set; }
}

public class Records
{
    public string Home { get; set; }
    public string Road { get; set; }
    public string Conference { get; set; }
    public string Division { get; set; }
}
