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
using System.Security.Cryptography;
using System.Text;

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
            if (lecturer.Description!= null)
                cmd.Parameters.AddWithValue("@description", lecturer.Description);
            else
                cmd.Parameters.AddWithValue("@description", DBNull.Value);
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
        public LecturerPassword getPasswordDetails(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Lecturer WHERE LecturerID = @selectedLecturerID", conn);
            cmd.Parameters.AddWithValue("@selectedLecturerID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "LecturerPasswordDetails");
            conn.Close();
            LecturerPassword lecturer = new LecturerPassword();
            if (result.Tables["LecturerPasswordDetails"].Rows.Count > 0)
            {
                lecturer.LecturerId = lecturerId;
                DataTable table = result.Tables["LecturerPasswordDetails"];
                if (!DBNull.Value.Equals(table.Rows[0]["Password"]))
                    lecturer.Password = table.Rows[0]["Password"].ToString();
                return lecturer;
            }
            else
            {
                return null;
            }
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
        //returns true if password is changed successfully without errors
        public bool ChangePassword(LecturerPassword lecturer)
        {
            //numeric validation
            //count the number of character in the password
            int counter = lecturer.NewPassword.Length;
            //use for loop to loop thru each character in the string, checks through the whole string for numbers
            for (int i = 0; i < counter; i++)
            {
                if (Char.IsDigit(lecturer.NewPassword, i))
                {
                    //hashed the new password
                    var sha1 = new SHA1CryptoServiceProvider();
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(lecturer.NewPassword));
                    string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                    SqlCommand cmd = new SqlCommand("UPDATE Lecturer SET Password=@newPassword" +
                    " WHERE LecturerID = @selectedLecturerID", conn);
                    cmd.Parameters.AddWithValue("@newPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@selectedLecturerID", lecturer.LecturerId);
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }

        //this method checks whether there are students are under the currently logged in lecturer
        public bool CheckIsUsed(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("Select Name FROM Student" +
                " WHERE MentorID = @selectedlecturerID", conn);
            cmd.Parameters.AddWithValue("@selectedlecturerID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "CheckMentorID");
            conn.Close();
            List<Student> studentCount = new List<Student>();
            foreach (DataRow row in result.Tables["CheckMentorID"].Rows)
            {
                studentCount.Add(
                    new Student
                    {
                        Name = row["Name"].ToString(),
                    });
            }
            //if there are students under the lecturer, return true
            if (studentCount.Count > 0)
            {
                //ViewData["StudentNames"] = 
                return true;
            }
            //if not return false
            else
            {
                return false;
            }
        }

        //deletes record from database
        public int Delete(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Lecturer " +
                "WHERE LecturerID = @selectLecturerID", conn);
            cmd.Parameters.AddWithValue("@selectLecturerID", lecturerId);
            conn.Open();
            int rowCount;
            rowCount = cmd.ExecuteNonQuery();
            conn.Close();
            return rowCount;
        }

        //get mentees profile in a student list
        public List<Student> GetMenteeDetails(int lecturerId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student" +
                " WHERE MentorID = @selectedMentorID" +
                " ORDER BY StudentID ASC", conn);
            cmd.Parameters.AddWithValue("@selectedMentorID", lecturerId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AllMentees");
            conn.Close();
            List<Student> studentList = new List<Student>();
            foreach (DataRow row in result.Tables["AllMentees"].Rows)
            {
                studentList.Add(
                    new Student
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        Name = row["Name"].ToString(),
                        Course = row["Course"].ToString(),
                        Photo = row["Photo"].ToString(),
                        Description = row["Description"].ToString(),
                        Achievement = row["Achievement"].ToString(),
                        ExternalLink = row["ExternalLink"].ToString(),
                        EmailAddr = row["EmailAddr"].ToString(),
                        Password = row["Password"].ToString(),
                        MentorID = Convert.ToInt32(row["MentorID"])
                    });
            }
            return studentList;
        }

        //get a list of mentees under the lecturer in a selectListItem List
        public List<SelectListItem> GetMentees(int lecturerId)
        {

            SqlCommand cmd = new SqlCommand("SELECT StudentID, Name FROM Student " +
                "WHERE MentorID = @selectedMentorID " +
                "ORDER BY Name ASC", conn);
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
        public int Update(Lecturer lecturer)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Lecturer SET Name=@name, EmailAddr=@email, Description=@desc" +
                "WHERE LecturerID = @selectedLecturerID", conn);
            cmd.Parameters.AddWithValue("@name", lecturer.Name);
            cmd.Parameters.AddWithValue("@email", lecturer.Email);

            if (lecturer.Description != null)
                cmd.Parameters.AddWithValue("@desc", lecturer.Description);
            else
                cmd.Parameters.AddWithValue("@desc", DBNull.Value);

            cmd.Parameters.AddWithValue("@selectedLecturerID", lecturer.LecturerId);
            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;

        }
    }

}
