using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;
using Microsoft.AspNetCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ladders.Repositorys
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

        public async Task<List<Challenge>> GetAllAsync()
        {
            return await _context.Challenge.ToListAsync();
        }

        public async Task<Challenge> AddAsync(Challenge challenge)
        {
            _context.Challenge.Add(challenge);
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

        public bool IsUserInChallenge(ProfileModel user)
        {
            return _context.Challenge.Any(c => c.ChallengeeId == user.Id || c.ChallengerId == user.Id);
        }
    }
}