using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using Repository.UnitOfWork;
using Repository.Interfaces;
using Model;

namespace Repository
{
    public class TagsRepository : GenericRepository<Tag, Guid> ,ITagRepository
    {
        const string NEW_IN_SYSTEM = "חדש במערכת";
        public TagsRepository(IUnitOfWork uof) : base(uof)
        {
        }

        public IEnumerable<Tag> GetAllByParentId(Guid? parentId)
        {
            return dbSet.Where(tag => tag.ParentTagID == parentId).ToList();
        }

        public IEnumerable<Tag> GetBySearch(string tagName)
        {
            return dbSet
                .Include("ParentTag")
                .Where(tag => tag.Name.StartsWith(tagName) && tag.IsDynamic == false)
                .ToList();
        }

        public Tag GetNewInSystemTag()
        {
            return dbSet.FirstOrDefault(tg => tg.Level == TagLevel.Root && tg.Name.Contains(NEW_IN_SYSTEM));
        }

        public override void Delete(Tag entityToDelete)
        {
            while (entityToDelete.TagList?.Count > 0)
            {
                Delete(entityToDelete.TagList.First());
            }

            base.Delete(entityToDelete);
        }

        public Tag GetByID(Guid id, 
            ModelGetNavigtorOptions getParentOprions,
            ModelGetNavigtorOptions getChildrenOptions)
        {
            Tag res = null;
            DbQuery<Tag> tagQuery;
            Expression<Func<Tag, bool>> predicate = (tag => tag.ID == id);
            if (getParentOprions == ModelGetNavigtorOptions.GetNavigatorRecursive)
            {
                tagQuery = this.dbSet.Include("ParentTag");
            }
            else
            {
                tagQuery = this.dbSet;
            }
            if (getChildrenOptions == ModelGetNavigtorOptions.GetNavigatorRecursive)
            {
                tagQuery.Include("TagList");
            }
            res = tagQuery.First(predicate);
            if (getParentOprions == ModelGetNavigtorOptions.GetNavigator)
            {
                // EF is fichsa
                res.ParentTag?.ToString();
            }
            if (getChildrenOptions == ModelGetNavigtorOptions.GetNavigator)
            {
                res.TagList = res.TagList ?? new List<Tag>();
            }

            return res;
        }

        public IEnumerable<Tag> GetTagList(IEnumerable<Guid> tagIds)
        {
            return dbSet.Where(tg => tagIds.Any(t => tg.ID == t));
        }
    }
}
