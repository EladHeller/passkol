using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Model.Helper
{
    public static class ModelStateExt
    {
        public static string[] GetErrorsFor(this ModelStateDictionary m, string filed)
        {
            foreach (var va in m.Values)
            {
                foreach (var err in va.Errors)
                {
                    var errd = err;
                    
                }
            }
            return new string[] { "sd" };
        }
    }
}
