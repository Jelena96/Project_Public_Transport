using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Persistence.Repository;

namespace WebApp.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        
        IVehicleRepository Vehicles { get; set; }
        ILineRepository Lines { get; set; }
        IStationRepository Stations { get; set; }
        IScheduleRepository Schedules { get; set; }
        IPriceListRepository PriceLists { get; set; }
        IPassengerTypeRepository PassengerTypes { get; set; }
        ITicketTypeRepository TicketTypes { get; set; }
        IPassengerRepository Passengers { get; set; }
        IScheduleTypesRepository ScheduleTypes { get; set; }
        ITicketsRepository Tickets { get; set; }
        IPayPalRepository PayPals { get; set; }


        int Complete();
    }
}
