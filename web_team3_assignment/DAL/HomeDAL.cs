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

        public bool lecturerLogin(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Lecturer ORDER BY LecturerID", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "loginDetails");
            conn.Close();
            foreach (DataRow row in result.Tables["loginDetails"].Rows)
            {
                string emailaddress = row["EmailAddr"].ToString().ToLower();
                string pass = row["Password"].ToString();
                
                if (emailaddress == email && pass == password)
                {
                    return true;
                }
            }
            return false;
        }
        public bool studentLogin(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student ORDER BY StudentID", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "studentLoginDetails");
            conn.Close();
            foreach (DataRow row in result.Tables["studentLoginDetails"].Rows)
            {
                string emailaddress = row["EmailAddr"].ToString().ToLower();
                string pass = row["Password"].ToString();

                if (emailaddress == email && pass == password)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
