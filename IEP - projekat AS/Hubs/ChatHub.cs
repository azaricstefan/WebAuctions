using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace IEP___projekat_AS.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            /// Any connection or hub wire up and configuration should go here
            Clients.All.addNewMessageToPage(name,message); //pozovi addNewMessage
        }
    }
}