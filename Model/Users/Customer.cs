using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Domain;

namespace Model.Users
{
    public class Customer : IAggregateRoot
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public virtual PaskolUser User { get; set; }
    }
}
