using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository.Messaging;
using Infrastructure.Domain;
using Repository;
using Repository.UnitOfWork;

namespace Repository
{
    public class FileRepository : GenericRepository<File,int>
    {
        public FileRepository(IUnitOfWork uof) : base(uof)
        {
        }
    }
}
