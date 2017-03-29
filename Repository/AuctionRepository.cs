using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using Repository.UnitOfWork;
using Repository.Interfaces;
using Model;
using Model.Auctions;
using Repository.Messaging;
using System.Data.Entity;
using Model.ContextDB;

namespace Repository
{
    public class AuctionRepository : GenericRepository<Auction, string> ,IAuctionRepository
    {
        public AuctionRepository(IUnitOfWork uof) : base(uof)
        {
        }

        private IEnumerable<Auction> ValidateAuctionStatus(IEnumerable<Auction> actions)
        {
            foreach (var action in actions)
            {
                AuctionStatus OldStatus = action.AuctionStatus;

                if ((action.AuctionStatus == AuctionStatus.Open || 
                    action.AuctionStatus == AuctionStatus.PickWinnerWating) && 
                    action.PickWinnerDate < DateTime.Now)
                {
                    action.AuctionStatus = AuctionStatus.CloseWating;
                }
                else if(action.AuctionStatus == AuctionStatus.Open && 
                    action.CloseDate < DateTime.Now)
                {
                    action.AuctionStatus = AuctionStatus.PickWinnerWating;
                }

                if (OldStatus != action.AuctionStatus)
                {
                    this.Update(action);
                    this._uow.Commit();
                }
            }

            return actions;
        }

        public RepositoryPagingResponse<Auction> Get(int max, int skip, AuctionSort sortOrder, string UserId = null)
        {
            RepositoryPagingResponse<Auction> res =
                new RepositoryPagingResponse<Auction>();

            IQueryable<Auction> query = this.dbSet;

            res.TotalResults = query.Count();

            if (UserId != null)
            {
                query = query.Where(a => a.Customer.Id == UserId);
            }

            // Sort by param
            switch (sortOrder)
            {
                case AuctionSort.OpenDate:
                    query = query.OrderBy(a => a.OpenDate);
                    break;
                case AuctionSort.Status:
                    query = query.OrderBy(a => a.AuctionStatus);
                    break;
                case AuctionSort.StatusDesc:
                    query = query.OrderByDescending(a => a.AuctionStatus);
                    break;
                case AuctionSort.CloseDate:
                    query = query.OrderBy(a => a.CloseDate);
                    break;
                case AuctionSort.CloseDateDesc:
                    query = query.OrderBy(a => a.CloseDate);
                    break;
                case AuctionSort.PickWinnerDate:
                    query = query.OrderBy(a => a.PickWinnerDate);
                    break;
                case AuctionSort.PickWinnerDateDesc:
                    query = query.OrderByDescending(a => a.PickWinnerDate);
                    break;
                default:
                    query = query.OrderByDescending(a => a.OpenDate);
                    break;
            }
            
            res.Entities = ValidateAuctionStatus(ValidateAuctionStatus(query
                .Skip(skip)
                .Take(max)
                .ToList() as IEnumerable<Auction>));

            return res;
        }

        public int GetAuctionsCountByStatus(AuctionStatus status)
        {
            return dbSet.Where(auct => auct.AuctionStatus == status).Count();
        }

        public IEnumerable<Auction> GetAuctionsForArtist(string userID)
        {
            return ValidateAuctionStatus(dbSet.Where(a => a.Participants.Any(p => p.Id == userID)).ToList());
        }

        public IEnumerable<Auction> GetAuctionsForCustomer(string userID)
        {
            return ValidateAuctionStatus(dbSet.Where(a => a.Customer.Id == userID).ToList());
        }
    }
}
