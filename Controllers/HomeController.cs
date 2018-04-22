using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using website.Data;
using website.Models;
using Microsoft.EntityFrameworkCore;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
             IEnumerable<IdeaModel> model = _context.Idea.Include(m => m.Donations).ToList();
            List<IdeaViewModel> vml = new List<IdeaViewModel>();
            foreach (var item in model)
            {
                IdeaViewModel vm = new IdeaViewModel();
                vm.ProductName = item.ProductName;
                vm.ProductContent = item.ProductContent;
                vm.ImagePath = item.ImagePath;
                vm.IdeaId = item.IdeaId;
                vm.FundGoal = item.FundGoal;
                vm.Donations = item.Donations;
                vml.Add(vm);
            }
            return View(vml);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description pag.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
