using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositories.Interfaces
{
    public interface ILaddersRepository
    {
        Task<LadderModel> FindByIdAsync(int id);
        
        Task<LadderModel> GetByIdAsync(int id);
        
        Task<LadderModel> GetByIdIncAllAsync(int id);
        
        Task<LadderModel> GetByIdIncAllAndUserRankAsync(int id);

        Task<LadderModel> GetAllForDeleteAsync(int id);

        Task<LadderModel> GetByIdIncApprovedAsync(int id);

        Task<List<Ranking>> GetRankingsByLadderId(int id);

        Task<List<LadderModel>> GetAllAsync();

        Task<LadderModel> AddAsync(LadderModel ladder);

        Task<LadderModel> UpdateAsync(LadderModel ladder);

        Task<LadderModel> DeleteAsync(LadderModel ladder, IEnumerable<Challenge> allChallenges);

        Ranking GetRankByIdAsync(LadderModel ladder, int userId);

        Task<LadderModel> UpdateLadder(Challenge challenge);

        Task<List<LadderModel>> GetAllAsyncIncludes();

        bool Exists(int id);
    }
}