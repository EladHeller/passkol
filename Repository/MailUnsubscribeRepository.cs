using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.UnitOfWork;
using Repository.Interfaces;

namespace Repository
{
    public class MailUnsubscribeRepository : GenericRepository<MailUnsubscribe, string>, IMailUnsubscribeRepository
    {
        public IUnitOfWork Uow;
        public MailUnsubscribeRepository(IUnitOfWork uow) : base(uow)
        {
            Uow = uow;
        }
        
    }
}
