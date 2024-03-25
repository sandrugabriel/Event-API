using EventAPI.Models;
using System;

namespace EventAPI.Repository.interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();

        Task<Event> GetByNameAsync(string name);

        Task<Event> GetByIdAsync(int id);
    }
}
