namespace ContosoCrafts.WebSite.Models
{
    public class NbaGameResponse
    {
        public int Id { get; set; }
        public string League { get; set; }
        public int Season { get; set; }
        public NbaGameDate Date { get; set; }
        public string Stage { get; set; }
        public NbaStatus Status { get; set; }
        public NbaTeams Teams { get; set; }
        public NbaScores Scores { get; set; }
    }

    public class NbaGameDate
    {
        public string Start { get; set; }    
        public string End { get; set; }      
        public string Duration { get; set; } 
    }

    public class NbaStatus
    {
        public string Short { get; set; }  
        public string Long { get; set; }   
        public string Clock { get; set; }  
        public bool Halftime { get; set; } 
    }

    public class NbaTeams
    {
        public NbaTeam Home { get; set; }
        public NbaTeam Visitors { get; set; }
    }

    public class NbaTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Code { get; set; }   
        public string Logo { get; set; }   
    }

    public class NbaScores
    {
        public NbaScoreDetails Home { get; set; }
        public NbaScoreDetails Visitors { get; set; }
    }

    public class NbaScoreDetails
    {
        public int? Quarter1 { get; set; } 
        public int? Quarter2 { get; set; }
        public int? Quarter3 { get; set; }
        public int? Quarter4 { get; set; }
        public int? Overtime { get; set; } 
        public int? Total { get; set; }    
    }
}


