using EventAPI.Constants;
using EventAPI.Dto;
using EventAPI.Exceptions;
using EventAPI.Models;
using EventAPI.Repository.interfaces;
using EventAPI.Service;
using EventAPI.Service.intefaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Events.Helpers;

namespace Teste.Events.UnitTeste
{
    public class TestCommandService
    {
        private readonly Mock<IRepository> _mock;
        private readonly ICommandService _commandService;

        public TestCommandService()
        {
            _mock = new Mock<IRepository>();
            _commandService = new CommandService(_mock.Object);
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

            _mock.Setup(repo => repo.Create(createRequest)).ReturnsAsync((Event)null);
            var exception = await Assert.ThrowsAsync<InvalidLocation>(() => _commandService.Create(createRequest));

            Assert.Equal(Constants.InvalidLocation, exception.Message);
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

            var events = TestEventFactory.CreateEvent(50);
            events.Location = createRequest.Location;

            _mock.Setup(repo => repo.Create(It.IsAny<CreateRequest>())).ReturnsAsync(events);

            var result = await _commandService.Create(createRequest);

            Assert.NotNull(result);
            Assert.Equal(result.Location, createRequest.Location);
        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateRequest
            {
                Location = "test"
            };

            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Event)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.Update(50, updateRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task Update_InvalidLocation()
        {
            var updateRequest = new UpdateRequest
            {
                Location = ""
            };
            var events = TestEventFactory.CreateEvent(50);
            events.Location = updateRequest.Location;
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync(events);

            var exception = await Assert.ThrowsAsync<InvalidLocation>(() => _commandService.Update(50, updateRequest));

            Assert.Equal(Constants.InvalidLocation, exception.Message);
        }

        [Fact]
        public async Task Update_ValidData()
        {
            var updateREquest = new UpdateRequest
            {
                Location = "test"
            };

            var events = TestEventFactory.CreateEvent(1);
            events.Location = updateREquest.Location;

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(events);
            _mock.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequest>())).ReturnsAsync(events);

            var result = await _commandService.Update(1, updateREquest);

            Assert.NotNull(result);
            Assert.Equal(events, result);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.DeleteById(It.IsAny<int>())).ReturnsAsync((Event)null);

            var exception = await Assert.ThrowsAnyAsync<ItemDoesNotExist>(() => _commandService.Delete(1));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {
            Event events = TestEventFactory.CreateEvent(50);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(events);

            var restul = await _commandService.Delete(50);

            Assert.NotNull(restul);
            Assert.Equal(events, restul);
        }

    }
}
