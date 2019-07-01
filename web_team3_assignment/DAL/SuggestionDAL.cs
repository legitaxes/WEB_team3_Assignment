using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_team3_assignment.Models;

namespace web_team3_assignment.DAL
{
    public class SuggestionDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public SuggestionDAL()
        {
            //Locate the appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            //Read ConnectionString from appsettings.json file
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "Student_EportfolioConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public int PostSuggestion(Suggestion suggestion)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Suggestion (LecturerID, StudentID, Description, Status) " +
            "OUTPUT INSERTED.SuggestionID " +
            "VALUES(@lecturerid, @student, @desc, @status)", conn);
            cmd.Parameters.AddWithValue("@lecturerid", suggestion.LecturerId);
            cmd.Parameters.AddWithValue("@student", suggestion.StudentId);
            cmd.Parameters.AddWithValue("@desc", suggestion.Description);
            cmd.Parameters.AddWithValue("@status", suggestion.Status);
            conn.Open();
            suggestion.SuggestionId = (int)cmd.ExecuteScalar();
            conn.Close();
            return suggestion.SuggestionId;
        }

    }
}
