using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PMPCore.Controllers
{
    public class ReportsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}