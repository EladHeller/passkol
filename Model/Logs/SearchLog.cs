using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Model.Logs
{
    public class SearchLog : IAggregateRoot
    {
        [Key, ForeignKey("User")]
        [Column(Order = 1)]
        public string UserID { get; set; }
        public virtual PaskolUser User { get; set; }
        [Key]
        [Column(Order = 2)]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        [NotMapped]
        public string TagIds
        {
            get
            {
                return string.Join(";", Tags.Select(t=>t.ID));
            }
        }
        [NotMapped]
        public string TagsNames
        {
            get
            {
                return string.Join(", ", Tags.Select(t => t.Name));
            }
        }
        [NotMapped]
        public string SearchUrl
        {
            get
            {
                return string.Format("/Search?searchText={0}&strTagIds={1}", SearchString, TagIds);
            }
        }
        public string SearchString { get; set; }
        public int ResultCount { get; set; }
    }
}
