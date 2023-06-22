using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EuroJack
{
    public class PlanetApi
    {

        public PlanetApi()
        {

        }

        public RespPlanets GetCoordinate(DateTime forTime, string[] planets)
        {
            if (planets == null || planets.Length == 0)
            {
                planets = new string[] { "Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto" };
            }

            return GetDataAsync(new ReqData() { DetectTime = forTime, Planets = planets }).Result;
        }

        private async Task<RespPlanets> GetDataAsync(ReqData requestData)
        {
            RespPlanets result = null;
            var baseAddress = new Uri("http://ephemeris.kibo.cz/api/v1/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("planets", content))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        result = JsonConvert.DeserializeObject<RespPlanets>(responseData);
                    }
                }
            }

            return result;
        }
    }

    public class ReqData
    {
        public DateTime DetectTime { get; set; }
        public string Event => $"{DetectTime.ToString("YYYYMMDDhhmmss")}";
        public string[] Planets { get; set; }

    }

    public class RespPlanets
    {
        public List<Planet> Planets { get; set; }
    }

    public class Planet
    {
        public string Name { get; set; }
        public double[] Coordinate { get; set; }
    }
}
