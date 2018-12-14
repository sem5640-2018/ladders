using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositorys
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly LaddersContext _context;
        
        public ProfileRepository(LaddersContext context)
        {
            _context = context;
        }
        
        public async Task<ProfileModel> FindByIdAsync(int id)
        {
            return await _context.ProfileModel.FindAsync(id);
        }

        public async Task<ProfileModel> GetByNameIncAsync(string name)
        {
            return await _context.ProfileModel.Include(a => a.CurrentRanking).ThenInclude(lad => lad.LadderModel).FirstOrDefaultAsync(e => e.UserId == name);
        }

        public async Task<List<ProfileModel>> GetAllAsync()
        {
            return await _context.ProfileModel.ToListAsync();
        }

        public async Task<ProfileModel> AddAsync(ProfileModel profile)
        {
            _context.ProfileModel.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<ProfileModel> UpdateAsync(ProfileModel profile)
        {
            _context.ProfileModel.Update(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<ProfileModel[]> UpdateRangeAsync(ProfileModel[] profiles)
        {
            _context.ProfileModel.UpdateRange(profiles);
            await _context.SaveChangesAsync();
            return profiles;
        }

        public async Task<ProfileModel> DeleteAsync(ProfileModel profile)
        {
            _context.ProfileModel.Remove(profile);
            await _context.SaveChangesAsync();
            return profile;
        }
    }
}