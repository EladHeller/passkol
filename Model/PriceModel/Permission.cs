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
    public class Permission : IAggregateRoot
    {
        public Permission()
        {
            this.ID = Guid.NewGuid();
        }
        public Permission(Guid catId, double? cost)
        {
            this.ID = Guid.NewGuid();
            PermissionCategoryID = catId;
            PermissionCost = cost;
        }

        [Key]
        [Column(Order =1)]
        public Guid ID { get; set; }
        public Guid PermissionCategoryID { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "מחיר חייב להיות מספר חיובי")]
        public double? PermissionCost { get; set; }
        public virtual List<PermissionPropertyValue> PropertyValues { get; set; }
    }
}
