using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositories.Interfaces;
using ladders.Shared;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositories
{
    public class ChallengesRepository : IChallengesRepository
    {
        private readonly LaddersContext _context;

        public ChallengesRepository(LaddersContext context)
        {
            _context = context;
        }

        public async Task<Challenge> FindByIdAsync(int id)
        {
            return await _context.Challenge.FindAsync(id);
        }

        public async Task<Challenge> GetFullChallenge(int id)
        {
            return await _context
                .Challenge
                .Include(c => c.Challenger)
                .ThenInclude(u => u.CurrentRanking)
                .Include(c => c.Challengee)
                .ThenInclude(u => u.CurrentRanking)
                .Include(c => c.Booking)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Challenge> GetByIdIncDirectDecentAsync(int id)
        {
            return await _context.Challenge
                .Include(c => c.Challenger)
                .Include(c => c.Challengee)
                .Include(c => c.Booking)
                .Include(c => c.Ladder)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Challenge> GetByIdExclUserInfAsync(int id)
        {
            return await _context.Challenge
                .Include(c => c.Booking)
                .ThenInclude(b => b.facility)
                .ThenInclude(f => f.sport)
                .Include(c => c.Booking)
                .ThenInclude(b => b.facility)
                .ThenInclude(f => f.venue)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public List<Challenge> GetByChallenger(ProfileModel challenger)
        {
            return _context.Challenge.Where(c => c.Challenger == challenger).ToList();
        }

        public List<Challenge> GetByChallengee(ProfileModel challengee)
        {
            return _context.Challenge.Where(c => c.Challengee == challengee).ToList();
        }

        public List<Challenge> GetByLadder(LadderModel ladder)
        {
            return _context.Challenge.Where(a => a.Ladder == ladder).ToList();
        }

        public List<Challenge> GetByLadderActive(LadderModel ladder)
        {
            return _context.Challenge
                .Include(c => c.Challenger)
                .Include(c => c.Challengee)
                .Where(a => a.Ladder == ladder && !a.Resolved).ToList();
        }

        public List<Challenge> GetOutstanding(int userId)
        {
            return _context.Challenge
                .Where(a => !a.Resolved)
                .Where(a => a.ChallengeeId == userId || a.ChallengerId == userId)
                .ToList();
        }
        
        public List<Challenge> GetResolved(int userId)
        {
            return _context.Challenge
                .Where(a => a.Resolved)
                .Where(a => a.ChallengeeId == userId || a.ChallengerId == userId)
                .OrderByDescending(a => a.ChallengedTime)
                .ToList();
        }

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenge.ToListAsync();
        }

        public async Task<Challenge> AddAsync(Challenge challenge)
        {
            await _context.Challenge.AddAsync(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> UpdateAsync(Challenge challenge)
        {
            _context.Challenge.Update(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public async Task<Challenge> DeleteAsync(Challenge challenge)
        {
            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();
            return challenge;
        }

        public bool Exists(int id)
        {
            return _context.Challenge.Any(c => c.Id == id);
        }

        public bool IsUserInActiveChallenge(ProfileModel user)
        {
            return GetActiveUserChallenge(user) != null;
        }

        public Challenge GetActiveUserChallenge(ProfileModel user)
        {
            bool Check(Challenge challenge)
            {
                if (challenge.ChallengeeId != user.Id && challenge.ChallengerId != user.Id)
                    return false;

                return !challenge.Resolved;
            }

            return _context.Challenge.Include(a => a.Challenger).Include(a => a.Challengee).Include(a => a.Ladder).FirstOrDefault(Check);
        }

        public async Task<Challenge> UserConcedeChallenge(ProfileModel user, IApiClient apiClient, string bookingUri,
            Challenge challenge)
        {
            var isChallenger = challenge.Challenger == user;
            var winner = isChallenger ? Winner.Challengee : Winner.Challenger;

            return await UpdateWinner(winner, apiClient, bookingUri, challenge);
        }

        public async Task<Challenge> UpdateWinner(Winner winner, IApiClient apiClient, string bookingUri,
            Challenge challenge)
        {
            if (challenge.ChallengedTime < DateTime.UtcNow)
            {
                await Helpers.FreeUpVenue(bookingUri, apiClient, challenge.Booking.bookingId);
            }

            challenge.Result = winner;
            challenge.Resolved = true;
            challenge.Accepted = true;

            _context.Challenge.Update(challenge);
            await _context.SaveChangesAsync();

            return challenge;
        }

        public bool IsChallengeStale(Challenge challenge)
        {
            if (challenge.Resolved)
            {
                return false;
            }

            var now = DateTime.UtcNow;

            if (challenge.Accepted)
            {
                return challenge.ChallengedTime < now.AddDays(-7);
            }

            return challenge.Created < now.AddDays(-3) || challenge.ChallengedTime < now;
        }
    }
}