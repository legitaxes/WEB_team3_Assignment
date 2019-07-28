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
    public class ProjectMemberDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public ProjectMemberDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "Student_EportfolioConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }


        public List<ProjectMember> GetAllProjectMembers()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM ProjectMember", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet result = new DataSet();

            conn.Open();

            //Use DataAdapter “da” to fill up a DataTable "ProjectMemberDetails" in DataSet "result".
            da.Fill(result, "ProjectMemberDetails");

            conn.Close();

            //Transferring rows of data in DataTable to “ProjectMember” objects
            List<ProjectMember> projectMemberList = new List<ProjectMember>();
            foreach (DataRow row in result.Tables["ProjectMemberDetails"].Rows)
            {
                projectMemberList.Add(
                    new ProjectMember
                    {
                        ProjectId = Convert.ToInt32(row["ProjectID"]),
                        StudentId = Convert.ToInt32(row["StudentID"]),
                        Role = row["Role"].ToString()
                    }
                    );
            }
            return projectMemberList;
        }


        public List<Project> GetProjectPM(int projectId)
        {
            //Instantiate a SqlCommand object, supply it with a SELECT SQL
            //statement which retrieves all attributes of a projectmember record.
            SqlCommand cmd = new SqlCommand
            ("SELECT * FROM Project WHERE ProjectId = @selectedProjectMember", conn);

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the projectID property of the ProjectMember class.
            cmd.Parameters.AddWithValue("@selectedProjectMember", projectId);

            //Instantiate a DataAdapter object, pass the SqlCommand
            //object created as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            // Create a DataSet object result
            DataSet result = new DataSet();

            conn.Open();

            //Use DataAdapter to fetch data to a table "ProjectMembersDetails" in DataSet.
            da.Fill(result, "ProjectMemberDetails");

            conn.Close();

            List<Project> projectList = new List<Project>();
            foreach (DataRow row in result.Tables["ProjectMemberDetails"].Rows)
            {
                projectList.Add(
                new Project
                {
                    ProjectId = Convert.ToInt32(row["ProjectID"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString(),
                    ProjectPoster = row["ProjectPoster"].ToString(),
                    ProjectURL = row["ProjectURL"].ToString(),
                }
                );
            }
    
            return projectList;
        }


        public List<ProjectMember> GetAllProjectMember()
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM ProjectMember ORDER BY ProjectID", conn);

            //Instantiate a DataAdapter object and pass the
            //SqlCommand object created as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            //Create a DataSet object to contain records get from database
            DataSet result = new DataSet();

            //Open a database connection
            conn.Open();


            da.Fill(result, "ProjectMemberDetails");

            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to "Project" objects
            List<ProjectMember> projectMemberList = new List<ProjectMember>();
            foreach (DataRow row in result.Tables["ProjectMemberDetails"].Rows)
            {
                projectMemberList.Add(
                new ProjectMember
                {

                    ProjectId = Convert.ToInt32(row["ProjectID"]),
                    StudentId = Convert.ToInt32(row["StudentId"]),
                    Role = row["Role"].ToString(),
                }
                );
            }
            return projectMemberList;
        } 
    }
}
