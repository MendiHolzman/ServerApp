using ServerApp.Controller;
using ServerApp.Model;
using ServerApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TestBatchService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create instance of the Controller.
            UserController userCon = new UserController();

            //Change to real email and password for results. 
            const string EmailSender = "??????" + "@outlook.co.il";
            const string PasswordSerder = "??????";

            // Creating the user who sends the email. 
            var serder = new User { ID = -1, Name = "Sender", Email = EmailSender, Password = PasswordSerder };

            // Send Newspaper to users without timing.
            //userBatchSer.SendingAweeklyNewsletterToUsers();


            // Create instance of batch service and Send Newspaper According to the timing you chose in the service.
            UserBatchService userBatchSer = new UserBatchService(userCon, serder);

            Console.WriteLine("Service started. Press any key to exit...");
            Console.ReadKey();

        }
    }
}
