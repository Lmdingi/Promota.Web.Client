using Services.Interfaces;
using Services.Models;
using Services.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EventService : IEventService
    {
        private readonly IAPIService _aPIService;

        public EventService(IAPIService aPIService)
        {
            _aPIService = aPIService;
        }

        public async Task DeleteEventByIdAsync(string id)
        {
             await _aPIService.DeleteAsync($"Events/{id}");
        }

        public async Task<Event> GetEventByIdAsync(string id)
        {
            var response = await _aPIService.GetAsync<Event>($"Events/{id}");

            return response;
        }

        public async Task<List<Event>> GetEventsByUserIdAsync(string userId)
        {
            var response = await _aPIService.GetAsync<List<Event>>($"Events/user/{userId}");

            return response;
        }

        public async Task<string> UpdateEventAsync(Event eventModel)
        {
            var response = await _aPIService.PutAsync<string>($"Events", eventModel);
            return response;
        }

        async Task<string> IEventService.CreatEventAsync(Event eventModel)
        {
            var response = await _aPIService.PostAsync<string>("Events", eventModel);

            return response;
        }

        async Task<List<Event>> IEventService.GetEventsAsync()
        {
            var response = await _aPIService.GetAsync<List<Event>>("Events");

            return response;
        }
    }
}
