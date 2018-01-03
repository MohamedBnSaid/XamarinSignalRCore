using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets.Client;
using MobileClient.Services;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileClient
{
    /// <summary>
    /// SignalR Client
    /// </summary>
    /// 
    [PreserveAttribute(AllMembers = true)]
    public class SignalRClient
    {
        private HubConnection _hub;
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public HubConnection Hub { get { return _hub; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRClient"/> class.
        /// </summary>
        public SignalRClient()
        {
            Debug.WriteLine("SignalR Initialized...");
            InitializeSignalR().ContinueWith(task =>
            {
                Debug.WriteLine("SignalR Started...");
            });
        }

        /// <summary>
        /// Initializes SignalR.
        /// </summary>
        public async Task InitializeSignalR()
        {
            try
            {
                _hub = new HubConnectionBuilder()
                .WithUrl("http://196.203.192.101/SignalRCoreApi/updater")
                .WithJsonProtocol()
                .Build();

                _hub.On<string, double>("NewUpdate",
                   (command, state) => ValueChanged?
                   .Invoke(this, new ValueChangedEventArgs(command, state)));

                _hub.Connected += _hub_Connected;
                _hub.Closed += _hub_Closed;

                await _hub.StartAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(String.Format("Exception : {0}",ex));
                await App.Current.MainPage.DisplayAlert("Exception", ex.ToString(), "Ok");
            }
            
        }

        private Task _hub_Closed(Exception arg)
        {
            Console.WriteLine("Hub is Closed");
            return null;
        }

        private Task _hub_Connected()
        {
            Console.WriteLine("Hub is Connected");         
            return null;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="state">The state.</param>
        public async Task SendMessage(string command, double state)
        {
            await _hub?.InvokeAsync("NewUpdate", new object[] { command, state });
        }
    }

}
