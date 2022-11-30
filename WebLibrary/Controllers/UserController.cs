using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class UserController : ApiController
    {
        LibraryEntities2 libraryEntities = new LibraryEntities2();
        [HttpGet]
        public IEnumerable<User> GetAllUser()
        {
            var users = libraryEntities.User.ToList();
            return users;
        }
        public IHttpActionResult GetUser(int id)
        {
            var user = libraryEntities.User.FirstOrDefault((p) => p.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        [HttpPost]
        public IHttpActionResult AddUser(User NewUser)
        {
            var reponse = libraryEntities.User.Where(q => q.UserName == NewUser.UserName).Any();

            if (!reponse)
            {

                libraryEntities.User.Add(NewUser);

                libraryEntities.SaveChanges();
            }
            else
            {
                return BadRequest("UserName already exist");
            }
            return Ok(NewUser);
        }

        [HttpDelete]

        public IHttpActionResult DeleteUser(int id)
        {
            var user = libraryEntities.User.FirstOrDefault((p) => p.Id == id);
            if (user != null)
            {
                libraryEntities.User.Remove(user);
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
