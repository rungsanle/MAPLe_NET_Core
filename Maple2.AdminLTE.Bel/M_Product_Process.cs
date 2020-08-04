using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_product_process")]
    public class M_Product_Process : Base_Related_Field
    {
        public int ProductId { get; set; }
        public int ProcessId { get; set; }

        [Display(Name = "Seq")]
        public int? ProcessSeq { get; set; }

        [NotMapped]
        [Display(Name = "Process Code")]
        public string ProcessCode { get; set; }

        [NotMapped]
        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }
    }
}
