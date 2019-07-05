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
                    }
                    );
            }
            return projectMemberList;
        }


        //public List<Project> GetProjectPM(int projectId)
        //{
        //    //Instantiate a SqlCommand object, supply it with a SELECT SQL
        //    //statement which retrieves all attributes of a branch record.
        //    SqlCommand cmd = new SqlCommand
        //    ("SELECT * FROM Project WHERE ProjectId = @selectedProjectId", conn);

        //    //Define the parameter used in SQL statement, value for the
        //    //parameter is retrieved from the branchNo property of the Branch class.
        //    cmd.Parameters.AddWithValue("@selectedProjectId", projectId);

        //    //Instantiate a DataAdapter object, pass the SqlCommand
        //    //object created as parameter.
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);

        //    // Create a DataSet object result
        //    DataSet result = new DataSet();

        //    //Open a database connection.
        //    conn.Open();

        //    //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet.
        //    da.Fill(result, "ProjectDetails");

        //    //Close database connection
        //    conn.Close();

        //    List<Project> projectList = new List<Project>();
        //    foreach (DataRow row in result.Tables["ProjectDetails"].Rows)
        //    {
        //        projectList.Add(
        //        new Project
        //        {
        //            ProjectId = Convert.ToInt32(row["ProjectID"]),
        //            Title = row["Title"].ToString(),
        //            Description = row["Description"].ToString(),
        //            ProjectPoster = row["ProjectPoster"].ToString(),
        //            ProjectURL = row["ProjectURL"].ToString(),
        //        }
        //        );
        //    }

        //    return projectList;
        //}




        //public int DetailsProject(ProjectMember projectMember)
        //{
        //    SqlCommand cmd = new SqlCommand("INSERT INTO ProjectMember (ProjectID, StudentID, Role) " +
        //    "OUTPUT INSERTED.ProjectID " +
        //    "VALUES(@projectID, @studentID, @role)", conn);

        //    cmd.Parameters.AddWithValue("@projectID", projectMember.ProjectId);
        //    cmd.Parameters.AddWithValue("@studentID", projectMember.StudentId);
        //    cmd.Parameters.AddWithValue("@description", projectMember.Role);


        //    conn.Open();
        //    projectMember.ProjectId = (int)cmd.ExecuteScalar();
        //    conn.Close();
        //    return projectMember.ProjectId;
        //}


        //public ProjectMember GetProjectMemberDetails(int projectId)
        //{
        //    //Instantiate a SqlCommand object, supply it with a SELECT SQL
        //    //statement which retrieves all attributes of a staff record.
        //    SqlCommand cmd = new SqlCommand
        //    ("SELECT * FROM ProjectMember WHERE ProjectID = @selectedProjectID", conn);

        //    //Define the parameter used in SQL statement, value for the
        //    //parameter is retrieved from the method parameter “staffId”.
        //    cmd.Parameters.AddWithValue("@selectedProjectID", projectId);

        //    //Instantiate a DataAdapter object, pass the SqlCommand
        //    //object “cmd” as parameter.
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);

        //    //Create a DataSet object “result"
        //    DataSet result = new DataSet();

        //    //Open a database connection.
        //    conn.Open();

        //    //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet. 
        //    da.Fill(result, "ProjectMemberDetails");

        //    //Close the database connection 
        //    conn.Close();

        //    ProjectMember projectMember = new ProjectMember();
        //    if (result.Tables["ProjectMemberDetails"].Rows.Count > 0)
        //    {
        //        projectMember.ProjectId = projectId;

        //        // Fill staff object with values from the DataSet
        //        DataTable table = result.Tables["ProjectMemberDetails"];

        //        if (!DBNull.Value.Equals(table.Rows[0]["ProjectID"]))
        //            projectMember.ProjectId = Convert.ToInt32(table.Rows[0]["ProjectID"]);

        //        if (!DBNull.Value.Equals(table.Rows[0]["StudentID"]))
        //            projectMember.StudentId = Convert.ToInt32(table.Rows[0]["StudentID"]);

        //        if (!DBNull.Value.Equals(table.Rows[0]["Role"]))
        //            projectMember.Role = Convert.ToString(table.Rows[0]["Role"]);

        //        return projectMember; // No error occurs
        //    }
        //    else
        //    {
        //        return null; // Record not found
        //    }
        //}





        //public int Update(ProjectMember projectMember)
        //{
        //    SqlCommand cmd = new SqlCommand("UPDATE ProjectMember SET Role=@role WHERE ProjectID = @selectedProjectID", conn);

        //    cmd.Parameters.AddWithValue("@role", projectMember.Role);

        //    conn.Open();

        //    int count = cmd.ExecuteNonQuery();

        //    conn.Close();

        //    return count;
        //}

        //public int Delete(int projectId)
        //{
        //    SqlCommand cmd = new SqlCommand("DELETE FROM ProjectMember " +
        //    "WHERE ProjectID = @selectProjectID", conn);

        //    cmd.Parameters.AddWithValue("@selectProjectID", projectId);
        //    conn.Open();
        //    int rowCount;
        //    rowCount = cmd.ExecuteNonQuery();
        //    conn.Close();
        //    return rowCount;
        //}

        ////Add ProjectMember
        //public int Add(ProjectMember projectMember)
        //{
        //    SqlCommand cmd = new SqlCommand
        //    ("INSERT INTO ProjectMember (StudentID, Role) " +
        //    "OUTPUT INSERTED.ProjectID " +
        //    "VALUES(@studentID, @role)", conn);

        //    cmd.Parameters.AddWithValue("@studentID", projectMember.StudentId);
        //    cmd.Parameters.AddWithValue("@role", projectMember.Role);

        //    //open connection to run command
        //    conn.Open();

        //    //ExecuteScalar is used to retrieve the auto-generated
        //    //ExecuteScalar RETURNs a single value
        //    //StaffID after executing the INSERT SQL statement
        //    projectMember.ProjectId = (int)cmd.ExecuteScalar();

        //    //close connection
        //    conn.Close();


        //    //Return id when no error occurs.
        //    return projectMember.ProjectId;
        //}




    }
}
