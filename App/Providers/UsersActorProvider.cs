using Akka.Actor;
using App.Actors;
using App.Services;

namespace App.Providers
{
    public class UsersActorProvider
    {
        public IActorRef RequestDisplayActorInstance { get; }
        public IActorRef UserListActorInstance { get; }

        public UsersActorProvider(ActorSystem actorSystem, IUserService userService)
        {
            RequestDisplayActorInstance = actorSystem.ActorOf(Props.Create(() => new RequestDisplayActor(userService)),
                "requests-display");
            UserListActorInstance = actorSystem.ActorOf(Props.Create(() => new UsersListActor()),
                "users-list");

        }
    }
}
