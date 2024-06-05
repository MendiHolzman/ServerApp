using System;
using System.Collections.Generic;
using ServerApp.Controller;
using ServerApp.Model;
using System.Net;
using System.Timers;
using System.Net.Mail;

namespace ServerApp.Service
{
    public interface IUserBatchService
    {
        void ScheduleEmailsEveryTwentySeconds();
        void ScheduleWeeklyEmails();
        void SendingAweeklyNewsletterToUsers();
        string SendEmail(User sender, User user);
    }

    public class UserBatchService : IUserBatchService
    {
        private UserController _userController;
        private User _sender;
        private Timer _timer;

        public UserBatchService(UserController userController, User sender)
        {
            _userController = userController;
            _sender = sender;

            // Initialize the timer for Every Twenty Seconds
            _timer = new Timer();
            _timer.Elapsed += TimerElapsed;
            ScheduleEmailsEveryTwentySeconds();

            // Initialize the timer for Schedule Weekly
            //_timer = new Timer();
            //_timer.Elapsed += TimerElapsed1;
            //ScheduleWeeklyEmails();
        }

        // Timer elapsed event handler for Every Twenty Seconds
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            SendingAweeklyNewsletterToUsers();
        }

        // Timer elapsed event handler for Schedule Weekly
        private void TimerElapsed1(object sender, ElapsedEventArgs e)
        {
            // Stop the timer temporarily to prevent multiple executions
            _timer.Stop();

            // Send the weekly newsletter
            SendingAweeklyNewsletterToUsers();

            // Schedule the next round of weekly emails
            ScheduleWeeklyEmails();

            // Restart the timer
            _timer.Start();
        }

        // Schedule emails to be sent every twenty seconds
        public void ScheduleEmailsEveryTwentySeconds()
        {
            // Set the timer interval to 20,000 milliseconds (20 seconds)
            _timer.Interval = 20000;
            _timer.Start();
        }

        // Schedule weekly emails to be sent every Sunday at 8:00 PM
        public void ScheduleWeeklyEmails()
        {
            // Calculate the time until next Sunday 8:00 PM
            DateTime now = DateTime.Now;
            DateTime nextSunday = now.AddDays(((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7).Date.AddHours(20);
            TimeSpan timeUntilSunday = nextSunday - now;

            // Set the timer interval to the time until next Sunday
            _timer.Interval = timeUntilSunday.TotalMilliseconds;
            _timer.Start();
        }

        public void SendingAweeklyNewsletterToUsers()
        {

            if (_sender == null)
            {
                Console.WriteLine("Error: There is no standard sender for the email.");
            }

            if (_sender.Email == string.Empty)
            {
                Console.WriteLine("Error: There is no standard email for a sender.");
            }

            var users = _userController.GetUsers();

            int i = -1;
            foreach (var user in users)
            {
                i++;
                if (user == null)
                {
                    Console.WriteLine("--------------------------------------\n");
                    Console.WriteLine("Error: No receiver found for the email in index - " + i);
                    Console.WriteLine("\n--------------------------------------\n\n");
                    continue;
                }

                if (user.Email != string.Empty)
                {
                    Console.WriteLine("--------------------------------------\n");
                    Console.WriteLine(SendEmail(_sender, user));
                    Console.WriteLine("\n--------------------------------------\n\n");
                }
                else
                {
                    Console.WriteLine("--------------------------------------\n");
                    Console.WriteLine("There is no standard email for the recipient in index - " + i);
                    Console.WriteLine("\n--------------------------------------\n\n");
                }
            }
        }

        public string SendEmail(User sender, User user)
        {
            string res = string.Empty;

            using (var smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(sender.Email, sender.Password),
                EnableSsl = true,
            })
            {
                try
                {
                    smtpClient.Send(sender.Email, user.Email, "Your newspaper! ( " + DateTime.Now.Date.ToString(), " ) The content of the newspaper");

                    res = "The email to the user: " + user.Name + ", at the address: " + user.Email + " has been sent successfully!";
                }
                catch (SmtpException ex)
                {
                    res = "Sending the email to user " + user.Name + " failed!\n";
                    res += " - Error Message: " + ex.Message + "\n";

                    // SmtpException might not always have an InnerException
                    if (ex.InnerException != null)
                    {
                        res += " - InnerException: " + ex.InnerException.Message;
                    }

                    // Handle the exception gracefully, log it, or take appropriate action
                }
                catch (Exception ex)
                {
                    // Catch any other exceptions
                    res = "An unexpected error occurred while sending the email: " + ex.Message;
                }
            }

            return res;
        }
    }
}
