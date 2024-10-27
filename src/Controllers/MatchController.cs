using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        public MatchController(JsonFileMatchService matchService)
        {
            MatchService = matchService;
        }

        public JsonFileMatchService MatchService { get; }

        [HttpGet]
        public IEnumerable<MatchModel> Get()
        {
            return MatchService.GetAllData();
        }
    }

}