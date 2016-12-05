using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace YearUpRestApi.Models
{ 
    public class SQLCommunication
    {
        private const string _databaseString = "Server=tcp:yearup.database.windows.net,1433;Database=YearUpData;User ID=yearupadmin; Password=admin123.;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

        private SqlConnection _connection;

        public SQLCommunication()
        {
            using (_connection = new SqlConnection(_databaseString))
            {
                _connection.Open();
            }
        }

        public string GetUserByEmail(string emailAddress, string password)
        {
            var cmd = @"SELECT * FROM Users WHERE email = " + emailAddress;

            SqlCommand isNew = new SqlCommand(cmd, _connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(isNew);
            DataTable data = new DataTable();
            dataAdapter.Fill(data);

            if (data.Rows.Count == 0)
            {
                int newUserId = AddUser(0, "", emailAddress, password);
                cmd = @"SELECT * FROM Users WHERE id = " + newUserId;
                SqlCommand getNew = new SqlCommand(cmd, _connection);
                data = new DataTable();
                dataAdapter.Fill(data);
            }

            string json = GetJson(data);
            return json;
        }

        public string GetUserById(int userId)
        {
            var cmd = @"SELECT * FROM Users WHERE id = " + userId;
            SqlCommand getUser = new SqlCommand(cmd, _connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(getUser);
            DataTable data = new DataTable();
            dataAdapter.Fill(data);

            string json = GetJson(data);
            return json;
        }

        //return user id
        public int AddUser(int admin, string name, string email, string password)
        {
            SqlCommand addUserCmd = new SqlCommand();

            addUserCmd.CommandText = @"INSERT INTO Users (admin, name, email, password) VALUES (@val1, @val2, @val3, @val4)";

            addUserCmd.Parameters.AddWithValue("@val1", admin);
            addUserCmd.Parameters.AddWithValue("@val2", name);
            addUserCmd.Parameters.AddWithValue("@val3", email);
            addUserCmd.Parameters.AddWithValue("@val4", password);

            int id = (int)addUserCmd.ExecuteScalar();

            return id;
        }

        public string GetMatches()
        {
            var cmd = @"SELECT * FROM Matches";
            SqlCommand getUser = new SqlCommand(cmd, _connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(getUser);
            DataTable data = new DataTable();
            dataAdapter.Fill(data);

            string json = GetJson(data);
            return json;
        }

        public bool AddStudent(int userId, int skype, string location, int english, int math)
        {
            SqlCommand addStudentCmd = new SqlCommand();

            addStudentCmd.CommandText = @"INSERT INTO Student (id, skype, location, english, math) VALUES (@val1, @val2, @val3, @val4, @val5)";

            addStudentCmd.Parameters.AddWithValue("@val1", userId);
            addStudentCmd.Parameters.AddWithValue("@val2", skype);
            addStudentCmd.Parameters.AddWithValue("@val3", location);
            addStudentCmd.Parameters.AddWithValue("@val4", english);
            addStudentCmd.Parameters.AddWithValue("@val5", math);

            int id = (int)addStudentCmd.ExecuteScalar();

            return true;
        }

        public bool AddTeacher(int userId, int skype, string location, int english, int math)
        {
            SqlCommand addTeacherCmd = new SqlCommand();

            addTeacherCmd.CommandText = @"INSERT INTO Teacher (id, skype, location, english, math) VALUES (@val1, @val2, @val3, @val4, @val5)";

            addTeacherCmd.Parameters.AddWithValue("@val1", userId);
            addTeacherCmd.Parameters.AddWithValue("@val2", skype);
            addTeacherCmd.Parameters.AddWithValue("@val3", location);
            addTeacherCmd.Parameters.AddWithValue("@val4", english);
            addTeacherCmd.Parameters.AddWithValue("@val5", math);

            int id = (int)addTeacherCmd.ExecuteScalar();

            return true;
        }

        public bool AddAvailability(Dictionary<int, int> timeSlots)
        {
            SqlCommand addAvailabilityCmd = new SqlCommand();
            var command = "INSERT INTO Availability (userid, timeslots) VALUES ";

            foreach (var entry in timeSlots)
            {
                command += "(" + entry.Key + ", " + entry.Value + "), ";
            }

            command = command.Substring(0, command.Length - 1);

            addAvailabilityCmd.CommandText = command;

            int id = (int)addAvailabilityCmd.ExecuteScalar();

            return true;
        }

        private string GetJson(DataTable data)
        {
            var jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
            return jsonString;
        }

    }
}