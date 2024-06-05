using ServerApp;
using ServerApp.Controller;
using ServerApp.Model;
using ServerApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControllerAndServer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region Check Controller

            // Create instance of the Controller.
            UserController userCon = new UserController();


            // Add user.

            //userCon.AddUser(new User { Name = "Mendi", Email = "hh@gmail.com", Password = "1234" });
            //userCon.AddUser(new User { Name = "Shalom", Email = "sd@gmail.com", Password = "1111" });



            // Get user by Id.

            //var user = userCon.GetUser(6);
            //Console.WriteLine(user.ToString());



            // Update user details.

            //userCon.UpdateUser(new User { ID = 8, Name = "Bar", Email = "blabla@gmail.com", Password = "111" });
            //userCon.UpdateUser(new User { ID = 9, Name = "fff", Email = "blablabla@gmail.com", Password = "2222" });



            // Delete user by Id.

            //userCon.DeleteUser(6);



            // Get list of users.

            //var users = userCon.GetUsers();
            //foreach (var user in users)
            //{
            //    Console.WriteLine(user.ToString());
            //}

            #endregion


            #region Check Server

            //UserHelper userHelper = new UserHelper(); 
            //Server server = new Server(new string[] { "http://localhost:8080/" }, userHelper);

            //try
            //{
            //    server.StartAsync().Wait(); // Start the server and wait for it to finish
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}

            #endregion

        }
    }
}
