using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Website.Models
{
    public static class PagingExtention
    {
        private enum PaginationType { Current = 1, Next, Previous, Other }

        private static string GetPaginationLink(int PageNumber, PaginationType PageType)
        {
            string strURL = string.Empty;
            switch (PageType)
            {
                case PaginationType.Current:
                    strURL = String.Format("<li class=\"active\"><span>{0}</span></li>", PageNumber);
                    break;
                case PaginationType.Next:
                    strURL = String.Format("<li class=\"next last\"><a href=\"javascript:void(0)\" page-number=\"{0}\">Next</a></li>", PageNumber);
                    break;
                case PaginationType.Previous:
                    strURL = String.Format("<li class=\"prev first\"><a href=\"javascript:void(0)\" page-number=\"{0}\">Previous</a></li>", PageNumber);
                    break;
                case PaginationType.Other:
                    strURL = String.Format("<li><a href=\"javascript:void(0)\" page-number=\"{0}\">{0}</a></li>", PageNumber);
                    break;
            }
            return strURL;
        }

        //public static HtmlString Pager(this HtmlHelper helper, int currentPage, int currentPageSize, int totalRecords)
        //{
        //    StringBuilder sb1 = new StringBuilder();
        //    if (totalRecords > currentPageSize)
        //    {
        //        if (currentPage > 1)
        //            sb1.Append(GetPaginationLink((currentPage - 1), PaginationType.Previous));

        //        int intPage = 0;
        //        int intRecordCount = totalRecords;
        //        while (intRecordCount > 0)
        //        {
        //            intPage = intPage + 1;
        //            if (intPage == currentPage)
        //                sb1.Append(GetPaginationLink(currentPage, PaginationType.Current));
        //            else
        //                sb1.Append(GetPaginationLink(intPage, PaginationType.Other));
        //            intRecordCount = intRecordCount - currentPageSize;
        //        }

        //        if (currentPage < intPage)
        //            sb1.Append(GetPaginationLink((currentPage + 1), PaginationType.Next));
        //    }
        //    return new HtmlString(sb1.ToString());
        //}

        public static HtmlString Pager(this HtmlHelper helper, int currentPage, int currentPageSize, int totalRecords)
        {
            StringBuilder sb1 = new StringBuilder();
            if (totalRecords > currentPageSize)
            {
                if (currentPage > 1)
                    sb1.Append(GetPaginationLink((currentPage - 1), PaginationType.Previous));

                int intPage = 0;
                int intStartPoint = (currentPage - 5) <= 1 ? 1 : (currentPage - 5);
                int intEndPoint = intStartPoint + 9;
                int intRecordCount = totalRecords;
                while (intRecordCount > 0)
                {
                    intPage = intPage + 1;
                    if (intStartPoint <= intPage)
                    {
                        if (intPage == currentPage)
                            sb1.Append(GetPaginationLink(currentPage, PaginationType.Current));
                        else
                            sb1.Append(GetPaginationLink(intPage, PaginationType.Other));
                    }
                    if (intPage == intEndPoint)
                        break;
                    intRecordCount = intRecordCount - currentPageSize;
                }

                if (currentPage < intPage)
                    sb1.Append(GetPaginationLink((currentPage + 1), PaginationType.Next));
            }
            return new HtmlString(sb1.ToString());
        }
    }
}