using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaskolProd.Views.Helpers
{
    public static class StringHelper
    {
        //Truncates a string to be no longer than a certain length
        public static string TruncateWithEllipsis(string s)
        {
            const int length = 15;
            const string Ellipsis = "..";

            if (Ellipsis.Length > length)
                throw new ArgumentOutOfRangeException("length", length, "length must be at least as long as ellipsis.");

            if (s.Length > length)
            {
                // get first word
                s = s.IndexOf(' ') > -1 ? s.Substring(0, s.IndexOf(' ')) : s;

                if (s.Length > 18)
                {
                    s = s.Substring(0, length - Ellipsis.Length) + Ellipsis;
                }
                return s;
            }
            else
                return s;
        }
    }
}