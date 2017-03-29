//using Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Repository.Context
//{
//    public class DataContextFactory
//    {
//        public static PaskolDbContext GetDataContext()
//        {
//            IDataContextStorageContainer _dataContextStorageContainer = new DataContextContainer();
//            PaskolDbContext paskolcontext = _dataContextStorageContainer.GetDataContext();
//            if (paskolcontext == null)
//            {
//                paskolcontext = new PaskolDbContext();
//                _dataContextStorageContainer.Store(paskolcontext);
//            }

//            return paskolcontext;
//        }
//    }
//}
