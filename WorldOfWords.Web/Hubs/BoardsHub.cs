using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WorldOfWords.Web.Hubs
{
    public class BoardsHub : Hub
    {
        public void JoinBoard(string name)
        {
            this.Groups.Add(this.Context.ConnectionId, name);
        }

        public void WordAdded(string content, string boardName)
        {
            //zapisvame v bazata
            this.Clients.Group(boardName).updateBoard(content);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Context.ConnectionId // Trqbwa da iztriem tozi chovek ot wsichki grupi
        }
    }
}