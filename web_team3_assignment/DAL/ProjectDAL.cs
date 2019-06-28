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
    public class ProjectDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor

        public ProjectDAL()
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

        public int Add(Project project)
        {
            SqlCommand cmd = new SqlCommand
            ("INSERT INTO Project (ProjectId, title, description)" + "OUTPUT INSERTED.ProjectID" + "VALUES(@projectId, @title, @description)", conn);
            cmd.Parameters.AddWithValue("@projectId", project.ProjectId);
            cmd.Parameters.AddWithValue("@title", project.Title);
            cmd.Parameters.AddWithValue("@description", project.Description);
            //open connection to run command
            conn.Open();
            project.ProjectId = (int)cmd.ExecuteScalar();
            //close connection
            conn.Close();
            return project.ProjectId;        
        }

        public List<Project> GetAllProject()
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Project ORDER BY ProjectID", conn);

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
            da.Fill(result, "ProjectDetails");

            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to “Staff” objects
            List<Project> projectList = new List<Project>();
            foreach (DataRow row in result.Tables["ProjectDetails"].Rows)
            {
                projectList.Add(
                new Project
                {
                    ProjectId = Convert.ToInt32(row["ProjectId"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString()
                }
                );
            }
            return projectList;
        }

    }
}
