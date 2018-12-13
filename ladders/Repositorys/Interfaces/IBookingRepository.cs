using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;

namespace ladders.Repositorys
{
    public interface IBookingRepository
    {
        Task<Booking> FindByIdAsync(int id);
        
        Task<Booking> GetByIdIncAsync(int id);
        
        Task<List<Booking>> GetAllAsync();

        Task<Booking> AddAsync(Booking booking);

        Task<Booking> UpdateAsync(Booking booking);

        Task<Booking> DeleteAsync(Booking booking);
    }
}