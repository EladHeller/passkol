using Model.Confirm;
using System.Collections.Generic;

namespace PaskolProd.Models
{
    public class ConfirmationViewModel
    {
        public IEnumerable<Confirmation> Confirmations { get; set; }
        public ConfirmType ConfirmType { get; set; }
    }
}