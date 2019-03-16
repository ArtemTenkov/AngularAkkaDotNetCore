using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using App.Actors;
using App.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UsersActorProvider _usersActorProvider;

        public TestController(UsersActorProvider usersActorProvider) => _usersActorProvider = usersActorProvider;

        [HttpGet("Ping")]
        public async Task<IActionResult> Ping(string message)
        {
            var actor = _usersActorProvider.RequestDisplayActorInstance;
            actor.Tell(message);
            return Ok();
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetAll()
        {
            var actor = _usersActorProvider.UserListActorInstance;
            var users = await actor.Ask<List<string>>(new UsersListActor.ShowUsers());
            return Ok(users);
        }
    }
}