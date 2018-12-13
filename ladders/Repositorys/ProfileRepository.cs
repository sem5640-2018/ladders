using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public class ProfileRepository : IProfileRepository
    {
        public Task<ProfileModel> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProfileModel> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProfileModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ProfileModel> AddAsync(ProfileModel profile)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProfileModel> UpdateAsync(ProfileModel profile)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProfileModel> DeleteAsync(ProfileModel profile)
        {
            throw new System.NotImplementedException();
        }
    }
}