using System.Net;
using System.Text;
using EventAPI.Dto;
using EventAPI.Models;
using Newtonsoft.Json;
using Teste.Events.Helpers;
using Teste.Events.Infrastructure;

namespace Teste.Events.IntegrationTest;

public class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
{
    
        private readonly HttpClient _client;

        public IntegrationTest(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllEvents_EventsFound_ReturnsOkStatusCode_ValidResponse()
        {
            var createEventRequest = TestEventFactory.CreateEvent(1);
            var content = new StringContent(JsonConvert.SerializeObject(createEventRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/v1/ControllerEvent/createEvent", content);

            var response = await _client.GetAsync("/api/v1/ControllerEvent/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetEventById_EventFound_ReturnsOkStatusCode()
        {
            var createEvent = new CreateRequest
                { Name = "test", Date = DateTime.Today , Location = "Asdsd"};

            var content = new StringContent(JsonConvert.SerializeObject(createEvent), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/ControllerEvent/createEvent", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Event>(responseString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.Name, createEvent.Name);
        }

        [Fact]
        public async Task GetEventById_EventNotFound_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/v1/ControllerEvent/findById?id=9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_Create_ValidRequest_ReturnsCreatedStatusCode()
        {
            var request = "/api/v1/ControllerEvent/createEvent";
            
            var createEvent = new CreateRequest
                { Name = "test", Date = DateTime.Today , Location = "Asdsd"};

            var content = new StringContent(JsonConvert.SerializeObject(createEvent), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Event>(responseString);

            Assert.NotNull(result);
            Assert.Equal(createEvent.Name, result.Name);
        }

        [Fact]
        public async Task Put_Update_ValidRequest_ReturnsAcceptedStatusCode()
        {
            var request = "/api/v1/ControllerEvent/createEvent";
            var createEvent = new CreateRequest
                { Name = "test", Date = DateTime.Today , Location = "Asdsd"};

            var content = new StringContent(JsonConvert.SerializeObject(createEvent), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Event>(responseString);

            request = $"/api/v1/ControllerEvent/updateEvent?id={result.Id}";
            var updateEvent = new UpdateRequest { Location = "Asd" };
            content = new StringContent(JsonConvert.SerializeObject(updateEvent), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);
            var responceStringUp = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<Event>(responceStringUp);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result1.Location, updateEvent.Location);
        }

        [Fact]
        public async Task Put_Update_InvalidEventLocation_ReturnsBadRequestStatusCode()
        {
            var request = "/api/v1/ControllerEvent/createEvent";
            var createEvent = new CreateRequest
                { Name = "test", Date = DateTime.Today, Location = "asdasd"};

            var content = new StringContent(JsonConvert.SerializeObject(createEvent), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Event>(responseString);

            request = $"/api/v1/ControllerEvent/updateEvent?id={result.Id}";
            var updateEvent = new UpdateRequest { Location = "" };
            content = new StringContent(JsonConvert.SerializeObject(updateEvent), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);
            var responceStringUp = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<Event>(responseString);


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(result1.Location, updateEvent.Location);
        }

        [Fact]
        public async Task Put_Update_EventDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ControllerEvent/updateEvent";
            var updateEvent = new UpdateRequest { Location = "asd" };
            var content = new StringContent(JsonConvert.SerializeObject(updateEvent), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(request, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Delete_EventExists_ReturnsDeletedEvent()
        {
            var request = "/api/v1/ControllerEvent/createEvent";
            var createEvent = new CreateRequest
                { Name = "test", Date = DateTime.Today , Location = "Asdsd"};

            var content = new StringContent(JsonConvert.SerializeObject(createEvent), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Event>(responseString)!;

            request = $"/api/v1/ControllerEvent/deleteEvent?id={result.Id}";

            response = await _client.DeleteAsync(request);
            var responceString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<Event>(responseString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result1.Name, createEvent.Name);
        }

        [Fact]
        public async Task Delete_Delete_EventDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ControllerEvent/deleteEvent?id=7";

            var response = await _client.DeleteAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

}