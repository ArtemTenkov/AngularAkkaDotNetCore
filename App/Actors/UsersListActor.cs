using System.Collections.Generic;
using Akka.Actor;

namespace App.Actors
{
    public class UsersListActor : ReceiveActor
    {
        public UsersListActor()
        {
            ReceiveAsync<ShowUsers>(async showUsers => Sender.Tell(new List<string> { "CEO", "CTO" }));   
        }

        public class ShowUsers
        {

        }
    }
}
