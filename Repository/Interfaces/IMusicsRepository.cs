using Model;
using Repository.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMusicsRepository : IGenericRepository<Music, int>
    {
        RepositoryPagingResponse<Music> GetBySearch(
            string searchMusicText,
            string searchArtistText,
            int max,
            int skip,
            MusicFieldsSort sortField,
            bool orderDesc);
        int GetCount(DateTime? start, DateTime? end);
        RepositoryPagingResponse<Music> GetBySearch(string searchText, IEnumerable<Guid> tagIds, int resultInPage, int skip, SearchSortType sortType);
        RepositoryPagingResponse<Music> GetByArtistId(string artistId, int resultInPage, int v, SearchSortType sortType);
    }
}
