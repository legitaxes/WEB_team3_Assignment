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

        public Suggestion GetSuggestionDetails(int suggestionId)
        {
            SqlCommand cmd = new SqlCommand
            ("SELECT * FROM Suggestion WHERE SuggestionID = @selectedSuggestionID", conn);
            cmd.Parameters.AddWithValue("@selectedSuggestionID", suggestionId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "CurrentSuggestionDetails");
            conn.Close();
            Suggestion suggestion = new Suggestion();
            if (result.Tables["CurrentSuggestionDetails"].Rows.Count > 0)
            {
                suggestion.SuggestionId = suggestionId;
                DataTable table = result.Tables["CurrentSuggestionDetails"];

                //if (!DBNull.Value.Equals(table.Rows[0]["SuggestionID"]))
                //    suggestion.SuggestionId = Convert.ToInt32(table.Rows[0]["SuggestionID"]);

                if (!DBNull.Value.Equals(table.Rows[0]["LecturerID"]))
                    suggestion.LecturerId = Convert.ToInt32(table.Rows[0]["LecturerID"]);

                if (!DBNull.Value.Equals(table.Rows[0]["StudentID"]))
                    suggestion.StudentId = Convert.ToInt32(table.Rows[0]["StudentID"]);

                if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
                    suggestion.Description = table.Rows[0]["Description"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Status"]))
                    suggestion.Status = Convert.ToChar(table.Rows[0]["Status"]);

                if (!DBNull.Value.Equals(table.Rows[0]["DateCreated"]))
                    suggestion.DateCreated = Convert.ToDateTime(table.Rows[0]["DateCreated"]);

                return suggestion;
            }
            else
            {
                return null;
            }
        }

        public List<Suggestion> GetSuggestionPostedByMentor(int lecturerId)
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

        public List<Suggestion> GetSuggestionPostedByStudentsMentor(int studentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Suggestion" +
                " WHERE StudentID = @selectedstudentID" + " AND Status = 'N'", conn);
            cmd.Parameters.AddWithValue("@selectedstudentID", studentID);
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

        public int Acknowledge(int suggestionId)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Suggestion " +
            " SET Status = 'Y'" + " WHERE SuggestionID = @selectsuggestionID", conn);

            cmd.Parameters.AddWithValue("@selectSuggestionID", suggestionId);
            conn.Open();
            int rowCount;
            rowCount = cmd.ExecuteNonQuery();
            conn.Close();
            return rowCount;
        }

        public int Update(Suggestion suggestion)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Suggestion SET Description=@description, Status=@status" +
                " WHERE SuggestionID = @selectedSuggestionID", conn);
            cmd.Parameters.AddWithValue("@description", suggestion.Description);
            cmd.Parameters.AddWithValue("@status", Convert.ToChar(suggestion.Status));
            cmd.Parameters.AddWithValue("@selectedSuggestionID", suggestion.SuggestionId);

            conn.Open();

            int count = cmd.ExecuteNonQuery();

            conn.Close();

            return count;
        }

        public int Delete(int suggestionId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Suggestion " +
            "WHERE SuggestionID = @selectSuggestionID", conn);

            cmd.Parameters.AddWithValue("@selectSuggestionID", suggestionId);
            conn.Open();
            int rowCount;
            rowCount = cmd.ExecuteNonQuery();
            conn.Close();
            return rowCount;
        }
    }
}
