using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Maple2.AdminLTE.Uil.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            ViewData["ShortDateFormat"] = _configuration["AppSettings:ShortDateFormat"];
            ViewData["DefaultFirstPage"] = _configuration["AppSettings:DefaultFirstPage"];
            ViewData["TableDisplayLength"] = _configuration["AppSettings:TableDisplayLength"];
            ViewData["ToastrSuccessTimeout"] = _configuration["AppSettings:ToastrSuccessTimeout"];
            ViewData["ToastrErrorTimeout"] = _configuration["AppSettings:ToastrErrorTimeout"];
            ViewData["ToastrExtenTimeout"] = _configuration["AppSettings:ToastrExtenTimeout"];
        }
    }
}
