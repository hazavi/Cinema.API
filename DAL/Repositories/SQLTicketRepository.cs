using DAL.Models.Domain;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //Repository fungerer som en hjælper, der håndterer al kommunikation med databasen

    public class SQLTicketRepository : ITicketRepository
    {
        private readonly MyDbContext _context;

        // Constructor to inject the DbContext
        public SQLTicketRepository(MyDbContext context)
        {
            _context = context;
        }

        // Create a new Ticket
        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            _context.Tickets.Add(ticket);  // Adds the ticket to the Tickets DbSet
            await _context.SaveChangesAsync();  // Saves changes to the database
            return ticket;  // Return the created ticket
        }

        // Get all Tickets
        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                                 .Include(t => t.User)       // Include related User data
                                 .Include(t => t.Showtime)   // Include related Showtime data
                                 .Include(t => t.Seat)       // Include related Seat data
                                 .ToListAsync();  // Retrieve all tickets from the database
        }

        // Get a Ticket by ID
        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                                 .Include(t => t.User)
                                 .Include(t => t.Showtime)
                                 .Include(t => t.Seat)
                                 .FirstOrDefaultAsync(t => t.TicketId == id);  // Find ticket by TicketId
        }

        // Update an existing Ticket
        public async Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            var existingTicket = await _context.Tickets
                                               .FirstOrDefaultAsync(t => t.TicketId == id);  // Find the ticket to update

            if (existingTicket == null)
                return null;  // Ticket not found

            // Update properties
            existingTicket.UserId = ticket.UserId;
            existingTicket.ShowtimeId = ticket.ShowtimeId;
            existingTicket.SeatId = ticket.SeatId;
            existingTicket.PurchaseDate = ticket.PurchaseDate;
            existingTicket.Price = ticket.Price;
            existingTicket.Quantity = ticket.Quantity;

            await _context.SaveChangesAsync();  // Save changes to the database
            return existingTicket;  // Return the updated ticket
        }

        // Delete a Ticket by ID
        public async Task<Ticket?> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets
                                       .FirstOrDefaultAsync(t => t.TicketId == id);  // Find the ticket to delete

            if (ticket == null)
                return null;  // Ticket not found

            _context.Tickets.Remove(ticket);  // Remove ticket from the DbSet
            await _context.SaveChangesAsync();  // Save changes to the database
            return ticket;  // Return the deleted ticket
        }
    }
}
