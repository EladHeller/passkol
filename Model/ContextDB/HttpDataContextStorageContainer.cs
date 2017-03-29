using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.ContextDB
{
    public class HttpDataContextStorageContainer
    {
        private string _dataContextKey = "DataContext";

        public PaskolDbContext GetDataContext()
        {
            PaskolDbContext objectContext = null;
            if (HttpContext.Current.Items.Contains(_dataContextKey)) objectContext = 
                    (PaskolDbContext)HttpContext.Current.Items[_dataContextKey];
            return objectContext;
        }

        public void Store(PaskolDbContext pasloDb)
        {
            if (HttpContext.Current.Items.Contains(_dataContextKey))
                HttpContext.Current.Items[_dataContextKey] = pasloDb;
            else
                HttpContext.Current.Items.Add(_dataContextKey, pasloDb);
        }

    }
}
