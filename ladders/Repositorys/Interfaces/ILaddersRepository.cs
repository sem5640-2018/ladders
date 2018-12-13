using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public interface ILaddersRepository
    {
        Task<LadderModel> FindByIdAsync(int id);
        
        Task<LadderModel> GetByIdIncAsync(int id);
        
        Task<List<LadderModel>> GetAllAsync();

        Task<LadderModel> AddAsync(LadderModel ladder);

        Task<LadderModel> UpdateAsync(LadderModel ladder);

        Task<LadderModel> DeleteAsync(LadderModel ladder);
    }
}