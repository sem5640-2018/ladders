using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public interface IRankingRepository
    {
        Task<Ranking> FindByIdAsync(int id);
        
        Task<Ranking> GetByIdIncAsync(int id);
        
        Task<List<Ranking>> GetAllAsync();

        Task<Ranking> AddAsync(Ranking ranking);

        Task<Ranking> UpdateAsync(Ranking ranking);

        Task<Ranking> DeleteAsync(Ranking ranking);
    }
}