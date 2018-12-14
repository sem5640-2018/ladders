using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositories
{
    public class LadderRepository : ILaddersRepository
    {
        private readonly LaddersContext _context;

        public LadderRepository(LaddersContext context)
        {
            _context = context;
        }
        
        public async Task<LadderModel> FindByIdAsync(int id)
        {
            return await _context.LadderModel.FindAsync(id);
        }

        public async Task<LadderModel> GetByIdAsync(int id)
        {
            return await _context.LadderModel
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<LadderModel> GetByIdIncAllAsync(int id)
        {
            return  await _context.LadderModel
                .Include(m => m.ApprovalUsersList)
                .Include(o => o.CurrentRankings)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<LadderModel> GetByIdIncAllAndUserRankAsync(int id)
        {
            return  await _context.LadderModel
                .Include(ladder => ladder.ApprovalUsersList)
                .Include(ladder => ladder.CurrentRankings)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<LadderModel> GetByIdIncApprovedAsync(int id)
        {
            return await _context.LadderModel
                .Include(m => m.ApprovalUsersList)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Ranking>> GetRankingsByLadderId(int id)
        {
            var ladder = await _context.LadderModel
                .Include(m => m.CurrentRankings)
                .FirstOrDefaultAsync(m => m.Id == id);
            return ladder.CurrentRankings.ToList();
        }

        public async Task<List<LadderModel>> GetAllAsync()
        {
            return await _context.LadderModel.ToListAsync();
        }

        public async Task<LadderModel> AddAsync(LadderModel ladder)
        {
            _context.LadderModel.Add(ladder);
            await _context.SaveChangesAsync();
            return ladder;
        }

        public async Task<LadderModel> UpdateAsync(LadderModel ladder)
        {
            _context.LadderModel.Update(ladder);
            await _context.SaveChangesAsync();
            return ladder;
        }

        public async Task<LadderModel> DeleteAsync(LadderModel ladder)
        {
            _context.LadderModel.Remove(ladder);
            await _context.SaveChangesAsync();
            return ladder;
        }

        public bool Exists(int id)
        {
            return _context.LadderModel.Any(l => l.Id == id);
        }
    }
}