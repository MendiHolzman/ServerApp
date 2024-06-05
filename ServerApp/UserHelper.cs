using ServerApp.Controller;
using ServerApp.Model;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerApp
{
    public class UserHelper
    {
        private readonly UserController _userController;

        public UserHelper()
        {
            _userController = new UserController();
        }


        public async Task AddUser(HttpListenerContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    var newUser = JsonConvert.DeserializeObject<User>(requestBody);
                    _userController.AddUser(newUser);

                    context.Response.StatusCode = (int)HttpStatusCode.Created;

                    Console.WriteLine($"New user named {newUser.Name} successfully added! " +
                                      $"by accessing the address - " +
                                      $" {context.Request.UserHostName}{context.Request.RawUrl}" +
                                      $" with status code -  {context.Response.StatusCode}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public async Task GetUser(HttpListenerContext context)
        {
            try
            {
                string userIdString = context.Request.QueryString["id"];
                if (string.IsNullOrEmpty(userIdString))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                int userId;
                if (!int.TryParse(userIdString, out userId))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                // Retrieve user from database based on userId
                User user = _userController.GetUser(userId);
                if (user == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                string responseJson = JsonConvert.SerializeObject(user);
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine($"The user with ID {userId} - retrieved successfully! " +
                                  $"by accessing the address - " +
                                  $" {context.Request.UserHostName}{context.Request.RawUrl}" +
                                  $" with status code -  {context.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public async Task UpdateUser(HttpListenerContext context)
        {
            try
            {
                string userIdString = context.Request.QueryString["id"];
                if (string.IsNullOrEmpty(userIdString))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                int userId;
                if (!int.TryParse(userIdString, out userId))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                // Retrieve user from database based on userId
                User userToUpdate = _userController.GetUser(userId);
                if (userToUpdate == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                // Deserialize request body to update user object
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    User updatedUser = JsonConvert.DeserializeObject<User>(requestBody);

                    // Update user properties
                    userToUpdate.Name = updatedUser.Name;
                    userToUpdate.Email = updatedUser.Email;
                    userToUpdate.Password = updatedUser.Password;

                    // Save changes to database
                    _userController.UpdateUser(userToUpdate);
                }

                context.Response.StatusCode = (int)HttpStatusCode.OK;

                Console.WriteLine($"The user with ID {userId} - Updated successfully! " +
                  $"by accessing the address - " +
                  $" {context.Request.UserHostName}{context.Request.RawUrl}" +
                  $" with status code -  {context.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public async Task DeleteUser(HttpListenerContext context)
        {
            try
            {
                string userIdString = context.Request.QueryString["id"];
                if (string.IsNullOrEmpty(userIdString))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                int userId;
                if (!int.TryParse(userIdString, out userId))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                await Task.Run(() =>
                {

                    // Retrieve user from database based on userId
                    User userToDelete = _userController.GetUser(userId);
                    if (userToDelete == null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }

                    // Delete user from database
                    _userController.DeleteUser(userId);

                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;

                    Console.WriteLine($"The user with ID {userId} - deleted successfully! " +
                   $"by accessing the address - " +
                   $" {context.Request.UserHostName}{context.Request.RawUrl}" +
                   $" with status code -  {context.Response.StatusCode}");

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }


        }

        public async Task GetUsers(HttpListenerContext context)
        {
            try
            {
                var users = _userController.GetUsers();
                string responseJson = JsonConvert.SerializeObject(users);
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine($"The users were successfully retrieved to" +
                    $" {context.Request.UserHostName}{context.Request.RawUrl}" +
                    $" with status code -  {context.Response.StatusCode}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

    }
}