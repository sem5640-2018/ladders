using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ladders.Controllers
{
    [Authorize]
    public class LaddersController : Controller
    {
        private readonly LaddersContext _context;

        public LaddersController(LaddersContext context)
        {
            _context = context;
        }

        #region User Interaction

        // GET: Ladders
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = AmIAdmin();
            return View(await _context.LadderModel.ToListAsync());
        }

        // GET: Ladders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _context.LadderModel.Include(ladder => ladder.MemberList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        // GET: Ladders/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _context.LadderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        [ActionName("Join")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join_Post(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _context.LadderModel
                .Include(ladder => ladder.ApprovalUsersList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null) return NotFound();

            var me = await GetMe();
            if (me == null) return RedirectToPage("Profile", "Create");

            if (IsMember(me, ladderModel))
                return RedirectToAction(nameof(Details), new {id});

            if (ladderModel.ApprovalUsersList == null)
                ladderModel.ApprovalUsersList = new List<ProfileModel>();

            ladderModel.ApprovalUsersList.Add(me);

            _context.Update(ladderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id});
        }

        // GET: Ladders/Create
        public IActionResult Create()
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            var ladderModel = new LadderModel
            {
                MemberList = new List<ProfileModel>(),
                CurrentRankings = new List<Ranking>(),
                ApprovalUsersList = new List<ProfileModel>()
            };


            return View(ladderModel);
        }


        // POST: Ladders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MemberList,CurrentRankings,ApprovalUsersList")]
            LadderModel ladderModel)
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            if (!ModelState.IsValid) return View(ladderModel);

            _context.Add(ladderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Ladders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();

            var ladderModel = await _context.LadderModel.FindAsync(id);
            if (ladderModel == null) return NotFound();
            return View(ladderModel);
        }

        // POST: Ladders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MemberList,CurrentRankings,ApprovalUsersList")]
            LadderModel ladderModel)
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            if (id != ladderModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(ladderModel);

            try
            {
                _context.Update(ladderModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LadderModelExists(ladderModel.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Ladders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            if (id == null) return NotFound();

            var ladderModel = await _context.LadderModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        // POST: Ladders/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AmIAdmin()) return RedirectToAction(nameof(Index));

            var ladderModel = await _context.LadderModel.FindAsync(id);
            _context.LadderModel.Remove(ladderModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool LadderModelExists(int id)
        {
            return _context.LadderModel.Any(e => e.Id == id);
        }

        #endregion

        #region Help Functions

        private async Task<bool> AmIMember(LadderModel ladder)
        {
            return IsMember(await GetMe(), ladder);
        }

        private string GetMyName()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }


        private bool AmIAdmin()
        {
            var usersGroup = User.Claims.Where(c => c.Type == "user_type");
            return usersGroup.Select(claim => claim.Value)
                .Any(value => value.Equals("administrator") || value.Equals("coordinator"));
        }

        private async Task<ProfileModel> GetMe()
        {
            var name = GetMyName();
            return await _context.ProfileModel.FirstOrDefaultAsync(e => e.UserId == name);
        }

        private static bool IsMember(ProfileModel user, LadderModel ladder)
        {
            return ladder.MemberList?.Contains(user) ?? false;
        }

        #endregion
    }
}