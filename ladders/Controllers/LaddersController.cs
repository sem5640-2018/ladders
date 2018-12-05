using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ladders.Models;
using Microsoft.AspNetCore.Authorization;

namespace ladders.Controllers
{
    public class LaddersController : Controller
    {
        private readonly LaddersContext _context;

        public LaddersController(LaddersContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Ladders
        public async Task<IActionResult> Index()
        {
            return View(await _context.LadderModel.ToListAsync());
        }

        // GET: Ladders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladderModel = await _context.LadderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null)
            {
                return NotFound();
            }

            return View(ladderModel);
        }

        // GET: Ladders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ladders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] LadderModel ladderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ladderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ladderModel);
        }

        // GET: Ladders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladderModel = await _context.LadderModel.FindAsync(id);
            if (ladderModel == null)
            {
                return NotFound();
            }
            return View(ladderModel);
        }

        // POST: Ladders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] LadderModel ladderModel)
        {
            if (id != ladderModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ladderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LadderModelExists(ladderModel.Id))
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
            return View(ladderModel);
        }

        // GET: Ladders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ladderModel = await _context.LadderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null)
            {
                return NotFound();
            }

            return View(ladderModel);
        }

        // POST: Ladders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ladderModel = await _context.LadderModel.FindAsync(id);
            _context.LadderModel.Remove(ladderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LadderModelExists(int id)
        {
            return _context.LadderModel.Any(e => e.Id == id);
        }
    }
}
