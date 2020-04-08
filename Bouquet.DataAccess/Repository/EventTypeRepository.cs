using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bouquet.DataAccess.Repository
{
    public class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public EventTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(EventType eventType)
        {
            var eventTypeDb = _db.EventTypes.FirstOrDefault(c => c.Id == eventType.Id);
            if (eventTypeDb != null)
            {
                eventTypeDb.Name = eventType.Name;               
            }
        }
    }
}
