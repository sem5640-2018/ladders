using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositorys
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

        public Task<LadderModel> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException(); //TODO Implement
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
    }
}