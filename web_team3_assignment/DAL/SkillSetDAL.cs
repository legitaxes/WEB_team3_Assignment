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
    public class SkillSetDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        public SkillSetDAL()
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

        public int Add(SkillSet skillSet)
        {
            //sql command to add 
            SqlCommand cmd = new SqlCommand
                ("INSERT INTO SkillSet (SkillSetName)" +
                "OUTPUT INSERTED.SkillSetID " +
                "VALUES(@skillsetname)", conn);
            cmd.Parameters.AddWithValue("@skillsetname", skillSet.SkillSetName);        
            //open connection to run command
            conn.Open();
            skillSet.SkillSetId = (int)cmd.ExecuteScalar();
            //close connection
            conn.Close();
            return skillSet.SkillSetId;
        }

        public List<SkillSet> GetAllSkillSet()
        {
            //Instantiate a SqlCommand object, supply it with a
            //SELECT SQL statement that operates against the database,
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Skillset ORDER BY SkillsetID", conn);
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
            da.Fill(result, "SkillsetDetails");
            //Close the database connection
            conn.Close();

            //Transferring rows of data in DataSet’s table to “Staff” objects
            List<SkillSet> SkillSetList = new List<SkillSet>();
            foreach (DataRow row in result.Tables["SkillsetDetails"].Rows)
            {
                SkillSetList.Add(
                new SkillSet
                {
                    SkillSetId = Convert.ToInt32(row["SkillsetID"]),
                    SkillSetName = row["SkillSetName"].ToString()
                }
                );
            }
            return SkillSetList;
        }

        // Return number of row updated
        public int Update(SkillSet skillSet)
        {
            //Instantiate a SqlCommand object, supply it with SQL statement UPDATE
            //and the connection object for connecting to the database.
            SqlCommand cmd = new SqlCommand
            ("UPDATE SkillSet SET SkillsetName=@skillsetName WHERE SkillsetID = @selectedSkillsetID", conn);

            cmd.Parameters.AddWithValue("@skillsetName", skillSet.SkillSetName);
            cmd.Parameters.AddWithValue("@selectedSkillsetID", skillSet.SkillSetId);

            conn.Open();

            int count = cmd.ExecuteNonQuery();

            conn.Close();
            return count;
        }

        //deletes record from database
        public int Delete(int SkillSetId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM SkillSet " +
                "WHERE SkillSetID = @selectedSkillSetID", conn);
            cmd.Parameters.AddWithValue("@selectedSkillSetID", SkillSetId);
            conn.Open();
            int rowCount;
            rowCount = cmd.ExecuteNonQuery();
            conn.Close();
            return rowCount;
        }

        public SkillSet GetDetails(int SkillSetId)
        {
            //Instantiate a SqlCommand object, supply it with a SELECT SQL
            //statement which retrieves all attributes of a staff record.
            SqlCommand cmd = new SqlCommand
            ("SELECT * FROM SKillset WHERE SKillsetID = @selectedSkillsetID", conn);
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedSkillsetID", SkillSetId);
            //Instantiate a DataAdapter object, pass the SqlCommand
            //object “cmd” as parameter.
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Create a DataSet object “result"
            DataSet result = new DataSet();
            //Open a database connection.
            conn.Open();
            //Use DataAdapter to fetch data to a table "StaffDetails" in DataSet.
            da.Fill(result, "SkillsetDetails");
            //Close the database connection
            conn.Close();
            SkillSet skillset = new SkillSet();
            if (result.Tables["SkillsetDetails"].Rows.Count > 0)
            {
                skillset.SkillSetId = SkillSetId;
                // Fill staff object with values from the DataSet
                DataTable table = result.Tables["SkillsetDetails"];
                if (!DBNull.Value.Equals(table.Rows[0]["SkillSetName"]))
                    skillset.SkillSetName = table.Rows[0]["SkillSetName"].ToString();
               
                return skillset; // No error occurs
            }
            else
            {
                return null; // Record not found
            }
        }

        public bool IsSkillSetExist(string skillset)
        {
            SqlCommand cmd = new SqlCommand
            ("SELECT SkillSetID FROM SKillSet WHERE SkillSetName=@selectedSkillSetName", conn);
            cmd.Parameters.AddWithValue("@selectedSkillSetName", skillset);
            SqlDataAdapter daSkillSet = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            //Use DataAdapter to fetch data to a table "SkillSetDetails" in DataSet.
            daSkillSet.Fill(result, "SkillSetDetails");
            conn.Close();
            if (result.Tables["SkillSetDetails"].Rows.Count > 0)
                return true; //The email exists for another staff
            else
                return false; // The email address given does not exist
        }
    }
}
