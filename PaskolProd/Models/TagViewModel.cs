using Model;
using PaskolProd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaskolProd.Models
{
    public class TagViewModel : Tag
    {
        public TagViewModel()
        {

        }
        public TagViewModel(Tag tg, 
            Guid? id, 
            bool getParent, 
            bool getChildren, 
            bool sortByName,
            bool sortDesc) :base(tg)
        {
            this.SortDesc = sortDesc;
            this.SortByName = sortByName;
            this.NullableID = id;
            if (id.HasValue)
            {
                this.ID = id.Value;
            }
            if (getChildren)
            {
                this.TagList = tg.TagList;//?.Select(tag => new TagViewModel(tag, tag.ID,false,true,false,false)) as ICollection<TagViewModel>;
            }
            Tag currTag = tg;
            TagHirarchi = new List<string>(3);
            while (currTag != null)
            {
                TagHirarchi.Add(currTag.Name);
                currTag = currTag.ParentTag;
            }
            if (getParent && tg.ParentTag != null)
            {
                this.ParentTag = new TagViewModel(tg.ParentTag,tg.ParentTagID,true,false,false,false);
                
            }
        }
        public Guid? NullableID { get; set; }
        public List<string> TagHirarchi { get; set; }
        public bool SortDesc { get; set; }
        public bool SortByName { get; set; }
        public bool IsNotDeleted { get; set; }
    }
}