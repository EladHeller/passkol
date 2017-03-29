using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum FileType
    {
        [Display(Name = "תמונה")]
        Photo,
        [Display(Name ="יצירה")]
        Music,
        [Display(Name = "PDF")]
        PDF
    }
}
