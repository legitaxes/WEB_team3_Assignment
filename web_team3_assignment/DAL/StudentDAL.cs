﻿using System;
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
    public class StudentDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public StudentDAL()
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
        //Gets list of all students
        public List<Student> GetAllStudent()
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Student ORDER BY StudentID", conn);
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
            da.Fill(result, "StudentDetails");
            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to “Staff” objects
            List<Student> studentList = new List<Student>();
            foreach (DataRow row in result.Tables["StudentDetails"].Rows)
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
                }
                );
            }
            return studentList;
        }
        //Adds a student into database
        public int Add(StudentCreate student)
        {
            SqlCommand cmd = new SqlCommand
                ("INSERT INTO Student (Name, Course, Photo, Description, Achievement, ExternalLink, EmailAddr, Password, MentorID)" +
                " OUTPUT INSERTED.StudentID" +
                " VALUES(@name, @course, @photo, @description, @achievement, @externallink, @email, @password, @mentorid)", conn);
            cmd.Parameters.AddWithValue("@name", student.Name);
            cmd.Parameters.AddWithValue("@course", student.Course);
            if (student.Photo != null)
                cmd.Parameters.AddWithValue("@photo", student.Photo);
            else
                cmd.Parameters.AddWithValue("@photo", DBNull.Value);
            if (student.Description != null)
                cmd.Parameters.AddWithValue("@description", student.Description);
            else
                cmd.Parameters.AddWithValue("@description", DBNull.Value);
            if (student.Achievement != null)
                cmd.Parameters.AddWithValue("@achievement", student.Achievement);
            else
                cmd.Parameters.AddWithValue("@achievement", DBNull.Value);
            if (student.ExternalLink != null)
                cmd.Parameters.AddWithValue("@externallink", student.ExternalLink);
            else
                cmd.Parameters.AddWithValue("@externallink", DBNull.Value);
            cmd.Parameters.AddWithValue("@email", student.EmailAddr);
            cmd.Parameters.AddWithValue("@password", student.Password);
            cmd.Parameters.AddWithValue("@mentorid", student.MentorID);

            //open connection to run command
            conn.Open();
            student.StudentID = (int)cmd.ExecuteScalar();
            //close connection
            conn.Close();
            return student.StudentID;
        }
        //Check if the email exists in the database
        public bool IsEmailExist(string email, int id)
        {
            SqlCommand cmd = new SqlCommand
            ("SELECT StudentID FROM Student WHERE EmailAddr=@selectedEmail" + " AND StudentID = @selectedstudentID", conn);
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            cmd.Parameters.AddWithValue("@selectedstudentID", id);
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
        public StudentPhoto GetPhotoDetails(int studentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT Name, Photo FROM Student WHERE StudentID = @selectedStudentID", conn);
            cmd.Parameters.AddWithValue("@selectedStudentID", studentID);
            //object “cmd” as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            //Create a DataSet object “result"
            DataSet studentresult = new DataSet();

            //Open a database connection.
            conn.Open();

            //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet. 
            da.Fill(studentresult, "StudentDetails");

            //Close the database connection 
            conn.Close();
            StudentPhoto studentPhoto = new StudentPhoto();
            if (studentresult.Tables["StudentDetails"].Rows.Count > 0)
            {
                studentPhoto.StudentID = studentID;
                DataTable table = studentresult.Tables["StudentDetails"];

                if (!DBNull.Value.Equals(table.Rows[0]["Name"]))
                    studentPhoto.Name = table.Rows[0]["Name"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Photo"]))
                    studentPhoto.Photo = table.Rows[0]["Photo"].ToString();
                return studentPhoto;
            }
            else 
                return null;
        }
            //Get student details
        public Student GetStudentDetails(int studentID)
        {
            //Instantiate a SqlCommand object, supply it with a SELECT SQL
            //statement which retrieves all attributes of a staff record.
            SqlCommand cmd = new SqlCommand           
            ("SELECT * FROM Student WHERE StudentID = @selectedStudentID", conn);


            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedStudentID", studentID);


            //Instantiate a DataAdapter object, pass the SqlCommand
            //object “cmd” as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            //Create a DataSet object “result"
            DataSet studentresult = new DataSet();

            //Open a database connection.
            conn.Open();

            //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet. 
            da.Fill(studentresult, "StudentDetails");

            //Close the database connection 
            conn.Close();

            Student student = new Student();

            if (studentresult.Tables["StudentDetails"].Rows.Count > 0)
            {
                student.StudentID = studentID;
                // Fill staff object with values from the DataSet
                DataTable table = studentresult.Tables["StudentDetails"];

                if (!DBNull.Value.Equals(table.Rows[0]["Name"]))
                    student.Name = table.Rows[0]["Name"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Course"]))
                    student.Course = table.Rows[0]["Course"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Photo"]))
                    student.Photo = table.Rows[0]["Photo"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
                    student.Description = table.Rows[0]["Description"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Achievement"]))
                    student.Achievement = table.Rows[0]["Achievement"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["ExternalLink"]))
                    student.ExternalLink = table.Rows[0]["ExternalLink"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["EmailAddr"]))
                    student.EmailAddr = table.Rows[0]["EmailAddr"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Password"]))
                    student.Password = table.Rows[0]["Password"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["MentorID"]))
                    student.MentorID = Convert.ToInt32(table.Rows[0]["MentorID"]);

                return student; // No error occurs
            }

            else
            {
                return null; // Record not found
            }
        }

        public int UpdateProfile(Student student)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Student SET Name=@name, Course=@course, Photo=@photo, Description=@description, Achievement=@achievement, ExternalLink=@externallink, EmailAddr=@emailaddr, Password=@password, MentorID=@mentorID" +
                " WHERE StudentID=@selectedStudentID", conn);
            
            cmd.Parameters.AddWithValue("@name", student.Name);
            cmd.Parameters.AddWithValue("@course", student.Course);
            if (student.Photo != null)
                cmd.Parameters.AddWithValue("@photo", student.Photo);
            else
                cmd.Parameters.AddWithValue("@photo", DBNull.Value);
            if (student.Description != null)
                cmd.Parameters.AddWithValue("@description", student.Description);
            else
                cmd.Parameters.AddWithValue("@description", DBNull.Value);
            if (student.Achievement != null)
                cmd.Parameters.AddWithValue("@achievement", student.Achievement);
            else
                cmd.Parameters.AddWithValue("@achievement", DBNull.Value);
            if (student.ExternalLink != null)
                cmd.Parameters.AddWithValue("@externallink", student.ExternalLink);
            else
                cmd.Parameters.AddWithValue("@externallink", DBNull.Value);
            cmd.Parameters.AddWithValue("@emailaddr", student.EmailAddr);
            cmd.Parameters.AddWithValue("@password", student.Password);
            cmd.Parameters.AddWithValue("@mentorID", student.MentorID);
            cmd.Parameters.AddWithValue("@selectedStudentID", student.StudentID);
            conn.Open();

            int count = cmd.ExecuteNonQuery();

            conn.Close();

            return count;
        }

        public int UpdatePhoto(StudentPhoto student)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Student SET Name=@name, Photo=@photo" +
                " WHERE StudentID=@selectedStudentID", conn);

            cmd.Parameters.AddWithValue("@name", student.Name);
            if (student.Photo != null)
                cmd.Parameters.AddWithValue("@photo", student.Photo);
            else
                cmd.Parameters.AddWithValue("@photo", DBNull.Value);
            cmd.Parameters.AddWithValue("@selectedStudentID", student.StudentID);
            conn.Open();

            int count = cmd.ExecuteNonQuery();

            conn.Close();

            return count;
        }

        public StudentUpdatePassword GetPassword(int studentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE StudentID = @selectedstudentID", conn);
            cmd.Parameters.AddWithValue("@selectedstudentID", studentID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "StudentPassword");
            conn.Close();
            StudentUpdatePassword student = new StudentUpdatePassword();
            if (result.Tables["StudentPassword"].Rows.Count > 0)
            {
                student.StudentID = studentID;
                DataTable table = result.Tables["StudentPassword"];
                if (!DBNull.Value.Equals(table.Rows[0]["Password"]))
                    student.Password = table.Rows[0]["Password"].ToString();
                return student;
            }
            else
            {
                return null;
            }
        }

        //Returns true if password is changed successfully without errors
        public bool ChangePassword(StudentUpdatePassword student)
        {
            //Numeric validation
            //Nount the number of character in the password
            int counter = student.NewPassword.Length;
            //Use for loop to loop thru each character in the string, checks through the whole string for numbers
            for (int i = 0; i < counter; i++)
            {
                //If the current iteration contains a number, execute the query which updates the password
                if (Char.IsDigit(student.NewPassword, i))
                {
                    //hashed the new password
                    var sha1 = new SHA1CryptoServiceProvider();
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(student.NewPassword));
                    string hashedPassword = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                    SqlCommand cmd = new SqlCommand("UPDATE Student SET Password=@newPassword" +
                    " WHERE StudentID = @selectedstudentID", conn);
                    cmd.Parameters.AddWithValue("@newPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@selectedstudentID", student.StudentID);
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
    }
}
