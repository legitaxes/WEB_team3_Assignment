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

        public int Add(Student student)
        {
            //sql command to add (i hope it works :pray:)
            SqlCommand cmd = new SqlCommand
                ("INSERT INTO Student (Name, Course, Photo, Description, Achievement, ExternalLink, EmailAddr, Password, MentorID)" +
                " OUTPUT INSERTED.StudentID" +
                " VALUES(@name, @course, @photo, @description, @achievement, @externallink, @email, @password, @mentorid)", conn);
            cmd.Parameters.AddWithValue("@name", student.Name);
            cmd.Parameters.AddWithValue("@course", student.Course);
            cmd.Parameters.AddWithValue("@photo", student.Photo);
            cmd.Parameters.AddWithValue("@description", student.Description);
            cmd.Parameters.AddWithValue("@achievement", student.Achievement);
            cmd.Parameters.AddWithValue("@externallink", student.ExternalLink);
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
    }
}
