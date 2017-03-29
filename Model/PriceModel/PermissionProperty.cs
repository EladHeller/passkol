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
    public class PermissionProperty : IAggregateRoot
    {
        public PermissionProperty()
        {
            this.ID = Guid.NewGuid();
        }
        public PermissionProperty(Guid catId, string name, bool isActive, int viewOrder, bool isCostLevel)
        {
            this.ID = Guid.NewGuid();
            PermissionsCategoryID = catId;
            Name = name;
            this.IsActive = isActive;
            ViewOrder = viewOrder;
            IsCostLevel = isCostLevel;
        }
        public PermissionProperty(PermissionProperty prop)
        {
            ID = prop.ID;
            PermissionsCategoryID = prop.PermissionsCategoryID;
            Name = prop.Name;
            IsActive = prop.IsActive;
            ViewOrder = prop.ViewOrder;
            IsCostLevel = prop.IsCostLevel;
        }
        [Key]
        [Column(Order = 1)]
        public Guid ID { get; set; }
        [Column(Order = 2)]
        [Key, ForeignKey("PermissionsCategory")]
        public Guid PermissionsCategoryID { get; set; }
        public virtual PermissionsCategory PermissionsCategory { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="יש להזין שם.")]
        [Display(Name ="שם")]
        public string Name { get; set; }
        [Display(Name = "סטטוס")]
        public bool IsActive { get; set; }
        [Display(Name = "סדר תצוגה")]
        [Range(1,int.MaxValue,ErrorMessage ="יש להזין סדר תצוגה גדול מ-0")]
        public int ViewOrder { get; set; }
        public bool IsCostLevel { get; set; }

        public virtual List<PermissionPropertyValue> PropertyValues { get; set; }
    }
}
