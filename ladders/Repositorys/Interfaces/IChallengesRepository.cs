using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys.Interfaces
{
    public interface IChallengesRepository
    {
        Task<Challenge> FindByIdAsync(int id);

        Task<Challenge> GetFullChallenge(int id);

        Task<Challenge> GetByIdIncDirectDecentAsync(int id);

        Task<Challenge> GetByIdExclUserInfAsync(int id);

        List<Challenge> GetByChallenger(ProfileModel challenger);
        
        List<Challenge> GetByChallengee(ProfileModel challengee);
        
        Task<List<Challenge>> GetAllAsync();

        Task<Challenge> AddAsync(Challenge challenge);

        Task<Challenge> UpdateAsync(Challenge challenge);

        Task<Challenge> DeleteAsync(Challenge challenge);

        bool Exists(int id);
    }
}