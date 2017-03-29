using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum PriceLevel
    {
        [Display(Name = "גבוה")]
        High,
        [Display(Name = "בינוני")]
        Medium,
        [Display(Name = "נמוך")]
        Low
    }
}
