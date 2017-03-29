using Infrastructure.Domain;
using Model.Logs;
using Model.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Purchase
{
    public class Purchase : IAggregateRoot
    {
        public string PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public virtual PaskolUser Customer { get; set; }
        public int MusicID { get; set; }
        public virtual Music Music { get; set; }
        public virtual Permission Permision { get; set; }
        public string CustomerReference { get; set; }
        public string Note { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "מחיר חייב להיות מספר חיובי")]
        public double? PurchaseCost { get; set; }
        public double? ArtistEarn { get; set; }
        public double? PasskolEarn { get; set; }
        public string PaidToArtistReference { get; set; }
        public string PaidToArtistNote { get; set; }
        public string CustomerPaidNote { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }
        public virtual NCPurchase NCPurchase { get; set; }
        public virtual Model.File PurchaseAgreement { get; set; }

        public Purchase()
        {
            PurchaseId = Guid.NewGuid().ToString();
        }
    }
}
