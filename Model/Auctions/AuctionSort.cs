using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Auctions
{
    public enum AuctionSort
    {
        OpenDate,
        OpenDateDesc,
        Status,
        StatusDesc,
        CloseDate,
        CloseDateDesc,
        PickWinnerDate,
        PickWinnerDateDesc
    }
}
