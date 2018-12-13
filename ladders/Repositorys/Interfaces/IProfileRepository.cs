using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public interface IProfileRepository
    {
        Task<ProfileModel> FindByIdAsync(int id);
        
        Task<ProfileModel> GetByIdIncAsync(int id);
        
        Task<List<ProfileModel>> GetAllAsync();

        Task<ProfileModel> AddAsync(ProfileModel profile);

        Task<ProfileModel> UpdateAsync(ProfileModel profile);

        Task<ProfileModel> DeleteAsync(ProfileModel profile);
    }
}