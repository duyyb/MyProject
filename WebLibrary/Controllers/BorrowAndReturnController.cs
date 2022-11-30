using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLibrary.Extention;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class BorrowAndReturnController : ApiController
    {
        LibraryEntities2 libraryEntities = new LibraryEntities2();
        [HttpGet]
        public IHttpActionResult GetAllBorrowBook(int pageSize, int pageNumber)
        {
            var BorrowInfor = (from BAR in libraryEntities.BorrowAndReturn
                              join U in libraryEntities.User on BAR.IdUser equals U.Id into u
                              from users in u.DefaultIfEmpty()
                              join B in libraryEntities.Book on BAR.IdBook equals B.Id
                              join TOP in libraryEntities.TypeOfBook on B.IdTypeOfBook equals TOP.Id
                              where B.Status == true
                              select new
                              {
                                  BAR.Id,
                                  users.UserName,
                                  B.BookCode,
                                  TOP.Category,
                                  BAR.BorrowDay,
                                  BAR.ReturnDate,
                                  BAR.ReturnDay,
                                  B.Status
                              })
                              .OrderByDescending(x=>x.BorrowDay)
                              .GetPagingList(pageNumber, pageSize);

            return Ok(BorrowInfor);
        }
        [HttpGet]
        public IHttpActionResult GetAllAvaiableBook()
        {


            var BorrowInfor = from BAR in libraryEntities.BorrowAndReturn
                              join U in libraryEntities.User on BAR.IdUser equals U.Id into u
                              from users in u.DefaultIfEmpty()
                              join B in libraryEntities.Book on BAR.IdBook equals B.Id
                              join TOP in libraryEntities.TypeOfBook on B.IdTypeOfBook equals TOP.Id
                              where B.Status == false
                              select new
                              {
                                  BAR.Id,
                                  B.BookCode,
                                  TOP.Category,
                                  B.Status,
                                  BAR .ReturnDay
                              };

            return Ok(BorrowInfor);
        }
        public IHttpActionResult GetBorrowAndReturn(int id)
        {
            var borrowandreturn = libraryEntities.BorrowAndReturn.FirstOrDefault((p) => p.Id == id);
            if (borrowandreturn == null)
            {
                return NotFound();
            }
            return Ok(borrowandreturn);
        }
        [HttpPost]
        public IHttpActionResult Add_BorowAndReturn(BorrowAndReturn NewBRR)
        {
            var AddBRR = (from p in libraryEntities.BorrowAndReturn where p.IdBook == NewBRR.IdBook select p).Any();
            if (!AddBRR)
            {
                libraryEntities.BorrowAndReturn.Add(NewBRR);
            }
            libraryEntities.SaveChanges();
            return Ok(AddBRR);
        }
        [HttpDelete]
        public IHttpActionResult Delete_BorowAndReturn(int id)
        {
            var DelBRR = libraryEntities.BorrowAndReturn.FirstOrDefault((p) => p.Id == id);

            libraryEntities.BorrowAndReturn.Remove(DelBRR);
            libraryEntities.SaveChanges();
            return Ok(DelBRR);
        }
        [HttpPut]
        public IHttpActionResult Update_BorowBook(BorrowAndReturn borrowBook)
        {
            var borrowAndReturn = libraryEntities.BorrowAndReturn.FirstOrDefault(x => x.IdBook == borrowBook.Id);
            var book = libraryEntities.Book.FirstOrDefault(x => x.Id == borrowBook.Id);

            if (book.Status == false)
            {
                book.Status = true;
            }
            if (borrowAndReturn != null)
            {
                borrowAndReturn.BorrowDay = borrowBook.BorrowDay;
                borrowAndReturn.ReturnDate = borrowBook.ReturnDate;
                borrowAndReturn.IdUser = borrowBook.IdUser;
            }
            libraryEntities.SaveChangesAsync();

            return Ok(1);
        }
        [HttpPut]
        public IHttpActionResult Update_ReturnBook(BorrowAndReturn returnBook)
        {
            var book = libraryEntities.Book.FirstOrDefault(x => x.Id == returnBook.Id);
            var borrowAndReturn = libraryEntities.BorrowAndReturn.FirstOrDefault(x => x.Id == returnBook.Id);
            if (book.Status == true)
            {
                book.Status = false;
            }
            if (borrowAndReturn != null)
            {
                borrowAndReturn.ReturnDay = returnBook.ReturnDay;
                borrowAndReturn.ReturnDate = null;
                borrowAndReturn.IdUser = null;
                borrowAndReturn.ReturnDate = null;
            }
            return Ok(1);
        }
        [HttpPut]
        public IHttpActionResult Update_ReturnDate(BorrowAndReturn returnDate)
        {
            var book = libraryEntities.Book.FirstOrDefault(x => x.Id == returnDate.Id);
            var borrowAndReturn = libraryEntities.BorrowAndReturn.FirstOrDefault(x => x.Id == returnDate.Id);

            if (book.Status == true)
            {
                book.Status = false;
            }
            if (borrowAndReturn != null)
            {

                borrowAndReturn.ReturnDate = returnDate.ReturnDate;
            }
            return Ok(1);
        }

    }
}
    




