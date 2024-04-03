using EventAPI.Controllers.interfaces;
using EventAPI.Dto;
using EventAPI.Exceptions;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using EventAPI.Service.intefaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventAPI.Controllers
{
    public class ControllerEvent : ControllerAPI
    {


        private IQueryService _queryService;
        private ICommandService _commandService;

        public ControllerEvent(IQueryService queryService, ICommandService commandService)
        {
            _queryService = queryService;
            _commandService = commandService;
        }

        public override async Task<ActionResult<List<Event>>> GetAll()
        {
            try
            {
                var eventss = await _queryService.GetAll();

                return Ok(eventss);

            }
            catch (ItemsDoNotExists ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Event>> GetByName([FromQuery]string name)
        {

            try
            {
                var events = await _queryService.GetByNameAsync(name);
                return Ok(events);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<Event>> GetById([FromQuery] int id)
        {

            try
            {
                var events = await _queryService.GetById(id);
                return Ok(events);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<Event>> CreateEvent(CreateRequest request)
        {
            try
            {
                var events = await _commandService.Create(request);
                return Ok(events);
            }
            catch (InvalidLocation ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Event>> UpdateEvent([FromQuery]int id, UpdateRequest request)
        {
            try
            {
                var events = await _commandService.Update(id, request);
                return Ok(events);
            }
            catch (InvalidLocation ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Event>> DeleteEvent([FromQuery] int id)
        {
            try
            {
                var events = await _commandService.Delete(id);
                return Ok(events);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }



    }
}
