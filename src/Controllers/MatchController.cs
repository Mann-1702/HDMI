using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    // Marks the class as an API controller, making it part of the Web API framework.
    [ApiController]

    // Specifies the route pattern for this controller: "Match".
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        // Constructor that accepts a JsonFileMatchService dependency through dependency injection.
        // This service is used to manage match-related data.
        public MatchController(JsonFileMatchService matchService)
        {
            MatchService = matchService; // Assign the injected service to a property for later use.
        }

        // Property to hold the instance of JsonFileMatchService.
        // This service provides methods to retrieve match data from a JSON file.
        public JsonFileMatchService MatchService { get; }

        // HTTP GET method to retrieve a list of all matches.
        // Returns an IEnumerable<MatchModel>, which is a collection of match data.
        [HttpGet]
        public IEnumerable<MatchModel> Get()
        {
            // Uses the MatchService to fetch all match data and return it.
            return MatchService.GetAllData();
        }
    }
}