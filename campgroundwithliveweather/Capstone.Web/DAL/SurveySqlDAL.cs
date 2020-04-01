using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web.DAL
{
    public class SurveySqlDAL : ISurveyDAL
    {
        private readonly string connectionString;

        public SurveySqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void SaveSurvey(SurveyModel newSurvey)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT into survey_result(parkCode, emailAddress, state, activityLevel) values(@parkCode, @emailAddress, @state, @activityLevel)", conn);
                cmd.Parameters.AddWithValue("@parkCode", newSurvey.ParkName);
                cmd.Parameters.AddWithValue("@emailAddress", newSurvey.Email);
                cmd.Parameters.AddWithValue("@state", newSurvey.StateOfResidence);
                cmd.Parameters.AddWithValue("@activityLevel", newSurvey.ActivityLevel);

                var reader = cmd.ExecuteNonQuery();
            }
        }

        public IList<SurveyResultsModel> GetResults()
        {
            IList<SurveyResultsModel> results = new List<SurveyResultsModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT park.parkName, park.parkCode, COUNT(survey_result.parkCode) AS surveys_submitted FROM park
                                                JOIN survey_result ON survey_result.parkCode = park.parkCode
                                                GROUP BY park.parkCode, park.parkName
                                                ORDER BY COUNT(survey_result.parkCode) DESC, park.parkName ASC", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SurveyResultsModel result = new SurveyResultsModel();

                    result.ParkCode = Convert.ToString(reader["parkCode"]);
                    result.ParkName = Convert.ToString(reader["parkName"]);
                    result.SurveysSubmitted = Convert.ToInt32(reader["surveys_submitted"]);

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
