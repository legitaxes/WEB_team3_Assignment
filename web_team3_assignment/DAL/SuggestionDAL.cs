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

        //public List<Suggestion> GetSuggestionDetails(int lecturerid)
        //{
        //    SqlCommand cmd = new SqlCommand
        //    ("SELECT * FROM Suggestion WHERE LecturerID = @selectedLecturerID", conn);
        //    cmd.Parameters.AddWithValue("@selectedLecturerID", lecturerid);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet result = new DataSet();
        //    conn.Open();
        //    da.Fill(result, "SuggestionDetails");
        //    conn.Close();
        //    Suggestion suggestion = new Suggestion();
        //    if (result.Tables["SuggestionDetails"].Rows.Count > 0)
        //    {
        //        suggestion.LecturerId = lecturerid;
        //        DataTable table = result.Tables["SuggestionDetails"];

        //        if (!DBNull.Value.Equals(table.Rows[0]["SuggestionID"]))
        //            suggestion.SuggestionId = Convert.ToInt32(table.Rows[0]["SuggestionID"]);

        //        if (!DBNull.Value.Equals(table.Rows[0]["StudentID"]))
        //            suggestion.StudentId = Convert.ToInt32(table.Rows[0]["StudentID"]);

        //        if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
        //            suggestion.Description = table.Rows[0]["Description"].ToString();

        //        if (!DBNull.Value.Equals(table.Rows[0]["Status"]))
        //            suggestion.Status = Convert.ToChar(table.Rows[0]["Status"]);

        //        return suggestion;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public List<Suggestion> GetAllSuggestionDetails(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Suggestion" +
                " WHERE LecturerID = @selectedLecturerID", conn);
            cmd.Parameters.AddWithValue("@selectedLecturerID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "SuggestDetails");
            conn.Close();
            List<Suggestion> suggestList = new List<Suggestion>();
            foreach (DataRow row in result.Tables["SuggestDetails"].Rows)
            {
                suggestList.Add(
                    new Suggestion
                    {
                        SuggestionId = Convert.ToInt32(row["SuggestionID"]),
                        LecturerId = Convert.ToInt32(row["LecturerID"]),
                        StudentId = Convert.ToInt32(row["StudentID"]),
                        Description = row["Description"].ToString(),
                        Status = Convert.ToChar(row["Status"]),
                        DateCreated = Convert.ToDateTime(row["DateCreated"])
                    }
                    );
            }
            return suggestList; 
        }
    }
}
