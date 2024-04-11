using EventAPI.Constants;
using EventAPI.Controllers;
using EventAPI.Controllers.interfaces;
using EventAPI.Dto;
using EventAPI.Exceptions;
using EventAPI.Models;
using EventAPI.Service.intefaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Events.Helpers;

namespace Teste.Events.UnitTeste
{
    public class TestController
    {

        private readonly Mock<ICommandService> _mockCommandService;
        private readonly Mock<IQueryService> _mockQueryService;
        private readonly ControllerAPI eventsApiController;

        public TestController()
        {
            _mockCommandService = new Mock<ICommandService>();
            _mockQueryService = new Mock<IQueryService>();

            eventsApiController = new ControllerEvent(_mockQueryService.Object, _mockCommandService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAll()).ThrowsAsync(new ItemsDoNotExists(Constants.ItemsDoNotExist));

            var restult = await eventsApiController.GetAll();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var eventss = TestEventFactory.CreateEvents(5);
            _mockQueryService.Setup(repo => repo.GetAll()).ReturnsAsync(eventss);

            var result = await eventsApiController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var alleventss = Assert.IsType<List<Event>>(okResult.Value);

            Assert.Equal(4, alleventss.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Create_InvalidLocation()
        {

            var createRequest = new CreateRequest
            {
                Date = DateTime.Parse("01-01-2021"),
                Location = "",
                Name = "test"
            };

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequest>())).
                ThrowsAsync(new InvalidLocation(Constants.InvalidLocation));

            var result = await eventsApiController.CreateEvent(createRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(Constants.InvalidLocation, badRequest.Value);

        }

        [Fact]
        public async Task Create_ValidData()
        {
            var createRequest = new CreateRequest
            {

                Date = DateTime.Parse("01-01-2021"),
                Location = "test",
                Name = "test"
            };

            var events = TestEventFactory.CreateEvent(1);
            events.Location = createRequest.Location;
            events.Name = createRequest.Name;

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequest>())).ReturnsAsync(events);

            var result = await eventsApiController.CreateEvent(createRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, events);

        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var update = new UpdateRequest
            {
               Location = "test"
            };

            _mockCommandService.Setup(repo => repo.Update(1, update)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await eventsApiController.UpdateEvent(1, update);

            var ntFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(ntFound.StatusCode, 404);
            Assert.Equal(Constants.ItemDoesNotExist, ntFound.Value);

        }
        [Fact]
        public async Task Update_ValidData()
        {
            var update = new UpdateRequest
            {
                Location = "test"
            };

            var events = TestEventFactory.CreateEvent(1);

            _mockCommandService.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequest>())).ReturnsAsync(events);

            var result = await eventsApiController.UpdateEvent(1, update);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, events);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mockCommandService.Setup(repo => repo.Delete(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await eventsApiController.DeleteEvent(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(notFound.StatusCode, 404);
            Assert.Equal(notFound.Value, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {

            var events = TestEventFactory.CreateEvent(1);

            _mockCommandService.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(events);

            var result = await eventsApiController.DeleteEvent(1);

            var okresult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okresult.StatusCode);
            Assert.Equal(okresult.Value, events);

        }
    }
}
