using Infrastructure.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Logs
{
    public class NCPurchase : IAggregateRoot
    {
        [Key, ForeignKey("Purchase")]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Note { get; set; }
        public Purchase.Purchase Purchase { get; set; }
    }
}
