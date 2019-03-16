using System.Diagnostics;
using System.Threading.Tasks;
using Akka.Actor;
using App.Services;

namespace App.Actors
{
    public class RequestDisplayActor : ReceiveActor
    {

        public RequestDisplayActor(IUserService userService)
        {
            ReceiveAsync<string>(async message => await ProcessMessage(message));
        }

        private async Task<bool> ProcessMessage(string message)
        {
            Debug.WriteLine($"Actor name: {Self.Path.Name}");
            Debug.WriteLine($"Actor system: {Context.System.Name}");
            Debug.WriteLine($"Received {message}");
            return true;
        }
    }
}
