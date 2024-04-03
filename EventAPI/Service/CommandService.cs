using EventAPI.Dto;
using EventAPI.Exceptions;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using EventAPI.Service.intefaces;

namespace EventAPI.Service
{
    public class CommandService : ICommandService
    {


        private IRepository _repository;

        public CommandService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Event> Create(CreateRequest request)
        {

            if (request.Location == "")
            {
                throw new InvalidLocation(Constants.Constants.InvalidLocation);
            }

            var events = await _repository.Create(request);

            return events;
        }

        public async Task<Event> Update(int id, UpdateRequest request)
        {

            var events = await _repository.GetByIdAsync(id);
            if (events == null)
            {
                throw new ItemDoesNotExist(Constants.Constants.ItemDoesNotExist);
            }


            if (events.Location == "")
            {
                throw new InvalidLocation(Constants.Constants.InvalidLocation);
            }
            events = await _repository.Update(id, request);
            return events;
        }

        public async Task<Event> Delete(int id)
        {

            var events = await _repository.GetByIdAsync(id);
            if (events == null)
            {
                throw new ItemDoesNotExist(Constants.Constants.ItemDoesNotExist);
            }
            events = await _repository.DeleteById(id);
            return events;
        }

    }
}
