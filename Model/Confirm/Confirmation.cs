using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Confirm
{
    public class Confirmation : IAggregateRoot
    {
        public int ConfirmationId { get; set; }
        public ConfirmType ConfirmType { get; set; }
        public string Name { get; set; }
        public DateTime DateUpdate { get; set; }
        public string ManagerNote { get; set; }
        public string EntityId { get; set; }
    }
}
