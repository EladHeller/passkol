using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MailUnsubscribe : IAggregateRoot
    {
        [Key]
        public string Email { get; set; }
        public string Guid { get; set; }
        public bool Unsubscribe { get; set; }
    }
}
