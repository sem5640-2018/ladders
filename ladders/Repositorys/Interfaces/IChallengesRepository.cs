using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys.Interfaces
{
    public interface IChallengesRepository
    {
        Task<Challenge> FindByIdAsync(int id);
        
        Task<Challenge> GetByIdIncAsync(int id);
        
        Task<List<Challenge>> GetAllAsync();

        Task<Challenge> AddAsync(Challenge challenge);

        Task<Challenge> UpdateAsync(Challenge challenge);

        Task<Challenge> DeleteAsync(Challenge challenge);
    }
}