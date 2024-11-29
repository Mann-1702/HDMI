using Newtonsoft.Json;


public class NBAStanding
{
    public string League { get; set; }
    public int Season { get; set; }
    [JsonProperty("team")]
    public Team2 Team2 { get; set; }
    public Conference Conference { get; set; }
    public Division Division { get; set; }
    public WinLoss Win { get; set; }
    public WinLoss Loss { get; set; }
    public string GamesBehind { get; set; }
    public int Streak { get; set; }
    public bool WinStreak { get; set; }
    public string TieBreakerPoints { get; set; }
}

public class Team2
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string Code { get; set; }
    public string Logo { get; set; }
}

public class Conference
{
    public string Name { get; set; }
    public int Rank { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
}

public class Division
{
    public string Name { get; set; }
    public int Rank { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
    public string GamesBehind { get; set; }
}

public class WinLoss
{
    public int Home { get; set; }
    public int Away { get; set; }
    public int Total { get; set; }
    public string Percentage { get; set; }
    public int LastTen { get; set; }
}
