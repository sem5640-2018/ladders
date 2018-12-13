using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;

namespace ladders.Repositorys
{
    public class BookingRepository : IBookingRepository
    {
        public Task<Booking> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Booking> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Booking>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Booking> AddAsync(Booking booking)
        {
            throw new System.NotImplementedException();
        }

        public Task<Booking> UpdateAsync(Booking booking)
        {
            throw new System.NotImplementedException();
        }

        public Task<Booking> DeleteAsync(Booking booking)
        {
            throw new System.NotImplementedException();
        }
    }
}