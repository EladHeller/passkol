using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.Responses
{
    public class SavePhotoResponse : ResponseBase
    {
        public string PhotoId { get; set; }
    }
}
