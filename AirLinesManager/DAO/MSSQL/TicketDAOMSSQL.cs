using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class TicketDAOMSSQL : BasicDAO<Ticket>, ITicketDAO
    {
        private SqlConnection Connection;
        
        public TicketDAOMSSQL () { }

        //Also descends from the remaining tickets property of the flight.
        public Ticket Add(Ticket toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Ticket providen is empty or does not exist.");
            
                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();

                    using (SqlCommand command = new SqlCommand("ADD_TICKET", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("flight_id", toAdd.FlightID));
                        command.Parameters.Add(new SqlParameter("customer_id", toAdd.CustomerID));
                        command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt64(resultString);
                    return toAdd;
                }
                }
        }

        public Ticket Get(long id)
        {
            if (id <= 0)
                throw new IllegalValueException($"ID provided: '{id}' is invalid; less or equal to zero.");

                Ticket ticketToReturn;
                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_TICKET_BY_ID", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("ticket_id", id));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;
                        else
                            {
                                ticketToReturn = new Ticket()
                                {
                                    ID = (long)reader["ID"],
                                    CustomerID = (long)reader["Customer_ID"],
                                    FlightID = (long)reader["Flight_ID"]
                                };
                            }
                        }
                    }
                return ticketToReturn;
            }
        }//not used

        public List<Ticket> GetAll()
        {
            List<Ticket> tickets;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_ALL_TICKETS", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;

                            tickets = new List<Ticket>();

                            tickets.Add(new Ticket()
                            {
                                ID = (long)reader["ID"],
                                CustomerID = (long)reader["Customer_ID"],
                                FlightID = (long)reader["Flight_ID"]
                            });

                            while (reader.Read())
                            {
                                tickets.Add(new Ticket()
                                {
                                     ID = (long)reader["ID"],
                                     CustomerID = (long)reader["Customer_ID"],
                                     FlightID = (long)reader["Flight_ID"]
                                });
                            }
                            
                        }
                    }
                }
            return tickets;
        }

        public List<Ticket> GetTicketsByAirline(long airlineID)
        {
            List<Ticket> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_TICKETS_BY_AIRLINE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("airline_id", airlineID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Ticket>();
                        result.Add(new Ticket()
                        {
                            ID = (long)reader["ID"],
                            CustomerID = (long)reader["Customer_ID"],
                            FlightID = (long)reader["Flight_ID"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Ticket()
                            {
                                ID = (long)reader["ID"],
                                CustomerID = (long)reader["Customer_ID"],
                                FlightID = (long)reader["Flight_ID"]
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<Ticket> GetTicketsByCustomer(Customer customer)
        {
            if (customer is null)
                throw new NullReferenceException("Customer provided dots not exist or empty.");
            if (customer.ID <= 0)
                throw new IllegalValueException($"Customer ID: '{customer.ID}' is invalid; less or equal to zero");

            List<Ticket> tickets;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_TICKETS_BY_CUSTOMER", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("customer_id", customer.ID));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;

                            tickets = new List<Ticket>();
                            tickets.Add(new Ticket()
                            {
                                ID = (long)reader["ID"],
                                CustomerID = (long)reader["Customer_ID"],
                                FlightID = (long)reader["Flight_ID"]
                            });

                            while (reader.Read())
                            {
                                tickets.Add(new Ticket()
                                {
                                    ID = (long)reader["ID"],
                                    CustomerID = (long)reader["Customer_ID"],
                                    FlightID = (long)reader["Flight_ID"]
                                });
                            }
                            
                        }
                    }
                }
            return tickets;
        }//not used

        public List<Ticket> GetTicketsByFlight(Flight flight)//not used
        {
            if (flight is null)
                throw new NullReferenceException("Customer provided dots not exist or empty.");
            if (flight.ID <= 0)
                throw new IllegalValueException("Customer ID is not valid.");

            List<Ticket> tickets;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_TICKETS_BY_FLIGHT", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("flight_id", flight.ID));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;

                            tickets = new List<Ticket>();
                            tickets.Add(new Ticket()
                            {
                                ID = (long)reader["ID"],
                                CustomerID = (long)reader["Customer_ID"],
                                FlightID = (long)reader["Flight_ID"]
                            });

                            while (reader.Read())
                            {
                                tickets.Add(new Ticket()
                                {
                                    ID = (long)reader["ID"],
                                    CustomerID = (long)reader["Customer_ID"],
                                    FlightID = (long)reader["Flight_ID"]
                                });
                            }
                            
                        }
                    }
                }
            return tickets;
        }

        //Also ascends the remaining tickets property of the flight.
        public void Remove(Ticket toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Ticket providen does not exist or empty.");

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("REMOVE_TICKET", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("ticket_id", toRemove.ID));
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
        }

        public void Update(Ticket toUpdate)
        {
            CustomerDAOMSSQL customerDAO = new CustomerDAOMSSQL();
            FlightDAOMSSQL flightDAO = new FlightDAOMSSQL();

            if (toUpdate is null)
                throw new NullReferenceException("Ticket providen does not exist or empty.");
            if (toUpdate.ID <= 0)
                throw new IllegalValueException("The ticket ID of the providen ticket is not valid.");
            if (CheckTicketUniqueFieldsExistance(toUpdate))
                throw new AlreadyExistsException("There is already a ticket for this flight and customer.");
            if (toUpdate.CustomerID <= 0)
                throw new IllegalValueException("Customer ID in the providen ticket is not valid.");
            if (toUpdate.FlightID <= 0)
                throw new IllegalValueException("Flight ID in the providen ticket is not valid.");
            if (Get(toUpdate.ID) == null)
                throw new TicketNotFoundException("Ticket does not exist");
            if (customerDAO.Get(toUpdate.CustomerID) is null)
                throw new CustomerNotFoundException("The customer could not be found.");
            if (flightDAO.Get(toUpdate.FlightID) is null)
                throw new FlightNotFoundException("Flight could not be found.");

            using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("UPDATE_TICKET", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("ticket_id", toUpdate.ID));
                        command.Parameters.Add(new SqlParameter("flight_id", toUpdate.FlightID));
                        command.Parameters.Add(new SqlParameter("customer_id", toUpdate.CustomerID));
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
        }//not used
     
        //Checks wheather or not the unique fields of the ticket already exist in the DB before adding or updating.
        public bool CheckTicketUniqueFieldsExistance (Ticket ticket)
        {
            if (ticket is null || ticket.CustomerID <= 0 || ticket.FlightID <= 0)
                return false;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("CHECK_TICKET_UNIQUE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("flight_id", ticket.FlightID));
                    command.Parameters.Add(new SqlParameter("customer_id", ticket.CustomerID));
                    command.Parameters.Add(new SqlParameter("ticket_id", ticket.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return false;
                        else
                            return true;
                    }
                }
            }
        }

        //Adds tickets to history and removes them from regular, MUST happen after flights had been moved.
        public void MoveTicketsToHistory()
        {
            FlightDAOMSSQL flightDAO = new FlightDAOMSSQL();
            List<Flight> flightHistory = flightDAO.GetFlightHistory();

            List<Ticket> ticketsToMove = new List<Ticket>();
            //saves the tickets to move
            for (int i = 0; i < flightHistory.Count; i++)
            {
                List<Ticket> ticketsByFlight = GetTicketsByFlight(flightHistory[i]);
                for (int j = 0; j < ticketsByFlight.Count; j ++)
                {
                    ticketsToMove.Add(ticketsByFlight[i]);
                }
            }

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("MOVE_TICKET_TO_HISTORY", Connection))
                {
                    for (int i = 0; i < ticketsToMove.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("ticket_id", ticketsToMove[i].ID));
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
            }

        }

        //Removes all tickets from the Replica DB, will not work on the actual DB.
        public void RemoveAllReplica()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_TICKETS", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();
                    }

                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_TICKETSHISTORY", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();
                    }
                }
            }
        }

    }
}
