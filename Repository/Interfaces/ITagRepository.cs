using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag, Guid>
    {
        IEnumerable<Tag> GetAllByParentId(Guid? parentId);
        Tag GetByID(Guid id,
            ModelGetNavigtorOptions getParentOprions,
            ModelGetNavigtorOptions getChildrenOptions);
        IEnumerable<Tag> GetBySearch(string tagName);
        IEnumerable<Tag> GetTagList(IEnumerable<Guid> tagIds);
        Tag GetNewInSystemTag();
    }
}
