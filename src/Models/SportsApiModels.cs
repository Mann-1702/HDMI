public class GameResponse
{
    // Contains the details of the game, including the status, teams, and scores.
    public GameDetails Game { get; set; }

    // Contains the league information such as name, ID, and season details.
    public League League { get; set; }

    // Contains details of the teams involved in the game.
    public Teams Teams { get; set; }

    // Contains the score details for the game.
    public Scores Scores { get; set; }
}

public class GameDetails
{
    // Unique identifier for the game.
    public int Id { get; set; }

    // The stage or round of the competition (e.g., regular season, playoffs).
    public string Stage { get; set; }

    // The week of the season this game is part of.
    public string Week { get; set; }

    // The date and time information of the game.
    public GameDate Date { get; set; }

    // The venue where the game is being played.
    public Venue Venue { get; set; }

    // The status of the game (e.g., in progress, completed).
    public Status Status { get; set; }

    // Information about the periods (quarters) in the game.
    public Period Periods { get; set; }
}

public class GameDate
{
    // Timezone in which the game is played.
    public string Timezone { get; set; }

    // The date the game is scheduled to take place.
    public string Date { get; set; }

    // The time the game is scheduled to begin.
    public string Time { get; set; }

    // Timestamp for the start time of the game.
    public long Timestamp { get; set; }

    // Start time of the game in string format.
    public string Start { get; set; }

    // End time of the game in string format.
    public string End { get; set; }

    // Duration of the game.
    public string Duration { get; set; }
}

public class Venue
{
    // Name of the venue where the game is being played.
    public string Name { get; set; }

    // The city where the venue is located.
    public string City { get; set; }

    // The state where the venue is located.
    public string State { get; set; }

    // The country where the venue is located.
    public string Country { get; set; }
}

public class Status
{
    // Short status description (e.g., "In Progress").
    public string Short { get; set; }

    // Long status description (e.g., "Game is in progress").
    public string Long { get; set; }

    // Timer for the game, indicating how much time is left in the current period.
    public string Timer { get; set; }

    // The game clock time.
    public string Clock { get; set; }

    // Boolean indicating whether it is halftime.
    public bool HalfTime { get; set; }
}

public class Period
{
    // Current period or quarter in the game.
    public int Current { get; set; }

    // Total number of periods (quarters) in the game.
    public int Total { get; set; }

    // Boolean indicating whether the period has ended.
    public bool EndOfPeriod { get; set; }
}

public class League
{
    // Unique identifier for the league.
    public int Id { get; set; }

    // The name of the league (e.g., NBA, NFL).
    public string Name { get; set; }

    // The season year of the league (e.g., 2023-2024).
    public int Season { get; set; }

    // The logo of the league.
    public string Logo { get; set; }

    // Country information for the league.
    public Country Country { get; set; }
}

public class Country
{
    // Name of the country where the league is based.
    public string Name { get; set; }

    // Country code (e.g., "US" for United States).
    public string Code { get; set; }

    // URL to the country's flag image.
    public string Flag { get; set; }
}

public class Teams
{
    // Home team information.
    public Team Home { get; set; }

    // Away team information.
    public Team Away { get; set; }

    // Visitors team information (alternative to Away team).
    public Team Visitors { get; set; }
}

public class Team
{
    // Unique identifier for the team.
    public int Id { get; set; }

    // Name of the team (e.g., Los Angeles Lakers).
    public string Name { get; set; }

    // URL of the team's logo image.
    public string Logo { get; set; }

    // Nickname for the team (e.g., Lakers).
    public string Nickname { get; set; }

    // Code identifying the team (used for team abbreviations, etc.).
    public string Code { get; set; }
}

public class Scores
{
    // Score details for the home team.
    public ScoreDetails Home { get; set; }

    // Score details for the away team.
    public ScoreDetails Away { get; set; }

    // Score details for the visitors team (if applicable).
    public ScoreDetails Visitors { get; set; }
}

public class ScoreDetails
{
    // Score in the first quarter.
    public int? Quarter_1 { get; set; }

    // Score in the second quarter.
    public int? Quarter_2 { get; set; }

    // Score in the third quarter.
    public int? Quarter_3 { get; set; }

    // Score in the fourth quarter.
    public int? Quarter_4 { get; set; }

    // Score in overtime, if applicable.
    public int? Overtime { get; set; }

    // Total score of the team.
    public int? Total { get; set; }
}