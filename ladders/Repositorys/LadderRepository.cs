using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public class LadderRepository : ILaddersRepository
    {
        public Task<LadderModel> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<LadderModel> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<LadderModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<LadderModel> AddAsync(LadderModel ladder)
        {
            throw new System.NotImplementedException();
        }

        public Task<LadderModel> UpdateAsync(LadderModel ladder)
        {
            throw new System.NotImplementedException();
        }

        public Task<LadderModel> DeleteAsync(LadderModel ladder)
        {
            throw new System.NotImplementedException();
        }
    }
}