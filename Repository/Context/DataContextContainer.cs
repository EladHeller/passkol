//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Model;
//using System.Web;

//namespace Repository.Context
//{
//    public class DataContextContainer : IDataContextStorageContainer
//    {
//        private string _dataContextKey = "PaskolContext"; 

//        public PaskolDbContext GetDataContext()
//        {
//            PaskolDbContext Context = null;
//            if (HttpContext.Current.Items.Contains(_dataContextKey))
//            {
//                Context = (PaskolDbContext)HttpContext.Current.Items[_dataContextKey];
//            }

//            return Context;
//        }

//        public void Store(PaskolDbContext PaskolDbContext)
//        {
//            if (HttpContext.Current.Items.Contains(_dataContextKey))
//            {
//                HttpContext.Current.Items[_dataContextKey] = PaskolDbContext;
//            }
//            else
//            {
//                  HttpContext.Current.Items.Add(_dataContextKey, PaskolDbContext);
//            }
//        }
//    }
//}
