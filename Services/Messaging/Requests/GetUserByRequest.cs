using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Users;

namespace Services.Messaging.Requests
{
    public class GetUserByRequest
    {
        public List<UserType> WhereTypes { get; set; }
        public string UserName { get; set; }

        public string ArtistContactMan { get; set; }

        public GetUserByRequest()
        {
            this.WhereTypes = new List<UserType>();
        }
    }
}
