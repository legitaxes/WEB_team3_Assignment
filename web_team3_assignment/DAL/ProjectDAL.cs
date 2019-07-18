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


        //studentId can either have interger value or null value
        public List<Project> GetAllProject(int? studentId)
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            //Join project table and projectmember table where studentID is the selected studentID
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Project p" +
            " INNER JOIN ProjectMember pm ON p.ProjectID = pm.ProjectID" +
            " WHERE StudentID = @selectedstudentID" , conn);

            cmd.Parameters.AddWithValue("@selectedstudentID", studentId);

            //Instantiate a DataAdapter object and pass the
            //SqlCommand object created as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            //Create a DataSet object to contain records get from database
            DataSet result = new DataSet();

            //Open a database connection
            conn.Open();

            //Use DataAdapter, which execute the SELECT SQL through its
            //SqlCommand object to fetch data to a table "ProjectDetails"
            //in DataSet "result".
            da.Fill(result, "ProjectDetails");

            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to “Project” objects
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


        //Add Project
        public int Add(Project project)
        {
            SqlCommand cmd = new SqlCommand
            ("INSERT INTO Project (Title, Description, ProjectPoster, ProjectURL) " +
            "OUTPUT INSERTED.ProjectID " +
            "VALUES(@title, @description, @projectPoster, @projectURL)", conn);
            cmd.Parameters.AddWithValue("@title", project.Title);
            cmd.Parameters.AddWithValue("@description", project.Description);
            cmd.Parameters.AddWithValue("@projectPoster", project.ProjectPoster);
            cmd.Parameters.AddWithValue("@projectURL", project.ProjectURL);

            //open connection to run command
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated primary key value
            //ExecuteScalar get and RETURNs a single value (integer) from SQL in the first column of the first row, 
            //ProjectID after executing the INSERT SQL statement
            project.ProjectId = (int)cmd.ExecuteScalar();
                
            //close connection
            conn.Close();


            //Return id when no error occurs.
            return project.ProjectId;
        }
        public int AddProjectAsLeader(ProjectMember projectMember)
        {
            SqlCommand cmd = new SqlCommand
            ("INSERT INTO ProjectMember (ProjectID, StudentID, Role) " +
            "VALUES(@pid, @sid, @role)", conn);
            cmd.Parameters.AddWithValue("@pid", projectMember.ProjectId);
            cmd.Parameters.AddWithValue("@sid", projectMember.StudentId);
            cmd.Parameters.AddWithValue("@role", projectMember.Role);
            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;
        }

        //this returns a list of projectmember which is to be used in the view to show the colors for leader and member
        public List<ProjectMember> GetProjectMemberDetails(int? studentId)
        {
            SqlCommand cmd = new SqlCommand("Select * from projectmember where StudentID = @selectedStudentID", conn);
            cmd.Parameters.AddWithValue("@selectedStudentID", studentId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "ProjectMemberDetails");
            conn.Close();
            List<ProjectMember> projectMemberList = new List<ProjectMember>();
            foreach (DataRow row in result.Tables["ProjectMemberDetails"].Rows)
            {
                projectMemberList.Add(
                    new ProjectMember
                    {
                        ProjectId = Convert.ToInt32(row["ProjectID"]),
                        StudentId = Convert.ToInt32(row["StudentID"]),
                        Role = row["Role"].ToString()
                    });
            }
            return projectMemberList;
        }

        //Get project details
        public Project GetProjectDetails(int projectId)
        {

            //Instantiate a SqlCommand object, supply it with a SELECT SQL
            //statement which retrieves all attributes of a project record.
            SqlCommand cmd = new SqlCommand
            ("SELECT * FROM Project WHERE ProjectID = @selectedProjectID", conn);


            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “projectId”.
            cmd.Parameters.AddWithValue("@selectedProjectID", projectId);


            //Instantiate a DataAdapter object, pass the SqlCommand
            //object “cmd” as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            //Create a DataSet object “projectresult"
            DataSet projectresult = new DataSet();

            //Open a database connection.
            conn.Open();

            //Use DataAdapter to fetch data to a table "ProjectDetails" in DataSet. 
            da.Fill(projectresult, "ProjectDetails");

            //Close the database connection 
            conn.Close();

            //Create a new variable called project to store datas
            Project project = new Project();


            //If "ProjectDetails" in projectresult table rows is more than 0
            if (projectresult.Tables["ProjectDetails"].Rows.Count > 0)
            {
                project.ProjectId = projectId;

                // Fill project object with values from the DataSet
                DataTable table = projectresult.Tables["ProjectDetails"];

                if (!DBNull.Value.Equals(table.Rows[0]["Title"]))
                    project.Title = table.Rows[0]["Title"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["Description"]))
                    project.Description = table.Rows[0]["Description"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["ProjectPoster"]))
                    project.ProjectPoster = table.Rows[0]["ProjectPoster"].ToString();

                if (!DBNull.Value.Equals(table.Rows[0]["ProjectURL"]))
                    project.ProjectURL = table.Rows[0]["ProjectURL"].ToString();

                return project; // No error occurs
            }

            else
            {
                return null; // Record not found
            }
        }



        // Return number of row updated
        public int Update(Project project)
        {
            //Instantiate a SqlCommand object, supply it with SQL statement UPDATE
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand
            ("UPDATE Project SET Title=@Title, Description=@Description, ProjectURL=@ProjectURL, ProjectPoster=@ProjectPoster WHERE ProjectID = @selectedProjectID ", conn);

            //Assign values to parameters
            cmd.Parameters.AddWithValue("@selectedProjectID", project.ProjectId);
            cmd.Parameters.AddWithValue("@Title", project.Title);
            cmd.Parameters.AddWithValue("@Description", project.Description);
            cmd.Parameters.AddWithValue("@ProjectURL", project.ProjectURL);
            cmd.Parameters.AddWithValue("@ProjectPoster", project.ProjectPoster);

            //Open a database connection.
            conn.Open();

            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            //Close the database connection.
            conn.Close();

            return count;
        }


        //DELETE Project
        public int Delete(int projectId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a project record specified by a ProjectID.
            SqlCommand cmd = new SqlCommand("DELETE FROM Project WHERE ProjectId = @selectProjectId", conn);
            cmd.Parameters.AddWithValue("@selectProjectId", projectId);

            //Open a database connection.
            conn.Open();
            int rowCount;

            //Execute the DELETE SQL to remove the project record.
            rowCount = cmd.ExecuteNonQuery();

            //Close database connection.
            conn.Close();

            //Return number of row of project record deleted.
            return rowCount;
        }

        //Check if project Title exists
        public bool IsProjectTitleExists(string Title)
        {
            SqlCommand cmd = new SqlCommand
                ("SELECT ProjectID FROM Project WHERE Title=@selectedTitle", conn);

            cmd.Parameters.AddWithValue("@selectedTitle", Title);

            SqlDataAdapter daTitle = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();

            conn.Open();

            //Use DataAdapter to fetch data to a table "ProjectTitleDetails" in DataSet. 
            daTitle.Fill(result, "ProjectTitleDetails");

            //Close connection
            conn.Close();

            if (result.Tables["ProjectTitleDetails"].Rows.Count > 0)
                return true; //Return true is the title exists for another project

            else
                return false; //Return false if the project given does not exist 

        }

        public bool UpdateTitle(string Title)
        {
            SqlCommand cmd = new SqlCommand
                ("SELECT ProjectID FROM Project WHERE Title=@selectedTitle", conn);

            cmd.Parameters.AddWithValue("@selectedTitle", Title);

            SqlDataAdapter daTitle = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();

            conn.Open();

            //Use DataAdapter to fetch data to a table "ProjectTitleDetails" in DataSet. 
            daTitle.Fill(result, "ProjectTitleDetails");
            conn.Close();

            if (result.Tables["ProjectTitleDetails"].Rows.Count > 0)
                return true; //The title exists for another project

            else
                return false; // The project given does not exist 

        }

    }
}
