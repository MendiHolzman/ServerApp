using ServerApp.Controller;
using System;
using System.Net;
using System.Threading.Tasks;



namespace ServerApp
{

    public class Server
    {
        private readonly HttpListener _listener;
        private readonly UserHelper _userHelper;

        public Server(string[] prefixes, UserHelper userHelper)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("HttpListener is not supported on this platform.");
            }

            _listener = new HttpListener();
            _userHelper = userHelper;

            foreach (string prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Server started.");

            while (_listener.IsListening)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                ProcessRequest(context);
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine("Server stopped.");
        }

        private async void ProcessRequest(HttpListenerContext context)
        {
            string url = context.Request.Url.AbsolutePath;
            string method = context.Request.HttpMethod;

            switch (url)
            {
                case "/adduser":
                    await _userHelper.AddUser(context);
                    break;

                case "/getuser":
                    await _userHelper.GetUser(context);
                    break;

                case "/updateuser":
                    await _userHelper.UpdateUser(context);
                    break;

                case "/deleteuser":
                    await _userHelper.DeleteUser(context);
                    break;

                case "/getusers":
                    await _userHelper.GetUsers(context);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }

            context.Response.Close();
        }
    }

}