using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys.Interfaces
{
    public interface ILaddersRepository
    {
        Task<LadderModel> FindByIdAsync(int id);
        
        Task<LadderModel> GetByIdAsync(int id);
        
        Task<LadderModel> GetByIdIncAllAsync(int id);
        
        Task<LadderModel> GetByIdIncAllAndUserRankAsync(int id);
        
        Task<LadderModel> GetByIdIncApprovedAsync(int id);
        
        Task<List<LadderModel>> GetAllAsync();

        Task<LadderModel> AddAsync(LadderModel ladder);

        Task<LadderModel> UpdateAsync(LadderModel ladder);

        Task<LadderModel> DeleteAsync(LadderModel ladder);

        bool Exists(int id);
    }
}