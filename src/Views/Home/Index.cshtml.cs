using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Maple2.AdminLTE.Uil.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
            ViewData["DefaultFirstPage"] = _config["AppSettings:DefaultFirstPage"];
            ViewData["TableDisplayLength"] = _config["AppSettings:TableDisplayLength"];
            ViewData["ToastrSuccessTimeout"] = _config["AppSettings:ToastrSuccessTimeout"];
            ViewData["ToastrErrorTimeout"] = _config["AppSettings:ToastrErrorTimeout"];
            ViewData["ToastrExtenTimeout"] = _config["AppSettings:ToastrExtenTimeout"];
        }
    }
}