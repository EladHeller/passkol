using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Services.Messaging.Responses;

namespace Services
{
    public class MailUnsubscribeService : GenericServiceUOW<MailUnsubscribe, string>
    {
        public MailUnsubscribeService(IMailUnsubscribeRepository repository, IUnitOfWork uow) : base(repository, uow)
        {
        }
    }
}
