using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface ITicketDAO : IBasicDAO<Ticket>, IDAOTest
    {
        List<Ticket> GetTicketsByCustomer(Customer customer);
        List<Ticket> GetTicketsByFlight(Flight flight);
        List<Ticket> GetTicketsByAirline(long airlineID);
        void MoveTicketsToHistory();
        bool CheckTicketUniqueFieldsExistance(Ticket ticket);

    }
}
