using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ladders.Models;

namespace ladders.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly LaddersContext _context;

        public ProfilesController(LaddersContext context)
        {
            _context = context;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProfileModel.ToListAsync());
        }

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileModel = await _context.ProfileModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileModel == null)
            {
                return NotFound();
            }

            return View(profileModel);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Availability,PreferredLocation,Suspended")] ProfileModel profileModel)
        {
            if (!ModelState.IsValid) return View(profileModel);
            _context.Add(profileModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileModel = await _context.ProfileModel.FindAsync(id);
            if (profileModel == null)
            {
                return NotFound();
            }
            return View(profileModel);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Availability,PreferredLocation,Suspended")] ProfileModel profileModel)
        {
            if (id != profileModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileModelExists(profileModel.Id))
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
            return View(profileModel);
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileModel = await _context.ProfileModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileModel == null)
            {
                return NotFound();
            }

            return View(profileModel);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profileModel = await _context.ProfileModel.FindAsync(id);
            _context.ProfileModel.Remove(profileModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileModelExists(int id)
        {
            return _context.ProfileModel.Any(e => e.Id == id);
        }
    }
}
