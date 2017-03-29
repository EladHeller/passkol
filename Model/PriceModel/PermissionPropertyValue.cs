using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PermissionPropertyValue : IAggregateRoot
    {
        public PermissionPropertyValue()
        {
            this.ID = Guid.NewGuid();
        }
        public PermissionPropertyValue(Guid propId, Guid catId, string name, bool isActive, int viewOrder)
        {
            this.ID = Guid.NewGuid();
            PermissionPropertyId = propId;
            PermissionsCategoryID = catId;
            Name = name;
            this.IsActive = isActive;
            ViewOrder = viewOrder;
        }

        public PermissionPropertyValue(PermissionPropertyValue val)
        {
            ID = val.ID;
            PermissionPropertyId = val.PermissionPropertyId;
            PermissionsCategoryID = PermissionsCategoryID;
            Name = val.Name;
            IsActive = val.IsActive;
            ViewOrder = val.ViewOrder;
        }

        [Key]
        [Column(Order = 1)]
        public Guid ID { get; set; }
        [Column(Order = 2)]
        [Key, ForeignKey("PermissionProperty")]
        public Guid PermissionPropertyId { get; set; }
        [Column(Order = 3)]
        [Key, ForeignKey("PermissionProperty")]
        public Guid PermissionsCategoryID { get; set; }
        public virtual PermissionProperty PermissionProperty { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "יש להזין שם.")]
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Display(Name = "סטטוס")]
        public bool IsActive { get; set; }
        [Display(Name = "סדר תצוגה")]
        [Range(1, int.MaxValue, ErrorMessage = "יש להזין סדר תצוגה גדול מ-0")]
        public int ViewOrder { get; set; }
        public virtual ICollection<Permission> PermissionPrices { get; set; }
    }
}
