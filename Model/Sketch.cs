using Infrastructure.Domain;
using Model.Logs;
using Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Sketch : IAggregateRoot
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ArtistID { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual File MP3File { get; set; }
        public string MP3FileID { get; set; }
        public virtual File WAVFile { get; set; }
        public string WAVFileID { get; set; }
        [Display(Name ="תאריך יצירה")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "הערת מנהל")]
        public string Comment { get; set; }
        public bool Purchased { get; set; }

        public Sketch()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
