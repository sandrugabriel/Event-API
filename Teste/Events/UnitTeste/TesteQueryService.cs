using EventAPI.Constants;
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
    public class TesteQueryService
    {
        private readonly Mock<IRepository> _mock;
        private readonly IQueryService _service;

        public TesteQueryService()
        {
            _mock = new Mock<IRepository>();
            _service = new QueryService(_mock.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Event>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExists>(() => _service.GetAll());

            Assert.Equal(exception.Message, Constants.ItemsDoNotExist);
        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var eventss = TestEventFactory.CreateEvents(5);

            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(eventss);

            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(eventss, result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Event)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.GetById(1));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);
        }

        [Fact]
        public async Task GetById_ValidData()
        {
            var events = TestEventFactory.CreateEvent(1);
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(events);

            var result = await _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(events, result);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("")).ReturnsAsync((Event)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _service.GetByNameAsync(""));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetByName_ValidData()
        {
            var events = TestEventFactory.CreateEvent(10);
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync(events);
            var result = await _service.GetByNameAsync("test");

            Assert.NotNull(result);
            Assert.Equal(events, result);

        }

    }
}
