using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEventService
    {
        Task<string> CreatEventAsync(Event eventModel);
        Task<List<Event>> GetEventsAsync();
        Task<List<Event>> GetEventsByUserIdAsync(string userId);
    }
}
