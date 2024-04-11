using EventAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.Events.Helpers
{
    public class TestEventFactory
    {

        public static Event CreateEvent(int id)
        {
            return new Event
            {
                Id = id,
                Date = DateTime.Parse("01-01-2021"),
                Location = "test",
                Name = "test" + id

            };
        }

        public static List<Event> CreateEvents(int count)
        {

            List<Event> list = new List<Event>();
            for (int i = 1; i < count; i++)
            {
                list.Add(CreateEvent(i));
            }

            return list;
        }
    }
}
