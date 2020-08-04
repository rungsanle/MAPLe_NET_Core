using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_menu")]
    public class M_Menu : Base_Related_Field
    {
        [Display(Name = "Menu Name")]
        public string nameOption { get; set; }

        [Display(Name = "Controller")]
        public string controller { get; set; }

        [Display(Name = "Action")]
        public string action { get; set; }

        [Display(Name = "Menu Image")]
        public string imageClass { get; set; }

        [Display(Name = "Status")]
        public bool status { get; set; }

        [Display(Name = "Parent")]
        public bool isParent { get; set; }

        [Display(Name = "parentId")]
        public int? parentId { get; set; }

        [NotMapped]
        [Display(Name = "Parent Name")]
        public string parentName { get; set; }

        [Display(Name = "Area")]
        public string area { get; set; }

        [Display(Name = "Seq")]
        public int menuseq { get; set; }

    }
}
