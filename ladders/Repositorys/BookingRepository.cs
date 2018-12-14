using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositorys
{
    public class BookingRepository : IBookingRepository
    {
        private readonly LaddersContext _context;

        public BookingRepository(LaddersContext context)
        {
            _context = context;
        }
        
        public async Task<Booking> FindByIdAsync(int id)
        {
            return await _context.Booking.FindAsync(id);
        }

        public Task<Booking> GetByIdIncAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Booking.ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            _context.Booking.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> DeleteAsync(Booking booking)
        {
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}