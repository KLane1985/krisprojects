using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Capstone.Web.Models;
using Newtonsoft.Json;
using static Capstone.Web.Models.weatherJSON;

namespace Capstone.Web.DAL
{
    public class ParksqlDAL : IParkDAL
    {
        private readonly string connectionString;

        public ParksqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IList<WeatherModel> weathersList = new List<WeatherModel>();
        public IList<ParkModel> GetParks()
        {
            IList<ParkModel> parks = new List<ParkModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM park", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ParkModel park = new ParkModel();

                    park.ParkCode = Convert.ToString(reader["parkCode"]);
                    park.ParkName = Convert.ToString(reader["parkName"]);
                    park.State = Convert.ToString(reader["state"]);
                    park.Acreage = Convert.ToInt32(reader["acreage"]);
                    park.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
                    park.MilesOfTrail = Convert.ToInt32(reader["milesOfTrail"]);
                    park.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
                    park.Climate = Convert.ToString(reader["climate"]);
                    park.YearFounded = Convert.ToInt32(reader["yearFounded"]);
                    park.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
                    park.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
                    park.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                    park.ParkDescription = Convert.ToString(reader["parkDescription"]);
                    park.EntryFee = Convert.ToDouble(reader["entryFee"]);
                    park.NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
                    park.Latitude = Convert.ToDouble(reader["latitude"]);
                    park.Longitude = Convert.ToDouble(reader["longitude"]);

                    parks.Add(park);
                }
            }

            return parks;
        }
        public Dictionary<string,string> GetParksDictionary()
        {
            Dictionary<string, string> parks = new Dictionary<string, string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM park", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parks.Add(Convert.ToString(reader["parkCode"]), Convert.ToString(reader["parkName"]));
                }
            } 

            return parks;
        }
        public ParkModel GetPark(string parkCode)
        {
            ParkModel park = new ParkModel();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM park WHERE parkCode = @parkCode", conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                park.ParkCode = Convert.ToString(reader["parkCode"]);
                park.ParkName = Convert.ToString(reader["parkName"]);
                park.State = Convert.ToString(reader["state"]);
                park.Acreage = Convert.ToInt32(reader["acreage"]);
                park.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
                park.MilesOfTrail = Convert.ToInt32(reader["milesOfTrail"]);
                park.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
                park.Climate = Convert.ToString(reader["climate"]);
                park.YearFounded = Convert.ToInt32(reader["yearFounded"]);
                park.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
                park.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
                park.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
                park.ParkDescription = Convert.ToString(reader["parkDescription"]);
                park.EntryFee = Convert.ToDouble(reader["entryFee"]);
                park.NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
                park.Latitude = Convert.ToDouble(reader["latitude"]);
                park.Longitude = Convert.ToDouble(reader["longitude"]);
            }

            return park;
        }  
        public IList<WeatherModel> GetWeathers(string parkCode)
        {
            GinerateWeathers(parkCode);
            return weathersList;
        }
        
        public async Task<IList<WeatherModel>> GinerateWeathers(string parkCode)
        {
            //get latitude and longatude from database
            double latitude, longitude;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT latitude, longitude FROM park WHERE parkCode = @parkCode", conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                latitude = Convert.ToDouble(reader["latitude"]);
                longitude = Convert.ToDouble(reader["longitude"]);
            }

            //retrieve api JSON text
            Weathers weathersJSON = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.darksky.net/forecast/abceeae83c5db281569248683cd3ef49/");

                var responseTask = client.GetAsync(latitude+","+longitude+"?exclude=currently,minutely,hourly,alerts,flags");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    string content = await result.Content.ReadAsStringAsync();
                    weathersJSON = JsonConvert.DeserializeObject<Weathers>(content);
                }
            }

            IList<WeatherModel> weathers = new List<WeatherModel>();

            for(int i = 0; i < 5; i++)
            {
                WeatherModel weather = new WeatherModel();

                weather.Low = (int)weathersJSON.daily.data[i].temperatureLow;
                weather.High = (int)weathersJSON.daily.data[i].temperatureHigh;
                weather.Forecast = weathersJSON.daily.data[i].icon;
                weather.ActualDate = DateTime.Now.AddDays(i);

                if (weather.High > 75)
                {
                    weather.TempAdvisory += "Bring an extra gallon of water! ";
                }

                if (weather.Low < 20)
                {
                    weather.TempAdvisory += "Frigid temperatures can lead to frostbite! ";
                }

                if (weather.High - weather.Low > 20)
                {
                    weather.TempAdvisory += "Wear breathable layers! ";
                }

                weathers.Add(weather);
            }

            //this.weathersList = weathers;
            return weathers;
        }
    }
}
