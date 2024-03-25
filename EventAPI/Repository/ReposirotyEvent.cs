using AutoMapper;
using EventAPI.Data;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Repository
{
    public class ReposirotyEvent : IRepository
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public ReposirotyEvent(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Event.ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            List<Event> all = await _context.Event.ToListAsync();

            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Id == id) return all[i];
            }

            return null;
        }

        public async Task<Event> GetByNameAsync(string name)
        {
            List<Event> all = await _context.Event.ToListAsync();

            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].Name.Equals(name))
                {
                    return all[i];
                }
            }

            return null;
        }

    }
}
