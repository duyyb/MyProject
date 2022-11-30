using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class TypeOfBookController : ApiController
    {
        LibraryEntities2 libraryEntities = new LibraryEntities2();
        public IEnumerable<TypeOfBook> GetAllTypeOfBook()
        {
            var typeofbooks = libraryEntities.TypeOfBook.ToList();
            return typeofbooks;
        }
        public IHttpActionResult GetTypeOfBook(int id)
        {
            var typeofbook = libraryEntities.TypeOfBook.FirstOrDefault((p) => p.Id == id);
            if (typeofbook == null)
            {
                return NotFound();
            }
            return Ok(typeofbook);
        }
        
        [HttpPost]

        public IHttpActionResult AddToP(TypeOfBook NewToP)
        {
            var reponse = libraryEntities.TypeOfBook.Where(p => p.Category == NewToP.Category).Any();
            if (!reponse)
            {
                libraryEntities.TypeOfBook.Add(NewToP);
                libraryEntities.SaveChanges();
            }
            else
            {
                return BadRequest("TypeOfBook allready exist");
            }
            return Ok(NewToP);
        }
        [HttpDelete]

        public IHttpActionResult DeleteToP(int id)
        {
            var ToP = libraryEntities.TypeOfBook.FirstOrDefault((p) => p.Id == id);
            if (ToP != null)
            {
                libraryEntities.TypeOfBook.Remove(ToP);
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
       
    }
}
