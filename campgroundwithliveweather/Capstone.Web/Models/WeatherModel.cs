using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class WeatherModel
    {
        public DateTime ActualDate { private get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Forecast { private get; set; }
        public string WeatherAdvisory
        {
            get {
                if (advisory.ContainsKey(GetForcast()))
                    return advisory[GetForcast()];
                else return null;
            }
        }
        public string TempAdvisory { get; set; }
        public string GetDate()
        {
            return ActualDate.ToString("MM/dd/yyyy");
        }

        public string GetForcast()
        {
            return forcastconvert[Forecast];
        }

        private Dictionary<string, string> forcastconvert = new Dictionary<string, string>()
        {
            {"clear-day", "sunny" },
            {"clear-night", "sunny" },
            {"rain", "rain" },
            {"snow", "snow" },
            {"sleet", "snow" },
            {"wind", "partlyCloudy" },
            {"fog", "partlyCloudy" },
            {"cloudy", "cloudy" },
            {"partly-cloudy-day", "partlyCloudy" },
            {"partly-cloudy-night", "partlyCloudy" },
            {"hail", "snow" },
            {"thunderstorm", "thunderstorms" },
            {"tornado", "thunderstorms" },
        };

        private Dictionary<string, string> advisory = new Dictionary<string, string>()
        {
            {"snow", "Pack snowshoes!"},
            {"sunny", "Pack sunblock!"},
            {"rain", "Pack rain gear and wear waterproof shoes!"},
            {"thunderstorms", "Seek shelter and avoid hiking on exposed ridges!"}
        };
    }
}
