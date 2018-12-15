using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositories.Interfaces;
using ladders.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ladders.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly IConfigurationSection _appConfig;
        private readonly IProfileRepository _profileRepository;
        private readonly ILaddersRepository _laddersRepository;
        private readonly IChallengesRepository _challengesRepository;

        public ProfileController(IApiClient client, IConfiguration config, 
            IProfileRepository profileRepository, ILaddersRepository laddersRepository,
            IChallengesRepository challengesRepository)
        {
            _apiClient = client;
            _appConfig = config.GetSection("ladders");
            _profileRepository = profileRepository;
            _laddersRepository = laddersRepository;
            _challengesRepository = challengesRepository;
        }

        #region User Requests

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = Helpers.AmIAdmin(User);
            ViewBag.ID = Helpers.GetMyName(User);
            ViewBag.HaveAccount = _profileRepository.Exists(ViewBag.ID);
            

            return View(await _profileRepository.GetAllAsync());
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var profileModel = await _profileRepository.GetByUserIdAsync((int) id);
            if (profileModel == null) return NotFound();
            
            ViewBag.Rankings = null;
            ViewBag.OuststandingChallenges = null;
            ViewBag.LastFiveMatch = null;
            if (profileModel.CurrentRanking?.LadderModelId == null) return View(profileModel);
            
            ViewBag.Rankings =
                await _laddersRepository.GetRankingsByLadderId((int) profileModel.CurrentRanking.LadderModelId);

            ViewBag.OuststandingChallenges = _challengesRepository.GetOutstanding(profileModel.Id);
            
            ViewBag.LastFiveMatch = _challengesRepository.GetResolved(profileModel.Id).Take(5);

            return View(profileModel);
        }

        // GET: Profile/Create
        public async Task<IActionResult> Create()
        {
            var profileModel = new ProfileModel();
            if (Helpers.DoIHaveAnAccount(User, _profileRepository) && !Helpers.AmIAdmin(User))
                return await RedirectToMyProfile();

            profileModel.UserId = Helpers.GetMyName(User);
            profileModel.Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            profileModel.CurrentRanking = null;
            profileModel.ApprovalLadder = null;
            profileModel.Suspended = false;
            return View(profileModel);
        }

        // POST: Profile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Suspended,Availability,PreferredLocation,CurrentRankingId,ApprovalLadderId")]
            ProfileModel profileModel)
        {
            if (!ModelState.IsValid) return View(profileModel);

            await _profileRepository.AddAsync(profileModel);
            return RedirectToAction(nameof(Index));
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var profileModel = await _profileRepository.FindByIdAsync((int) id);
            if (profileModel == null) return NotFound();

            if (!Helpers.AmIAdmin(User) && !profileModel.UserId.Equals(Helpers.GetMyName(User))) return NotFound();

            return View(profileModel);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,UserId,Availability,PreferredLocation,Suspended,CurrentLadder")]
            ProfileModel profileModel)
        {
            if (id != profileModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(profileModel);

            if (!Helpers.AmIAdmin(User) && !profileModel.UserId.Equals(Helpers.GetMyName(User))) return NotFound();

            var activeChallenge = _challengesRepository.GetActiveUserChallenge(profileModel);
            if (activeChallenge != null)
            {
                activeChallenge = await _challengesRepository.UserConcedeChallenge(profileModel, _apiClient, _appConfig.GetValue<string>("BookingFacilitiesUrl"), activeChallenge);
                await _laddersRepository.UpdateLadder(activeChallenge);
            }

            try
            {
                await _profileRepository.UpdateAsync(profileModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileModelExists(profileModel.Id)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            if (id == null) return NotFound();

            var profileModel = await _profileRepository.GetByIdAsync((int) id);
            if (profileModel == null) return NotFound();

            return View(profileModel);
        }

        // POST: Profile/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Helpers.AmIAdmin(User)) return Unauthorized();

            var profileModel = await _profileRepository.FindByIdAsync(id);
            await _profileRepository.DeleteAsync(profileModel);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helper Functions

        private bool ProfileModelExists(int id)
        {
            return _profileRepository.Exists(id);
        }

        private async Task<IActionResult> RedirectToMyProfile()
        {
            var userId = Helpers.GetMyName(User);
            var account = await _profileRepository.GetByUserIdAsync(userId);
            return account == null
                ? RedirectToAction("Index", "LaddersMan")
                : RedirectToAction("Details", new {id = account.Id});
        }

        #endregion
    }
}