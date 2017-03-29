using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Logs
{
    public class UserActiveLog : IAggregateRoot
    {
        [Key, ForeignKey("User")]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime ActionDate { get; set; }
        
        public virtual PaskolUser User { get; set; }
    }
}
