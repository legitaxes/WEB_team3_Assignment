﻿using System;
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
    }

}