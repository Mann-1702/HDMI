using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileMatchService
    {

        public JsonFileMatchService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }


        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "matches.json"); }
        }

        public IEnumerable<MatchModel> GetAllData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<MatchModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }
        public MatchModel UpdateData(MatchModel data)
        {
            var matches = GetAllData();
            var matchData = matches.FirstOrDefault(x => x.Match.Equals(data.Match));

            if (matchData == null)
            {
                return null;
            }

            // Update the data to the new passed in values
            matchData.Match = data.Match;
            matchData.Date = data.Date;
            matchData.Location = data.Location;
    

            SaveData(matches);

            return matchData;
        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<MatchModel> matches)
        {

            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<MatchModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    matches
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public MatchModel CreateData()
        {
            var data = new MatchModel()
            {
                Match = "Enter Match",
                Date = new DateTime(),
                Location = "Enter Location",
            };

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return data;
        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public MatchModel DeleteData(string id)
        {
            // Get the current set, and append the new record to it
            var dataSet = GetAllData();
            var data = dataSet.FirstOrDefault(m => m.Match.Equals(id));

            var newDataSet = GetAllData().Where(m => m.Match.Equals(id) == false);

            SaveData(newDataSet);

            return data;
        }


    }
}
