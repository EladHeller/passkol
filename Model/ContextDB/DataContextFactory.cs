using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ContextDB
{
    public class DataContextFactory
    {
        public static PaskolDbContext GetDataContext()
        {
            HttpDataContextStorageContainer _dataContextStorageContainer = new HttpDataContextStorageContainer();
            PaskolDbContext paskolDb =
                _dataContextStorageContainer.GetDataContext();
            if (paskolDb == null)
            {
                paskolDb = new PaskolDbContext();
                _dataContextStorageContainer.Store(paskolDb);
            }

            return paskolDb;
        }
    }
}
