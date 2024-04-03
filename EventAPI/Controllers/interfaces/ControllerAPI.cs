using EventAPI.Dto;
using EventAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers.interfaces
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class ControllerAPI : ControllerBase
    {


        [HttpGet("/all")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Event>))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<List<Event>>> GetAll();

        [HttpGet("/findById")]
        [ProducesResponseType(statusCode: 200, type: typeof(Event))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<Event>> GetById([FromQuery]int id);

        [HttpGet("/findByName")]
        [ProducesResponseType(statusCode: 200, type: typeof(Event))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<Event>> GetByName([FromQuery] string name);

        [HttpPost("/createEvent")]
        [ProducesResponseType(statusCode: 201, type: typeof(Event))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<Event>> CreateEvent(CreateRequest request);

        [HttpPut("/updateEvent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Event))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        [ProducesResponseType(statusCode: 404, type: typeof(String))]
        public abstract Task<ActionResult<Event>> UpdateEvent([FromQuery] int id, UpdateRequest request);

        [HttpDelete("/deleteEvent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Event))]
        [ProducesResponseType(statusCode: 404, type: typeof(String))]
        public abstract Task<ActionResult<Event>> DeleteEvent([FromQuery] int id);


    }
}
