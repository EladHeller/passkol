using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;

namespace Model.Authentication
{
    public class PaskolInitializiar : System.Data.Entity.DropCreateDatabaseIfModelChanges<PaskolDbContext>
    {
        protected override void Seed(PaskolDbContext context)
        {
            List<Tag> tags = new List<Tag>()
            {
                new Tag()
                {
                    Name ="מילות מפתח",
                    IsCanDelete = false,
                    IsDynamic = true,
                    IsPublicTag = false,
                    Level = TagLevel.Root,
                    ViewOrder = 30,
                    CreateDate = DateTime.Now,
                },
                new Tag()
                {
                    Name ="חדש במערכת",
                    IsCanDelete = false,
                    IsDynamic = true,
                    IsPublicTag = false,
                    Level = TagLevel.Root,
                    ViewOrder = 30,
                    CreateDate = DateTime.Now,
                },
                new Tag()
                {
                    Name ="נשמע כמו - Sound alike",
                    IsCanDelete = false,
                    IsDynamic = false,
                    IsPublicTag = false,
                    Level = TagLevel.Root,
                    ViewOrder = 30,
                    CreateDate = DateTime.Now,
                }
            };
            context.Tags.AddRange(tags);
            context.SaveChanges();
        }
    }
}
