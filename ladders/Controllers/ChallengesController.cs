using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ladders.Models;
using ladders.Shared;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ladders.Controllers
{
    [Authorize]
    public class ChallengesController : Controller
    {
        private readonly LaddersContext _context;
        private readonly IApiClient _apiClient;

        public ChallengesController(LaddersContext context, IApiClient client)
        {
            _context = context;
            _apiClient = client;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            var me = await Helpers.GetMe(User, _context);
            ViewBag.Me = me;
            ViewBag.IsAdmin = Helpers.AmIAdmin(User);
            ViewBag.Challenged = _context.Challenge.Where(c => c.Challengee == me);
            ViewBag.Challenging = _context.Challenge.Where(c => c.Challenger == me);

            return View(await _context.Challenge.Where(c => c.Challenger == me || c.Challengee == me).ToListAsync());
        }

        // GET: Challenges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var me = await Helpers.GetMe(User, _context);
            var isAdmin = Helpers.AmIAdmin(User);
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .Include(c => c.Booking)
                .ThenInclude(b => b.facility)
                .ThenInclude(f => f.sport)
                .Include(c => c.Booking)
                .ThenInclude(b => b.facility)
                .ThenInclude(f => f.venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (challenge == null || !await IsValid(challenge))
            {
                return NotFound();
            }

            return View(challenge);
        }

        // GET: Challenges/Create
        public async Task<IActionResult> Create(int? userId, int? ladderId)
        {
            if (userId == null || ladderId == null)
            {
                return NotFound();
            }

            var challengee = await _context.ProfileModel.FindAsync(userId);
            var ladder = await _context.LadderModel.FindAsync(ladderId);

            if (challengee == null || ladder == null)
            {
                return NotFound();
            }

            var challenge = new Challenge
            {
                ChallengedTime = DateTime.UtcNow,
                Created = DateTime.UtcNow,
                Resolved = false,
                Challenger = await Helpers.GetMe(User, _context),
                Challengee = challengee,
                Ladder = ladder
            };

            var venueData = await _apiClient.GetAsync("https://docker2.aberfitness.biz/booking-facilities/api/venues");
            var sportData = await _apiClient.GetAsync("https://docker2.aberfitness.biz/booking-facilities/api/sports");
            if (!venueData.IsSuccessStatusCode || !sportData.IsSuccessStatusCode) return View(challenge);

            var info = await venueData.Content.ReadAsStringAsync();
            var facilities = JsonConvert.DeserializeObject<ICollection<Venue>>(info);

            info = await sportData.Content.ReadAsStringAsync();
            var sports = JsonConvert.DeserializeObject<ICollection<Sport>>(info);

            ViewBag.VenueId = new SelectList(facilities, "venueId", "venueName");
            ViewBag.SportId = new SelectList(sports, "sportId", "sportName");

            return View(challenge);
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChallengedTime,Challenger,Challengee,Ladder,Result")] Challenge challenge, [Bind("VenueId")] int VenueId, [Bind("SportId")] int SportId)
        {
            //if (!ModelState.IsValid) return View(challenge);

            challenge.ChallengeeId = challenge.Challengee.Id;
            challenge.ChallengerId = challenge.Challenger.Id;

            var user = await Helpers.GetMe(User, _context);

            challenge.Challenger = null;
            challenge.Challengee = null;

            if (challenge.Ladder == null)
                return View(challenge);

            var booking = await MakeBooking(VenueId, SportId, challenge.ChallengedTime, user.UserId);

            if (booking == null)
                return View(challenge);

            challenge.Created = DateTime.UtcNow;
            _context.Add(booking);
            challenge.Booking = booking;

            await Helpers.EmailUser(_apiClient, user.UserId, "Test", "email");

            await _context.AddAsync(challenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {challenge.Id});
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge.FindAsync(id);
            if (challenge == null || !await IsValid(challenge))
            {
                return NotFound();
            }
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingId,ChallengedTime,Resolved,Result")] Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && !await IsValid(challenge))
            {
                try
                {
                    _context.Update(challenge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeExists(challenge.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .FirstOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await _context.Challenge.FindAsync(id);
            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenge.Any(e => e.Id == id);
        }

        public async Task<Booking> MakeBooking(int venueId, int sportId, DateTime time, string userId)
        {
            var res = await _apiClient.PostAsync($"https://docker2.aberfitness.biz/booking-facilities/api/booking/{venueId}/{sportId}", new { bookingDateTime = time, userId = Helpers.GetMyName(User)});
            if (!res.IsSuccessStatusCode)
                return null;

            var data = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Booking>(data); ;
        }

        public async Task<bool> IsValid(Challenge challenge)
        {
            var me = await Helpers.GetMe(User, _context);
            var isAdmin = Helpers.AmIAdmin(User);
            return (challenge.Challenger == me || challenge.Challengee == me || !isAdmin);
        }
    }
}
