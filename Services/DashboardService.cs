using Model;
using Model.Auctions;
using Model.Logs;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DashboardService
    {
        private IUnitOfWork _uow;
        private INCRegisterRepository ncRegRep;
        private INCDataUploadRepository ncDataUpRep;
        private INCPurchaseRepository ncPurchaseRep;
        private IAuctionRepository auctRep;
        private IPurchaseRepository purchRep;
        private IConfirmationRepository confRep;

        public DashboardService(IUnitOfWork uow, 
            INCRegisterRepository ncRegRep, 
            INCDataUploadRepository ncDataUpRep,
            INCPurchaseRepository ncPurchaseRep,
            IAuctionRepository auctRep,
            IPurchaseRepository purchRep,
            IConfirmationRepository confRep)
        {
            this.auctRep = auctRep;
            this.confRep = confRep;
            this.ncDataUpRep = ncDataUpRep;
            this.ncPurchaseRep = ncPurchaseRep;
            this.ncRegRep = ncRegRep;
            this.purchRep = purchRep;
        }
        public EntityResponse<DashboardData> GetDashboardData()
        {
            var res = new EntityResponse<DashboardData>();
            res.Entity = new DashboardData();
            try
            {
                var now = DateTime.Now;
                var weekAgo = DateTime.Now.AddDays(-7);
                res.Entity.AuctionForConfirmCount = auctRep.GetAuctionsCountByStatus(AuctionStatus.ConfirmWating);
                res.Entity.AuctionForCloseCount = auctRep.GetAuctionsCountByStatus(AuctionStatus.CloseWating);
                res.Entity.NCDataUploadCount = ncDataUpRep.GetNCDataUploadCountByDates(start:weekAgo,end:now);
                res.Entity.NCProfileRegisterCount = 
                    ncRegRep.GetNCRegisterCount(start: weekAgo, end: now, type: NCUserType.ArtistProfile);
                res.Entity.NCPurchaseCount = ncPurchaseRep.GetNCPurchaseCountByDates(now, weekAgo);
                res.Entity.NCRegisterCount = 
                    ncRegRep.GetNCRegisterCount(start: weekAgo, end: now, type: NCUserType.Register);
                res.Entity.PurchaseForPaymentRegulation = purchRep.GetWaitingForPaymentRegulatingCount();
                res.Entity.RequestsForConfirmCount = confRep.GetConfirmationCount();
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = "אירעה שגיאה בעת הבאת הנתונים מהשרת.";
                res.Success = true;
            }

            return res;
        }
    }
}
