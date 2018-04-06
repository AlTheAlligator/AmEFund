using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using website.Data;
using website.Models;

namespace website.Controllers
{
    public class IdeaController : Controller
    {
        private readonly Context _context;

        public IdeaController(Context context)
        {
            _context = context;
        }

        // GET: Idea
        public async Task<IActionResult> Index()
        {
            return View(await _context.Idea.Include(m => m.Donations).ToListAsync());
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

            return View(ideaModel);
        }

        // GET: Idea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Idea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdeaId,ProductName,FundGoal,ImagePath,ProductContent")] IdeaModel ideaModel)
        {
            if (ModelState.IsValid)
            {
                ideaModel.Donations = new List<DonationModel>();
                _context.Add(ideaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ideaModel);
        }

        // GET: Idea/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            return View(ideaModel);
        }

        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(int? id)
        {
            IdeaModel ideaModel = _context.Idea.Include(m => m.Donations).First(m => m.IdeaId == id);
            ideaModel.Donations.Add(new DonationModel{ Amount = 5 });
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
                return View(nameof(Details), ideaModel);
            }
            return View(nameof(Details), ideaModel);
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
