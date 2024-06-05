using MySql.Data.MySqlClient;
using ServerApp.Model;
using System.Collections.Generic;
using System.Data;



namespace ServerApp.Controller
{
    public interface IUserController
    {
        void AddUser(User user);
        User GetUser(int userId);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        List<User> GetUsers();
    }


    public class UserController : IUserController
    {
        const string constring = @"SERVER=localhost;DATABASE=mydatabase;UID=root;PASSWORD=root;";

        public UserController()
        {
        }

        public void AddUser(User user)
        {
            using (MySqlConnection sqlCon = new MySqlConnection(constring))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("AddUser", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                user.ID = 0;

                sqlCmd.Parameters.AddWithValue("_id", user.ID);
                sqlCmd.Parameters.AddWithValue("_name", user.Name);
                sqlCmd.Parameters.AddWithValue("_email", user.Email);
                sqlCmd.Parameters.AddWithValue("_password", user.Password);

                sqlCmd.ExecuteNonQuery();
            }
        }

        public User GetUser(int userId)
        {
            var user = new User();

            using (MySqlConnection sqlCon = new MySqlConnection(constring))
            {
                sqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("GetUser", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDa.SelectCommand.Parameters.AddWithValue("_id", userId);

                DataTable dt = new DataTable();
                sqlDa.Fill(dt);

                var row = dt.Rows[0];

                user.ID = (int)row.ItemArray[0];
                user.Name = row.ItemArray[1].ToString();
                user.Email = row.ItemArray[2].ToString();
                user.Password = row.ItemArray[3].ToString();
            }

            return user;
        }

        public void UpdateUser(User user)
        {
            using (MySqlConnection sqlCon = new MySqlConnection(constring))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("UpdateUser", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("_id", (int)user.ID);
                sqlCmd.Parameters.AddWithValue("_name", user.Name);
                sqlCmd.Parameters.AddWithValue("_email", user.Email);
                sqlCmd.Parameters.AddWithValue("_password", user.Password);

                sqlCmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (MySqlConnection sqlCon = new MySqlConnection(constring))
            {
                sqlCon.Open();
                MySqlCommand sqlCmd = new MySqlCommand("DeleteUser", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("_id", userId);

                sqlCmd.ExecuteNonQuery();
            }
        }

        public List<User> GetUsers()
        {
            var res = new List<User>();

            using (MySqlConnection sqlCon = new MySqlConnection(constring))
            {
                sqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("GetUsers", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                sqlDa.Fill(dt);

                foreach (DataRow dataRow in dt.Rows)
                {
                    var user = new User();

                    user.ID = (int)dataRow.ItemArray[0];
                    user.Name = dataRow.ItemArray[1].ToString();
                    user.Email = dataRow.ItemArray[2].ToString();
                    user.Password = dataRow.ItemArray[3].ToString();

                    res.Add(user);
                }
            }

            return res;
        }


    }
}
