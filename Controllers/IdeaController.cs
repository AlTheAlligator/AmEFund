using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using website.Data;
using website.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ImageSharp;

namespace website.Controllers
{
    public class IdeaController : Controller
    {
        private readonly Context _context;
        private IHostingEnvironment _env;

        public IdeaController(Context context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
        
        // GET: Idea
        public async Task<IActionResult> Index()
        {
            IEnumerable<IdeaModel> model = await _context.Idea.Include(m => m.Donations).ToListAsync();
            
            return View(model);
        }

        // GET: Idea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ideaModel = await _context.Idea.Include(m => m.Donations)
                .SingleOrDefaultAsync(m => m.IdeaId == id);
            if (ideaModel == null)
            {
                return NotFound();
            }
           
            DonationViewModel vm = new DonationViewModel();
            vm.ProductName = ideaModel.ProductName;
            vm.ProductContent = ideaModel.ProductContent;
            vm.ImagePath = ideaModel.ImagePath;
            vm.IdeaId = ideaModel.IdeaId;
            vm.FundGoal = ideaModel.FundGoal;
            vm.Donations = ideaModel.Donations;

            return View(vm);
        }
        [Authorize]
        // GET: Idea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Idea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdeaId,ProductName,FundGoal,ImagePath,ProductContent")] IdeaModel ideaModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];

                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(_env.WebRootPath + "/Images", fileName);
                        using (FileStream output = System.IO.File.Create(path))
                        {
                            
                            await file.CopyToAsync(output);
                        }
                        using (Image<Rgba32> image = Image.Load(path))
                        {
                            image.Resize(1280,720);
                            image.Save(path); // Automatic encoder selected based on extension.
                        }
                        ideaModel.Donations = new List<DonationModel>();
                        ideaModel.ImagePath = "/Images/"+fileName;
                        _context.Add(ideaModel);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }
            return View(ideaModel);
        }

        // GET: Idea/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ideaModel = await _context.Idea.SingleOrDefaultAsync(m => m.IdeaId == id);
            if (ideaModel == null)
            {
                return NotFound();
            }
            return View(ideaModel);
        }

        // POST: Idea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdeaId,ProductName,FundGoal,ImagePath,ProductContent")] IdeaModel ideaModel)
        {
            if (id != ideaModel.IdeaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ideaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaModelExists(ideaModel.IdeaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(ideaModel);
        }

        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(int id, [Bind("IdeaId,DonationAmount")]DonationViewModel donationViewModel)
        {
            IdeaModel ideaModel = _context.Idea.Include(m => m.Donations).First(m => m.IdeaId == donationViewModel.IdeaId);
            DonationModel dm = new DonationModel();
            dm.Amount = donationViewModel.DonationAmount;
            dm.IdeaId = donationViewModel.IdeaId;
            ideaModel.Donations.Add(dm);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ideaModel);
                    _context.Update(ideaModel.Donations.Last());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaModelExists(ideaModel.IdeaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Idea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ideaModel = await _context.Idea
                .SingleOrDefaultAsync(m => m.IdeaId == id);
            if (ideaModel == null)
            {
                return NotFound();
            }

            return View(ideaModel);
        }

        // POST: Idea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ideaModel = await _context.Idea.SingleOrDefaultAsync(m => m.IdeaId == id);
            _context.Idea.Remove(ideaModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaModelExists(int id)
        {
            return _context.Idea.Any(e => e.IdeaId == id);
        }
    }
}
