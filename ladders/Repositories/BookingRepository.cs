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
            await SafeAddExtras(booking);

            await _context.Booking.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        private async Task SafeAddExtras(Booking booking)
        {
            var currentSport = await _context.Sport.FirstOrDefaultAsync(s => s.sportId == booking.facility.sportId);
            if (currentSport == null)
                await _context.Sport.AddAsync(booking.facility.sport);
            else
                _context.Sport.Update(booking.facility.sport);

            var currentVenue = await _context.Venue.FirstOrDefaultAsync(v => v.venueId == booking.facility.venueId);
            if (currentVenue == null)
                await _context.Venue.AddAsync(booking.facility.venue);
            else
                _context.Venue.Update(booking.facility.venue);

            var currentFacility =
                await _context.Facility.FirstOrDefaultAsync(b => b.facilityId == booking.facility.facilityId);
            if (currentFacility == null)
                await _context.Facility.AddAsync(booking.facility);
            else
                _context.Facility.Update(booking.facility);
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