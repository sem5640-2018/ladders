using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;
using ladders.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ladders.Controllers
{
    [Authorize]
    public class LaddersController : Controller
    {
        private readonly ILaddersRepository _laddersRepository;
        private readonly IChallengesRepository _challengesRepository;
        private readonly IProfileRepository _profileRepository;

        public LaddersController(ILaddersRepository laddersRepository, IChallengesRepository challengesRepository,
            IProfileRepository profileRepository)
        {
            _laddersRepository = laddersRepository;
            _challengesRepository = challengesRepository;
            _profileRepository = profileRepository;
        }

        #region User Interaction

        // GET: Ladders
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = Helpers.AmIAdmin(User);
            ViewBag.User = await _profileRepository.GetByUserIdIncAsync(Helpers.GetMyName(User));
            return View(await _laddersRepository.GetAllAsync());
        }

        // GET: Ladders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdIncAllAndUserRankAsync((int) id);

            if (ladderModel == null) return NotFound();
            ViewBag.IsAdmin = Helpers.AmIAdmin(User);
            ViewBag.Me = await _profileRepository.GetByUserIdIncAsync(Helpers.GetMyName(User));
            ViewBag.challenges = _challengesRepository.GetByLadder(ladderModel);

            return View(ladderModel);
        }

        // GET: Ladders/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdAsync((int) id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        [ActionName("Join")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join_Post(int? id)
        {
            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdIncAllAndUserRankAsync((int) id);
            if (ladderModel == null) return NotFound();

            var me = await _profileRepository.GetByUserIdIncAsync(Helpers.GetMyName(User));
            if (me == null) return RedirectToAction("Create", "Profile");

            if (IsMember(me, ladderModel))
                return RedirectToAction(nameof(Details), new {id});

            if (ladderModel.ApprovalUsersList == null)
                ladderModel.ApprovalUsersList = new List<ProfileModel>();
            ladderModel.ApprovalUsersList.Add(me);
            me.ApprovalLadder = ladderModel;

            await _profileRepository.UpdateAsync(me);
            await _laddersRepository.UpdateAsync(ladderModel);
            
            return RedirectToAction(nameof(Details), new {id});
        }

        // GET: Ladders/Approval/5
        public async Task<IActionResult> Approval(int? id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdIncApprovedAsync((int) id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        [ActionName("Approval")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Ladders/Approval/5
        public async Task<IActionResult> Approval(int? id, string userId, bool add)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null || userId == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdIncAllAsync((int) id);
            var user = await _profileRepository.GetByUserIdIncAsync(userId);

            if (ladderModel == null || user == null) return NotFound();

            if (!ladderModel.ApprovalUsersList.Contains(user))
                return View(ladderModel);

            ladderModel.ApprovalUsersList.Remove(user);
            user.ApprovalLadder = null;

            if (add)
            {
                var newRanking = new Ranking
                {
                    User = user,
                    Challenges = new List<Challenge>(),
                    LadderModel = ladderModel,
                    Wins = 0,
                    Draws = 0,
                    Losses = 0,
                    Position = ladderModel.CurrentRankings.Count
                };

                user.CurrentRanking = newRanking;
                ladderModel.CurrentRankings.Add(newRanking);
            }

            await _profileRepository.UpdateAsync(user);
            await _laddersRepository.UpdateAsync(ladderModel);

            return View(ladderModel);
        }


        // GET: Ladders/Create
        public IActionResult Create()
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            var ladderModel = new LadderModel
            {
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
            if (!Helpers.AmIAdmin(User)) return Unauthorized();
            if (!ModelState.IsValid) return View(ladderModel);

            await _laddersRepository.AddAsync(ladderModel);
            return RedirectToAction(nameof(Index));
        }

        // GET: Ladders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdIncAllAsync((int) id);

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
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id != ladderModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(ladderModel);

            try
            {
                await _laddersRepository.UpdateAsync(ladderModel);
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
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null) return NotFound();

            var ladderModel = await _laddersRepository.GetByIdAsync((int) id);
            if (ladderModel == null) return NotFound();

            return View(ladderModel);
        }

        // POST: Ladders/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            var ladderModel = await _laddersRepository.FindByIdAsync(id);
            await _laddersRepository.DeleteAsync(ladderModel);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveUser(int? id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null) return NotFound();

            var user = await _profileRepository.FindByIdAsync((int) id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Ladders/Delete/5
        [HttpPost]
        [ActionName("RemoveUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserConfirmed(int id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            var user = await _profileRepository.FindByIdAsync(id);
            if (user.CurrentRanking == null)
                return RedirectToAction(nameof(Index));

            var ladderModel = user.CurrentRanking.LadderModel;

            if (ladderModel == null)
                return NotFound();

            var rank = ladderModel.CurrentRankings.FirstOrDefault(a => a.User == user);

            if (rank == null)
                return NotFound();

            var allLower = ladderModel.CurrentRankings.Where(a => a.Position > rank.Position);
            foreach (var ranking in allLower)
            {
                ranking.Position--;
            }

            ladderModel.CurrentRankings.Remove(rank);
            user.CurrentRanking = null;

            await _profileRepository.UpdateAsync(user);
            await _laddersRepository.UpdateAsync(ladderModel);

            return RedirectToAction(nameof(Index));
        }

        private bool LadderModelExists(int id)
        {
            return _laddersRepository.Exists(id);
        }

        #endregion

        #region Help Functions

        private async Task<bool> AmIMember(LadderModel ladder) //TODO is it needed (remove comment if it is)
        {
            ProfileModel user = await _profileRepository.GetByUserIdAsync(Helpers.GetMyName(User));
            return IsMember(user, ladder);
        }


        private static bool IsMember(ProfileModel user, LadderModel ladder)
        {
            return ladder.CurrentRankings?.FirstOrDefault(a => a.User == user) != null;
        }

        #endregion
    }
}