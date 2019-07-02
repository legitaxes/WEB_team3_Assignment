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
                    ProjectId = Convert.ToInt32(row["ProjectID"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString(),
                    ProjectPoster = row["ProjectPoster"].ToString(),
                    ProjectURL = row["ProjectURL"].ToString()
                }
                );
            }
            return projectList;
        }



        public int Add(Project project)
        {
            SqlCommand cmd = new SqlCommand
            ("INSERT INTO Project (ProjectID, Title, Description, ProjectPoster, ProjectURL)" + 
            "OUTPUT INSERTED.ProjectID" +
            "VALUES(@projectID, @title, @description, @projectPoster, @projectURL)", conn);


            cmd.Parameters.AddWithValue("@projectID", project.ProjectId);
            cmd.Parameters.AddWithValue("@title", project.Title);
            cmd.Parameters.AddWithValue("@description", project.Description);
            cmd.Parameters.AddWithValue("@projectPoster", project.ProjectPoster);
            cmd.Parameters.AddWithValue("@projectURL", project.ProjectURL);

            //open connection to run command
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //ExecuteScalar RETURNs a single value
            //StaffID after executing the INSERT SQL statement
            project.ProjectId = (int)cmd.ExecuteScalar();

            //close connection
            conn.Close();


            //Return id when no error occurs.
            return project.ProjectId;        
        }

     

        public bool IsProjectExists(string ProjectId)
        {
            SqlCommand cmd = new SqlCommand
                ("SELECT ProjectID FROM Project WHERE ProjectID=@selectedProjectID", conn);

            cmd.Parameters.AddWithValue("@selectedProjectID", ProjectId);

            SqlDataAdapter daProjectId = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();

            conn.Open();

            //Use DataAdapter to fetch data to a table "EmailDetails" in DataSet. 
            daProjectId.Fill(result, "ProjectIdDetails");
            conn.Close();

            if (result.Tables["ProjectIdDetails"].Rows.Count > 0)
                return true; //The email exists for another staff

            else
                return false; // The email address given does not exist 

        }


        //Get project details
        public Project GetProjectDetails(int projectId)
        {

            //Instantiate a SqlCommand object, supply it with a SELECT SQL
            //statement which retrieves all attributes of a staff record.
            SqlCommand cmd = new SqlCommand
            ("SELECT * FROM Project WHERE ProjectID = @selectedProjectID", conn);


            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedProjectID", projectId);


            //Instantiate a DataAdapter object, pass the SqlCommand
            //object “cmd” as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            //Create a DataSet object “result"
            DataSet projectresult = new DataSet();

            //Open a database connection.
            conn.Open();

            //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet. 
            da.Fill(projectresult, "ProjectDetails");

            //Close the database connection 
            conn.Close();

            Project project = new Project();

            if (projectresult.Tables["ProjectDetails"].Rows.Count > 0)
            {
                project.ProjectId = projectId;

                // Fill staff object with values from the DataSet
                DataTable table = projectresult.Tables["ProjectDetails"];

                if (!DBNull.Value.Equals(table.Rows[0]["Title"]))
                    project.Title = table.Rows[0]["Title"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
                    project.Description = table.Rows[0]["Description"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["ProjectPoster"]))
                    project.Description = table.Rows[0]["ProjectPoster"].ToString();

                return project; // No error occurs
            }

            else
            {
                return null; // Record not found
            }
        }

      

        //// Return number of row updated
        //public int Update(Project project)
        //{
        //    //Instantiate a SqlCommand object, supply it with SQL statement UPDATE
        //    //and the connection object for connecting to the database.
        //    SqlCommand cmd = new SqlCommand
        //    ("UPDATE Project SET ProjectID=@projectId, Title=@title, Description=@description" +
        //    " WHERE ProjectID = @selectedProjectID", conn);

        //    //Define the parameters used in SQL statement, value for each parameter
        //    //is retrieved from the respective property of “staff” object.
        //    cmd.Parameters.AddWithValue("@projectId", project.ProjectId);
        //    cmd.Parameters.AddWithValue("@title", project.Title);
        //    cmd.Parameters.AddWithValue("@description", project.Description);

        //    if (project.ProjectId != null) // A title is assigned
        //        cmd.Parameters.AddWithValue("@ProjectID", project.ProjectId);

        //    else // No branch is assigned
        //        cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);

        //    cmd.Parameters.AddWithValue("@selectedProjectID", project.ProjectId);

        //    //Open a database connection.
        //    conn.Open();

        //    //ExecuteNonQuery is used for UPDATE and DELETE
        //    int count = cmd.ExecuteNonQuery();

        //    //Close the database connection.
        //    conn.Close();

        //    return count;
        //}


        //DELETE Project
        //public int Delete(int projectId)
        //{
        //    //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
        //    //to delete a staff record specified by a Staff ID.
        //    SqlCommand cmd = new SqlCommand("DELETE FROM Project " +
        //    "WHERE ProjectId = @selectProjectId", conn);
        //    cmd.Parameters.AddWithValue("@selectProjectId", projectId);

        //    //Open a database connection.
        //    conn.Open();
        //    int rowCount;

        //    //Execute the DELETE SQL to remove the staff record.
        //    rowCount = cmd.ExecuteNonQuery();

        //    //Close database connection.
        //    conn.Close();

        //    //Return number of row of staff record deleted.
        //    return rowCount;
        //}

    }
}
