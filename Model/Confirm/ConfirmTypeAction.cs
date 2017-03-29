using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Confirm
{
    public enum ConfirmTypeAction
    {
        [Display(Name = "אשר")]
        Ok,
        [Display(Name = "דחה")]
        Decline,
        [Display(Name = "חסום")]
        Block
    }
}