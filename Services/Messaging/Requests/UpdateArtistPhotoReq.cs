using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Messaging.Requests
{
    public class UpdateArtistPhotoReq
    {
        public HttpPostedFileBase upload { get; set; }
        public PaskolUser user { get; set; }
    }
}
