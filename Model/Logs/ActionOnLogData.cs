using Infrastructure.Domain;
using Model.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logs
{
    public class ActionOnLogData : IAggregateRoot
    {
        [Key]
        [Column(Order =1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string EntityId { get; set; }
        [Key]
        [Column(Order = 3)]
        public EntityType EntityType { get; set; }
        [Key]
        [Column(Order = 4)]
        public LogActionType ActionType { get; set; }
        [Key]
        [Column(Order = 5)]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        public string UserID { get; set; }
        public virtual PaskolUser User { get; set; }

        public int Value { get; set; }
    }
}
