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
    public class PermissionsCategory : IAggregateRoot
    {
        public PermissionsCategory()
        {
            this.ID = Guid.NewGuid();
        }
        [Key]
        public Guid ID { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="יש להזין שם לקטגוריה.")]
        [Display(Name = "שם")]
        public string Name { get; set; }
        [Display(Name = "סטטוס")]
        public bool IsActive { get; set; }
        [Display(Name = "סדר תצוגה")]
        [Range(1, int.MaxValue, ErrorMessage = "יש להזין סדר תצוגה גדול מ-0")]
        public int ViewOrder { get; set; }
        public virtual List<PermissionProperty> PermissionProperties { get; set; }
        /// <summary>
        /// This is NO navigate property!
        /// </summary>
        [NotMapped]
        public virtual List<Permission> Permissions { get; set; }

        public static List<PermissionProperty> CreateCostLevelProperties(Guid catId)
        {
            var props = new List<PermissionProperty>(2)
            {
                new PermissionProperty(catId,"רמת אמן",true,1,true)
                {
                    PropertyValues = CreateCostLevelValues()
                },
                new PermissionProperty(catId,"רמת יצירה",true,2,true){
                    PropertyValues = CreateCostLevelValues()
                }
            };
            return props;
        }
        private static List<PermissionPropertyValue> CreateCostLevelValues()
        {
            return new List<PermissionPropertyValue>(3)
            {
                
                new PermissionPropertyValue()
                {
                    IsActive = true,
                    Name="גבוה",
                    ViewOrder = 1,
                },
                new PermissionPropertyValue()
                {
                    IsActive = true,
                    Name="בינוני",
                    ViewOrder = 2,
                },
                new PermissionPropertyValue()
                {
                    IsActive = true,
                    Name="נמוך",
                    ViewOrder = 3,
                },
            };
        }
    }
}