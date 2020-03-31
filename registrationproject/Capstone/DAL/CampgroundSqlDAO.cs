using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;
        public CampgroundSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }
        public IList<Campground> ListCampgroundsFromPark(int parkID)
        {
            List<Campground> campgrounds = new List<Campground>();


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //Using SqlCommand to execute SQL query to get a list of campgrounds matching parkid
                    SqlCommand cmd = new SqlCommand("SELECT * FROM campground WHERE park_id = @parkID", conn);
                    cmd.Parameters.AddWithValue("@parkID", parkID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground campground = new Campground();


                        campground.ParkID = Convert.ToInt32(reader["park_id"]);
                        campground.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenDate = Convert.ToInt32(reader["open_from_mm"]);
                        campground.CloseDate = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                        campgrounds.Add(campground);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return campgrounds;




        }

        public string GetDateAsString(int dateNum)
        {
            string returnString = "";

            switch (dateNum)
            {
                case 1:
                    returnString = "January";
                    break;
                case 2:
                    returnString = "February";
                    break;
                case 3:
                    returnString = "March";
                    break;
                case 4:
                    returnString = "April";
                    break;
                case 5:
                    returnString = "May";
                    break;
                case 6:
                    returnString = "June";
                    break;
                case 7:
                    returnString = "July";
                    break;
                case 8:
                    returnString = "August";
                    break;
                case 9:
                    returnString = "September";
                    break;
                case 10:
                    returnString = "October";
                    break;
                case 11:
                    returnString = "November";
                    break;
                case 12:
                    returnString = "December";
                    break;

            }



            return returnString;
        }
    }
}
