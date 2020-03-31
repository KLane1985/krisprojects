using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    class SiteSqlDAO : ISiteDAO
    {

        private string connectionString;
        public SiteSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }
        public IList<Site> ListSitesAtCampground(int campgroundID)
        {
            IList<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site WHERE campground_id = @campgroundID", conn);
                    cmd.Parameters.AddWithValue("@campgroundID", campgroundID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.SiteID = Convert.ToInt32(reader["site_id"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        sites.Add(site);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return sites;
        }


        public IList<Site> ListSitesAtCampgroundWithinDate(int campgroundID, IList<DateTime> departArriveDates)
        {
            IList<Site> sites = new List<Site>();


            //get month to see if park is open
            int fromDateMonth = departArriveDates[0].Month;
            int toDateMonth = departArriveDates[1].Month;
            DateTime fromDate = departArriveDates[0].Date;
            DateTime toDate = departArriveDates[1].Date;

            //if already a reservation, don't show that site

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT TOP 5 site.campground_id, site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities, daily_fee FROM site
                                                    JOIN campground ON campground.campground_id = site.campground_id
                                                    JOIN reservation ON reservation.site_id = site.site_id
                                                    WHERE campground.campground_id = @campgroundID AND (open_from_mm <= @fromDateMonth AND open_to_mm >= @toDateMonth) 
                                                    EXCEPT 
                                                    SELECT site.campground_id, site.site_id, site_number, max_occupancy, accessible, max_rv_length, utilities, daily_fee FROM site 
                                                    JOIN campground ON campground.campground_id = site.campground_id
                                                    JOIN reservation ON reservation.site_id = site.site_id
                                                    WHERE (reservation.from_date <= @toDate AND reservation.to_date >= @fromDate) 
                                                    GROUP BY site.campground_id, site.site_id, site.site_number, site.max_occupancy, accessible, max_rv_length, utilities, daily_fee", conn);
                        //GROUP BY site_id, site.site_number, site.max_occupancy, accessible, max_rv_length, utilities, daily_fee
                                                    
                    cmd.Parameters.AddWithValue("@campgroundID", campgroundID);
                    cmd.Parameters.AddWithValue("@fromDateMonth", fromDateMonth);
                    cmd.Parameters.AddWithValue("@toDateMonth", toDateMonth);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);

                    //if campground is closed in certain months and the user wants to start reservation in open months, camp through closed month and end in open months, it should not be valid
                    


                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.SiteID = Convert.ToInt32(reader["site_id"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        sites.Add(site);
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return sites;
        }




    }
}
