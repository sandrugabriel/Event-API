using EventAPI.Dto;
using EventAPI.Models;

namespace EventAPI.Service.intefaces
{
    public interface ICommandService
    {
        Task<Event> Create(CreateRequest request);

        Task<Event> Update(int id, UpdateRequest request);

        Task<Event> Delete(int id);
    }
}
