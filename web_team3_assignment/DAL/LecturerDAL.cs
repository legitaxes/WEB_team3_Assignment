using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using web_team3_assignment.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web_team3_assignment.DAL
{
    public class LecturerDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public LecturerDAL()
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

        public int Add(Lecturer lecturer)
        {
            //sql command to add (i hope it works :pray:)
            SqlCommand cmd = new SqlCommand
                ("INSERT INTO Lecturer (Name, EmailAddr, Password, Description)" +
                " OUTPUT INSERTED.LecturerID" +
                " VALUES(@name, @email, @password, @description)", conn);
            cmd.Parameters.AddWithValue("@name", lecturer.Name);
            cmd.Parameters.AddWithValue("@email", lecturer.Email);
            cmd.Parameters.AddWithValue("@password", lecturer.Password);
            cmd.Parameters.AddWithValue("@description", lecturer.Description);
            //open connection to run command
            conn.Open();
            lecturer.LecturerId = (int)cmd.ExecuteScalar();
            //close connection
            conn.Close();
            return lecturer.LecturerId;
        }

        public bool IsEmailExist(string email)
        {
            SqlCommand cmd = new SqlCommand
            ("SELECT LecturerID FROM Lecturer WHERE EmailAddr=@selectedEmail", conn);
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            SqlDataAdapter daEmail = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            //Use DataAdapter to fetch data to a table "EmailDetails" in DataSet.
            daEmail.Fill(result, "EmailDetails");
            conn.Close();
            if (result.Tables["EmailDetails"].Rows.Count > 0)
                return true; //The email exists for another staff
            else
                return false; // The email address given does not exist
        }

        public List<Lecturer> GetAllLecturer()
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Lecturer ORDER BY LecturerID", conn);
            //Instantiate a DataAdapter object and pass the
            //SqlCommand object created as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Create a DataSet object to contain records get from database
            DataSet result = new DataSet();
            //Open a database connection
            conn.Open();
            //Use DataAdapter, which execute the SELECT SQL through its
            //SqlCommand object to fetch data to a table "StaffDetails"
            //in DataSet "result".
            da.Fill(result, "LecturerDetails");
            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to “Staff” objects
            List<Lecturer> lecturerList = new List<Lecturer>();
            foreach (DataRow row in result.Tables["LecturerDetails"].Rows)
            {
                lecturerList.Add(
                new Lecturer
                {
                    LecturerId = Convert.ToInt32(row["LecturerID"]),
                    Name = row["Name"].ToString(),
                    Email = row["EmailAddr"].ToString(),
                    Password = row["Password"].ToString(),
                    Description = row["Description"].ToString()
                }
                );
            }
            return lecturerList;
        }

        //get the details of the lecturer and return a lecturer object
        public Lecturer getLecturerDetails(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Lecturer WHERE LecturerID = @selectedLecturerID", conn);
            cmd.Parameters.AddWithValue("@selectedLecturerID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "LecturerDetails");
            conn.Close();
            Lecturer lecturer = new Lecturer();
            if (result.Tables["LecturerDetails"].Rows.Count > 0)
            {
                lecturer.LecturerId = lecturerId;
                DataTable table = result.Tables["LecturerDetails"];

                if (!DBNull.Value.Equals(table.Rows[0]["Name"]))
                    lecturer.Name = table.Rows[0]["Name"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["EmailAddr"]))
                    lecturer.Email = table.Rows[0]["EmailAddr"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Password"]))
                    lecturer.Password = table.Rows[0]["Password"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
                    lecturer.Description = table.Rows[0]["Description"].ToString();
                return lecturer;
            }
            else
            {
                return null; 
            }
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

        public List<SelectListItem> GetMentees(int lecturerId)
        {

            SqlCommand cmd = new SqlCommand("SELECT StudentID, Name FROM Student WHERE MentorID = @selectedMentorID", conn);
            cmd.Parameters.AddWithValue("@selectedMentorID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "MenteeDetails");
            conn.Close();
            List<SelectListItem> menteesList = new List<SelectListItem>();
            System.Diagnostics.Debug.WriteLine(menteesList.Count);
            foreach (DataRow row in result.Tables["MenteeDetails"].Rows)
            {
                menteesList.Add(
                    new SelectListItem
                    {
                        Value = row["StudentID"].ToString(),
                        Text = row["Name"].ToString()
                    });
            }
            return menteesList;
        }
    }

}
