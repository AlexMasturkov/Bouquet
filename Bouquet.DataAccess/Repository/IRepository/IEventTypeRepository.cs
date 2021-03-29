using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bouquet.DataAccess.Repository.IRepository
{
   public interface IEventTypeRepository:IRepositoryAsync<EventType>
    {
        void Update(EventType eventType);
        //Task<EventType> GetAsync(int v);
    }
}
