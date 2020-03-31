using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;
        public ReservationSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public decimal GetTotalCost(decimal campgroundFee, IList<DateTime> departArriveDates)
        {
            int daysDiff = (departArriveDates[1].Subtract(departArriveDates[0]).Days);
            decimal totalCost = campgroundFee * daysDiff;

            return totalCost;
        }

        public void MakeReservation(Site siteToReserve, IList<DateTime> departArriveDates)
        {
            try
            {
                Console.WriteLine("Enter a name for the reservation: ");
                string reservationName = Console.ReadLine();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO reservation (site_id, name, from_date, to_date, create_date)
                                                    VALUES (@siteID, @name, @fromDate, @toDate, GETDATE())", conn);


                    cmd.Parameters.AddWithValue("@siteID", siteToReserve.SiteID);
                    cmd.Parameters.AddWithValue("@name",reservationName);
                    cmd.Parameters.AddWithValue("@fromDate", departArriveDates[0].Date);
                    cmd.Parameters.AddWithValue("@toDate", departArriveDates[1].Date);
                    SqlDataReader reader = cmd.ExecuteReader();

                  int reservationID = GetReservationID();

                    Console.WriteLine($"Your confirmation number is {reservationID}.");
                    Console.ReadLine();
                    

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetReservationID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                IList<Reservation> reservations = new List<Reservation>();
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT MAX(reservation_id) as reservation_id FROM reservation", conn);

                int reservationID = 0;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    

                    reservationID = Convert.ToInt32(reader["reservation_id"]);

                }

                return reservationID;

                    


            }
        }
    }
}
