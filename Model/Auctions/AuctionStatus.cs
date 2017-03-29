using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Auctions
{
    public enum AuctionStatus
    {
        [Display(Name = "ממתין לאישור")]
        ConfirmWating,
        [Display(Name = "פתוח להצעות")]
        Open,
        [Display(Name = "ממתין לבחירת זוכה")]
        PickWinnerWating,
        [Display(Name = "נסגר")]
        close,
        [Display(Name = "ממתין לסגירה")]
        CloseWating
    }
}
