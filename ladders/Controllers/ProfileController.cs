using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ladders.Models;
using Microsoft.AspNetCore.Authorization;

namespace ladders.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly LaddersContext _context;

        public ProfileController(LaddersContext context)
        {
            _context = context;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = AmIAdmin();
            ViewBag.ID = GetMyName();

            return View(await _context.ProfileModel.ToListAsync());
        }

        // GET: Profile/Details/5
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

        // GET: Profile/Create
        public async Task<IActionResult> Create()
        {
            if (DoIHaveAnAccount() && !AmIAdmin())
                return await RedirectToMyProfile();
            return View();
        }

        // POST: Profile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId, Suspended, Availability,PreferredLocation")] ProfileModel profileModel)
        {
            profileModel.UserId = GetMyName();
            profileModel.Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            profileModel.CurrentLadder = -1;
            profileModel.Suspended = false;

            // TODO
            //if (!ModelState.IsValid) return View(profileModel);

            _context.Add(profileModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Profile/Edit/5
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

        // POST: Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Availability,PreferredLocation,Suspended,CurrentLadder")] ProfileModel profileModel)
        {
            if (id != profileModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(profileModel);

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

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!AmIAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

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

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AmIAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var profileModel = await _context.ProfileModel.FindAsync(id);
            _context.ProfileModel.Remove(profileModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // Helper Functions

        private bool ProfileModelExists(int id)
        {
            return _context.ProfileModel.Any(e => e.Id == id);
        }

        private async Task<IActionResult> RedirectToMyProfile()
        {
            var userId = GetMyName();
            var account  = await _context.ProfileModel.FirstOrDefaultAsync(e => e.UserId == userId);
            return account == null ? RedirectToAction("Index", "Ladders") : RedirectToAction("Details", new {id = account.Id});
        }

        private string GetMyName()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }

        private bool DoIHaveAnAccount()
        {
            var userId = GetMyName();

            return _context.ProfileModel.Any(e => e.UserId == userId);
        }

        private bool AmIAdmin()
        {
            var usersGroup = User.Claims.Where(c => c.Type == "user_type");
            return usersGroup.Select(claim => claim.Value).Any(value => value.Equals("administrator") || value.Equals("coordinator"));
        }
    }
}
