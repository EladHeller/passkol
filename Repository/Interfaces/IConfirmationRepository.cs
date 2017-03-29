using Model.Confirm;
using Repository.Messaging;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface IConfirmationRepository : IGenericRepository<Confirmation, int>
    {
        RepositoryPagingResponse<Confirmation> GetBySearch(IList<ConfirmType> Types, int max, int skip, ConfirmSorting sortOrder);
        Confirmation GetByEntityId(string Id);
        int GetConfirmationCount();
    }
}
