using EventAPI.Models;

namespace EventAPI.Service.intefaces
{
    public interface IQueryService
    {
        Task<List<Event>> GetAll();

        Task<Event> GetById(int id);

        Task<Event> GetByNameAsync(string name);

    }
}
