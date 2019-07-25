using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using web_team3_assignment.Models;

namespace web_team3_assignment.DAL
{
    public class HomeDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public HomeDAL()
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

        //lecturerlogin query
        public Lecturer lecturerLogin(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Lecturer", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "loginDetails");
            conn.Close();
            Lecturer lecturer = new Lecturer();
            foreach (DataRow row in result.Tables["loginDetails"].Rows)
            {
                lecturer.Email = row["EmailAddr"].ToString().ToLower();
                lecturer.Password = row["Password"].ToString();
                if (lecturer.Email == email && lecturer.Password == password)
                {
                    lecturer.LecturerId = Convert.ToInt32(row["LecturerID"]);
                    if (!DBNull.Value.Equals(row["Name"]))
                        lecturer.Name = row["Name"].ToString();
                    break;
                }
            }
            return lecturer;
        }

        public Student studentLogin(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "studentLoginDetails");
            conn.Close();
            Student student = new Student();
            foreach (DataRow row in result.Tables["studentLoginDetails"].Rows)
            {
                student.EmailAddr = row["EmailAddr"].ToString().ToLower();
                student.Password = row["Password"].ToString();
                if (student.EmailAddr == email && student.Password == password)
                {
                    student.StudentID = Convert.ToInt32(row["StudentID"]);
                    if (!DBNull.Value.Equals(row["Name"]))
                        student.Name = row["Name"].ToString();
                    break;
                }
            }
            return student;
        }

        //Get projectID by studentID in integer
        public ProjectMember getProjectID(int studentId)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM ProjectMember WHERE StudentID = @selectedstudentId", conn);
            cmd.Parameters.AddWithValue("@selectedstudentId", studentId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "projectDetails");
            conn.Close();
            ProjectMember projectmember = new ProjectMember();
            foreach (DataRow row in result.Tables["projectDetails"].Rows)
            {
                projectmember.ProjectId = Convert.ToInt32(row["ProjectID"]);
                projectmember.Role = row["Role"].ToString();
                projectmember.StudentId = Convert.ToInt32(row["StudentID"]);
            }
            return projectmember;
        }
    }
}
