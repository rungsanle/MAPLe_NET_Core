using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Areas.Transaction.Models
{
    public class AdvSearchArrivalModel
    {
        [Display(Name = "Arrival #")]
        [MaxLength(30)]
        public string ArrivalNo { get; set; }

        [Display(Name = "Document #")]
        public string DocRefNo { get; set; }

        [Display(Name = "Arrival Type")]
        public int? ArrivalTypeId { get; set; }

        [Display(Name = "Raw MAT. Type")]
        public int? RawMatTypeId { get; set; }

        [Display(Name = "Arrival Date From")]
        public DateTime? ArrivalDateF { get; set; }

        [Display(Name = "Arrival Date To")]
        public DateTime? ArrivalDateT { get; set; }

        [Display(Name = "Ref. Date From")]
        public DateTime? DocRefDateF { get; set; }

        [Display(Name = "Ref. Date To")]
        public DateTime? DocRefDateT { get; set; }
    }
}
