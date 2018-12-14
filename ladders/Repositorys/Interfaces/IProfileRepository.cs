using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys.Interfaces
{
    public interface IProfileRepository
    {
        Task<ProfileModel> FindByIdAsync(int id);
        
        Task<ProfileModel> GetByIdAsync(int id);
        
        Task<ProfileModel> GetByUserIdAsync(string userId);
        
        Task<ProfileModel> GetByUserIdIncAsync(string name);
        
        Task<List<ProfileModel>> GetAllAsync();

        Task<ProfileModel> AddAsync(ProfileModel profile);

        Task<ProfileModel> UpdateAsync(ProfileModel profile);

        Task<ProfileModel[]> UpdateRangeAsync(ProfileModel[] profiles);

        Task<ProfileModel> DeleteAsync(ProfileModel profile);

        bool Exists(int id);

        bool Exists(string userId);
    }
}