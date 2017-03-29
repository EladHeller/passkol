using System.Collections.Generic;
using System.Linq;
using Model;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using Repository.Messaging;
using Repository.UnitOfWork;
using Repository.Interfaces;
using System.Data.Entity;

namespace Repository
{
    public class SketchRepository : GenericRepository<Sketch, string>, ISketchRepository
    {
        public SketchRepository(IUnitOfWork uow) : base(uow) {}
       
    }
}
