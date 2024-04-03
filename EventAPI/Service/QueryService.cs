using EventAPI.Exceptions;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using EventAPI.Service.intefaces;

namespace EventAPI.Service
{
    public class QueryService : IQueryService
    {


        private IRepository _repository;

        public QueryService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Event>> GetAll()
        {
            var events = await _repository.GetAllAsync();

            if (events.Count() == 0)
            {
                throw new ItemsDoNotExists(Constants.Constants.ItemsDoNotExist);
            }

            return (List<Event>)events;
        }

        public async Task<Event> GetByNameAsync(string name)
        {
            var events = await _repository.GetByNameAsync(name);

            if (events == null)
            {
                throw new ItemDoesNotExist(Constants.Constants.ItemDoesNotExist);
            }

            return events;
        }

        public async Task<Event> GetById(int id)
        {
            var events = await _repository.GetByIdAsync(id);

            if (events == null)
            {
                throw new ItemDoesNotExist(Constants.Constants.ItemDoesNotExist);
            }

            return events;
        }

    }
}
