using Infrastructure.Domain;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Logs
{
    public class NCDataUpload : IAggregateRoot
    {
        [Key, ForeignKey("Music")]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Note { get; set; }
        public string UploaderName { get; set; }
        public string UploaderMail { get; set; }
        public string UploaderPhone { get; set; }

        public virtual Music Music { get; set; }
    }
}
