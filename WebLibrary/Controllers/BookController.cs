using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLibrary.Models;
using WebLibrary.Extention;

namespace WebLibrary.Controllers
{
    public class BookController : ApiController
    {
        LibraryEntities2 libraryEntities = new LibraryEntities2();
        [HttpGet]
        public IHttpActionResult GetAllBook(int pageNumber, int pageSize)
        {
            var BInfor = (from b in libraryEntities.Book
                         join top in libraryEntities.TypeOfBook on b.IdTypeOfBook equals top.Id into T
                         from top in T.DefaultIfEmpty()
                         select new { top.Category,topID = top.Id , b.BookCode, b.Id })
                         .OrderByDescending(x => x.Category)
                         .GetPagingList(pageNumber, pageSize);

            return Ok(BInfor);
        }
        public IHttpActionResult GetBookById(int id)
        {
            var book = libraryEntities.Book.FirstOrDefault((p) => p.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        //public IHttpActionResult GetBookByName(string name)
        //{
        //    var bookName = from b in libraryEntities.Book
        //                     join c in libraryEntities.TypeOfBook on b.IdTypeOfBook equals c.Id into T
        //                     from c in T.DefaultIfEmpty()
        //                     select new { c.TypeOfBook1 };
        //    var getBookByName = TypeOfBook.Where(x => x.TypeOfBook1.StartsWith(name)).ToList();
        //        return Ok(getBookByName);
        //}
        [HttpPost]

        public IHttpActionResult AddBook(Book NewBook)
        {
            var reponse = libraryEntities.Book.Where(p => p.BookCode == NewBook.BookCode).Any();
            if (!reponse)
            {

                libraryEntities.Book.Add(NewBook);
                libraryEntities.SaveChanges();
            }
            else
            {
                return BadRequest("This Book allready exist");
            }
            return Ok(NewBook);
        }
        [HttpDelete]

        public IHttpActionResult DeleteBook(int id)
        {
            var book = (from x in libraryEntities.Book
                        where x.Id == id
                        select x).FirstOrDefault();
            libraryEntities.Book.Remove(book);
            libraryEntities.SaveChanges();
            return Ok(book);
        }
        [HttpGet]
        public IHttpActionResult ListToP()
        {
            var ToP = (from p in libraryEntities.TypeOfBook
                       select new { p.Category, p.Id });
            return Ok(ToP);
        }
        [HttpPost]
        public IHttpActionResult UpdateBook(Book book)
        {
            var UBook = (from p in libraryEntities.Book where p.Id == book.Id select p).FirstOrDefault();
            var CheckBookCode = (from p in libraryEntities.Book where p.BookCode == book.BookCode select p).Any();
            if (UBook != null) 
            {
                if (!CheckBookCode)
                {

                    //var Book_ = libraryEntities.Book.Find(id);
                    UBook.BookCode = book.BookCode;
                    UBook.IdTypeOfBook = book.IdTypeOfBook;
                    libraryEntities.SaveChanges();
                    return Ok(UBook);
                }
                else
                {
                    return BadRequest("BookCode Already Exist !");
                }
            }
            else 
            {
                return NotFound();
            }
            


        }
        [HttpGet]
        public IHttpActionResult GetIdBook(int id)
        {
            var book = (from x in libraryEntities.Book
                        where x.Id == id
                        select x).FirstOrDefault();

            return Ok(book);
        }
        [HttpGet]
        public IHttpActionResult BookByName(string name)
        {
            var BookbyName = from x in libraryEntities.Book
                             join p in libraryEntities.TypeOfBook
                             on x.IdTypeOfBook equals p.Id
                             where p.Category.Contains(name) || string.IsNullOrEmpty(name)
                             select new
                             {
                                 x.Id, x.BookCode, p.Category
                             };
            return Ok(BookbyName);
        }
    }
}
