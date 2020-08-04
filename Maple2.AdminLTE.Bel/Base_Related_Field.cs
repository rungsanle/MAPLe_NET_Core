using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public abstract class Base_Related_Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Active")]
        public bool Is_Active { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? Created_Date { get; set; }

        [Display(Name = "Created By")]
        public int? Created_By { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? Updated_Date { get; set; }

        [Display(Name = "Updated By")]
        public int? Updated_By { get; set; }
    }
}
