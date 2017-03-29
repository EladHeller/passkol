using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Purchase
{
    public enum PurchaseStatus
    {
        [Display(Name = "שולם לאמן")]
        PaidToArtist,

        [Display(Name = "לא שולם לאמן")]
        NotPaidToArtist,

        [Display(Name = "לא שולם")]
        NotPaidBycustomer
    }
}
