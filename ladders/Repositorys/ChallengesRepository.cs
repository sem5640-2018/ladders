using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public class ChallengesRepository : IChallengesRepository
    {
        public Task<Challenge> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Challenge> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Challenge>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Challenge> AddAsync(Challenge challenge)
        {
            throw new System.NotImplementedException();
        }

        public Task<Challenge> UpdateAsync(Challenge challenge)
        {
            throw new System.NotImplementedException();
        }

        public Task<Challenge> DeleteAsync(Challenge challenge)
        {
            throw new System.NotImplementedException();
        }
    }
}