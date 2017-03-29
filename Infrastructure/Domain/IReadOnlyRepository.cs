using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Infrastructure.Domain
{
    // ReadOnly Rep
    public interface IReadOnlyRepository<T, TId> where T : IAggregateRoot
    {
        T FindBy(TId id);
        IEnumerable<T> FindAll();
    }
}
