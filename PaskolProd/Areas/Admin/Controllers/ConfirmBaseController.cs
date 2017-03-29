using Infrastructure.Configuration;
using Infrastructure.Email;
using Model.Confirm;
using Model.Users;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Areas.Admin.Controllers
{
    public class ConfirmBaseController : UserBaseController
    {
        protected MusicService musicSrv;
        protected ConfirmationService service;
        protected PaskolEmailService MAILService;
        protected PurchaseService purchaseService;

        public ConfirmBaseController()
        {
            purchaseService = ServiceLocator.GetService<PurchaseService>();
            service = ServiceLocator.GetService<ConfirmationService>();
            MAILService = ServiceLocator.GetService<PaskolEmailService>();
            musicSrv = ServiceLocator.GetService<MusicService>();
        }

        public async Task<bool> ConfirmNewArtist(PaskolUser User, ConfirmTypeAction Action, string Password
            ,string DeclineReason)
        {
            bool ActionSuccess = false;

            switch (Action)
            {
                case ConfirmTypeAction.Ok:
                    User.Status = UserStatus.Active;
                    break;
                case ConfirmTypeAction.Decline:
                    User.Status = UserStatus.WaitingNewArtist;
                    MAILService.DeclineNewUpdateArtist(User.Email,DeclineReason);
                    break;
                case ConfirmTypeAction.Block:
                    User.Status = UserStatus.Blocked;
                    break;
            }
            
            if (await UpdateUserAsync(User, Password))
            {
                var res = service.Delete(User.Id);
                if (res.Success)
                {
                    ActionSuccess = true;
                }
                else
                {
                    ModelState.AddModelError("", res.Message);
                }
            }

            return ActionSuccess;
        }
        
        public async Task<bool> ConfirmEditedArtist(PaskolUser User, ConfirmTypeAction Action, string DeclineReason)
        {
            bool ActionSuccess = false;

            switch (Action)
            {
                case ConfirmTypeAction.Ok:
                    User.Status = UserStatus.Active;
                    if (!string.IsNullOrEmpty(User.Artist.TempArtist.Email))
                    {
                        User.Email = User.Artist.TempArtist.Email;
                    }

                    if (!string.IsNullOrEmpty(User.Artist.TempArtist.UserName))
                    {
                        User.UserName = User.Artist.TempArtist.UserName;
                    }
                    
                    break;
                case ConfirmTypeAction.Decline:
                    User.Status = UserStatus.WaitingNewArtist;
                    MAILService.DeclineNewUpdateArtist(User.Email,DeclineReason);
                    break;
                case ConfirmTypeAction.Block:
                    // should delete from temp???
                    User.Status = UserStatus.Blocked;
                    break;
            }

            if (await UpdateUserAsync(User, null))
            {
                var res = service.Delete(User.Id);
                if (res.Success)
                {
                    ActionSuccess = true;
                }

                ModelState.AddModelError("", res.Message);
            }

            return ActionSuccess;
        }
    }
}