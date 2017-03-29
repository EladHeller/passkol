using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaskolProd.Models
{
    public class MusicReportViewModel
    {
        public MusicReportType Type { get; set; }
        public int Count { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public enum MusicReportType
    {
        [Display(Name ="כל היצירות")]
        All,
        [Display(Name ="נצפו")]
        Show,
        [Display(Name ="נמכרו")]
        Purchase
    }
}