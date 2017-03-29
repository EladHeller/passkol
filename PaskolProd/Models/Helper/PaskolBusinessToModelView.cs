using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Users;

namespace PaskolProd.Models.Helper
{
    public static class PaskolBusinessToModelView
    {
        public static PaskolUser UserFromArtistAgentDetails(this ArtistAgentEditView userDetails, PaskolUser user)
        {
            PaskolUser RetUser = userDetails.UserFromUserDetails(user);
            if (RetUser.ArtistAgent == null)
            {
                RetUser.ArtistAgent = new ArtistAgent();
            }

            RetUser.ArtistAgent.CompanyName = userDetails.CompanyName;
            RetUser.UserType = UserType.ArtistAgent;
            return RetUser;
        }

        public static PaskolUser UserFromCustomerDetails(this CustomerEditView userDetails, PaskolUser user)
        {
            PaskolUser RetUser = userDetails.UserFromUserDetails(user);
            if (RetUser.Customer == null)
            {
                RetUser.Customer = new Customer();
            }

            RetUser.Customer.CompanyName = userDetails.CompanyName;

            RetUser.UserType = UserType.Customer;
            return RetUser;
        }

        public static PaskolUser UserFromArtistEditView(this ArtistEditView userDetails, PaskolUser user)
        {
            PaskolUser RetUser = userDetails.UserFromUserDetails(user);
            if (RetUser.Artist == null)
            {
                RetUser.Artist = new Artist();
            }
            
            RetUser.Artist.ContactManName = userDetails.ContactManName;
            RetUser.Artist.ContactManPhone = userDetails.ContactManPhone;
            RetUser.Artist.PageUrl = userDetails.PublicPage;
            RetUser.Artist.AgentName = userDetails.AgentName;
            RetUser.Artist.AgentPhone = userDetails.AgentPhone;
            RetUser.Artist.AgentMail = userDetails.AgentEmail;
            RetUser.Artist.Biography = userDetails.Biography;
            RetUser.Artist.ParticipateInAuction = userDetails.ParticipateInAuction;
            RetUser.Artist.PriceLevel = userDetails.PriceLevel;

            RetUser.UserType = UserType.Artist;
            return RetUser;
            
        }

        public static PaskolUser UserFromUserDetails(this UserEditView userDetails, PaskolUser user)
        {
            if (user == null)
            {
                user = new PaskolUser();
                user.RegisteredDate = DateTime.Now;
            }
            
            user.UserName = userDetails.Name;
            user.Email = userDetails.Email;
            user.PhoneNumber = userDetails.Phone;
            user.Note = userDetails.Note;
            return user;
        }
        
        public static ManagersCreateView CreateManagerViewFrom(this PaskolUser user)
        {
            return new ManagersCreateView()
            {
                UserID = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.RoleId).ToList(),
                Note = user.Note
            };
        }
    }
}