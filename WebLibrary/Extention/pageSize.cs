using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebLibrary.Extention
{


    public class ListPaging<T>
    {
        public IList<T> ListOut { get; set; }
        public int TotalCount { get; set; }
        public int PageStart { get; set; }
        public int PageEnd { get; set; }
        public int TotalPage { get; set; }
    }
    public static class PaginationSet
    {
        public static ListPaging<T> GetPagingList<T>(this IQueryable<T> listAll, int pageNumber, int pageSize)
        {

            if (pageNumber < 1 || pageSize < 0) throw new ArgumentOutOfRangeException("PageNumber or PageSize is out of range");
            ListPaging<T> listPaging = new ListPaging<T>();
            var pageIndex = pageNumber - 1;  //current page number
            listPaging.TotalCount = listAll.Count();  // total count record
            if (pageSize == 0) pageSize = listPaging.TotalCount == 0 ? 1 : listPaging.TotalCount;  // show all record
            listPaging.ListOut = listAll.Skip(pageSize * pageIndex).Take(pageSize).ToList();  // display record in one page, when client chose page number
            listPaging.TotalPage = System.Convert.ToInt32(System.Math.Ceiling(listPaging.TotalCount / System.Convert.ToDouble(pageSize)));// number of page
            listPaging.PageStart = (pageIndex * pageSize) + 1;  //number of records in page start
            if (listPaging.TotalPage == pageNumber)
            {
                listPaging.PageEnd = listPaging.TotalCount;  // if current page is PageEnd display totalCount
            }
            else listPaging.PageEnd = (pageIndex * pageSize) + pageSize;  // else showing (pageIndex * pageSize) + pageSize to totalCount of totalCount entries
            return listPaging;

            
        }
        //public static async Task<ListPaging<T>> GetPagingList<T>(this IEnumerable<T> listAll, int pageNumber, int pageSize)
        //{
        //    if (pageNumber < 1 || pageSize < 0) throw new ArgumentOutOfRangeException("PageNumber or PageSize is out of range");
        //    ListPaging<T> listPaging = new ListPaging<T>();
        //    var pageIndex = pageNumber - 1;
        //    listPaging.TotalCount = listAll.Count();
        //    if (pageSize == 0) pageSize = listPaging.TotalCount == 0 ? 1 : listPaging.TotalCount;
        //    listPaging.ListOut = listAll.Skip(pageSize * pageIndex).Take(pageSize).ToList();
        //    listPaging.TotalPage = System.Convert.ToInt32(System.Math.Ceiling(listPaging.TotalCount / System.Convert.ToDouble(pageSize)));
        //    listPaging.PageStart = (pageIndex * pageSize) + 1;
        //    if (listPaging.TotalPage == pageNumber)
        //    {
        //        listPaging.PageEnd = listPaging.TotalCount;
        //    }
        //    else listPaging.PageEnd = (pageIndex * pageSize) + pageSize;
        //    return listPaging;
        //}
    }

}