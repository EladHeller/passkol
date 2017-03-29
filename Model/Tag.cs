using Infrastructure.Domain;
using Model.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Tag : IAggregateRoot
    {
        public Tag()
        {
        }

        public Tag(Tag tg)
        {
            this.IsCanDelete = tg.IsCanDelete;
            this.IsDynamic = tg.IsDynamic;
            this.IsPublicTag = tg.IsPublicTag;
            this.Level = tg.Level;
            this.Name = tg.Name;
            this.ParentTagID = tg.ParentTagID;
            this.ViewOrder = tg.ViewOrder;
        }

        public Guid ID { get; set; }
        public Guid? ParentTagID { get; set; }
        public virtual Tag ParentTag { get; set; }
        [Display(Name = "שם")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="השדה שם נדרש")]
        public string Name { get; set; }
        public bool IsCanDelete { get; set; }
        [Display(Name = "תיוג ציבורי")]
        public bool IsPublicTag { get; set; }
        public bool IsDynamic { get; set; }
        public TagLevel Level { get; set; }
        [Display(Name = "סדר תצוגה")]
        [Range(1,int.MaxValue,ErrorMessage ="יש להזין מספר הגדול מ-0")]
        public int ViewOrder { get; set; }
        public virtual ICollection<Tag> TagList { get; set; }
        public virtual ICollection<Music> MusicList { get; set; }
        public ICollection<SearchLog> Tags { get; set; }
    }

    public enum TagLevel
    {
        Root=1,
        Second=2,
        Bottom =3
    }
}
