using EventAPI.Dto;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventAPI.Controllers
{
    [ApiController]
    [Route("api/v1/event")]
    public class ControllerEvent : ControllerBase
    {

        private readonly ILogger<ControllerEvent> _logger;

        private IRepository _repository;

        public ControllerEvent(ILogger<ControllerEvent> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var events = await _repository.GetAllAsync();
            return Ok(events);
        }


        [HttpGet("/findById")]
        public async Task<ActionResult<Event>> GetById([FromQuery] int id)
        {
            var events = await _repository.GetByIdAsync(id);
            return Ok(events);
        }


        [HttpGet("/find/{name}")]
        public async Task<ActionResult<Event>> GetByNameRoute([FromRoute] string name)
        {
            var events = await _repository.GetByNameAsync(name);
            return Ok(events);
        }


        [HttpPost("/create")]
        public async Task<ActionResult<Event>> Create([FromBody] CreateRequest request)
        {
            var events = await _repository.Create(request);
            return Ok(events);

        }

        [HttpPut("/update")]
        public async Task<ActionResult<Event>> Update([FromQuery] int id, [FromBody] UpdateRequest request)
        {
            var events = await _repository.Update(id, request);
            return Ok(events);
        }

        [HttpDelete("/deleteById")]
        public async Task<ActionResult<Event>> DeleteCarById([FromQuery] int id)
        {
            var events = await _repository.DeleteById(id);
            return Ok(events);
        }


    }
}
