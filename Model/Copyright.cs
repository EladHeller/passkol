using Infrastructure.Domain;
using Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum CopyrightType
    {
        [Display(Name = "מו\"ל")]
        Publisher,
        [Display(Name ="כותב")]
        Writer,
        [Display(Name ="מלחין")]
        Composer,
        [Display(Name ="מעבד")]
        Cultivator
    }
    public class Copyright : IAggregateRoot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order =3)]
        public int ID { get; set; }
        [Column(Order =1)]
        [Key, ForeignKey("Music")]
        public int MusicID { get; set; }
        public virtual Music Music { get; set; }
        [Required]
        public CopyrightType Type { get; set; }
        [Range(0,100,ErrorMessage ="יש להזין מספר בין 0 ל-100")]
        public int Percents { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="יש להזין שם בעל זכויות.")]
        public string CopyrightAuthorName { get; set; }

        public Copyright()
        {

        }

        public Copyright(Copyright copyright)
        {
            CopyrightAuthorName = copyright.CopyrightAuthorName;
            ID = copyright.ID;
            IsDeleted = copyright.IsDeleted;
            MusicID = copyright.MusicID;
            Percents = copyright.Percents;
            Type = copyright.Type;
        }
    }
}
