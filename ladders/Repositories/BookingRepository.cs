using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ladders.Models;
using ladders.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ladders.Repositories
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
            throw new NotImplementedException();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Booking.ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Booking.Update(booking);
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