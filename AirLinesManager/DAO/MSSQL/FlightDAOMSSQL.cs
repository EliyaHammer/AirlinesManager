using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
   public class FlightDAOMSSQL : BasicDAO<Flight>, IFlightDAO, IDAOTest
    {
        private SqlConnection Connection;
        private AirlineDAOMSSQL airlineDAO = new AirlineDAOMSSQL();

        public FlightDAOMSSQL() { }

        public Flight Add(Flight toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Flight provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("ADD_FLIGHT", Connection))
                {
                    command.Parameters.Add(new SqlParameter("airline_id", toAdd.AirlineCompanyID));
                    command.Parameters.Add(new SqlParameter("departure_time", toAdd.DepartureTime));
                    command.Parameters.Add(new SqlParameter("destination_country_code", toAdd.DestinationCountryCode));
                    command.Parameters.Add(new SqlParameter("flight_status", toAdd.FlightStatus));
                    command.Parameters.Add(new SqlParameter("landing_time", toAdd.LandingTime));
                    command.Parameters.Add(new SqlParameter("origin_country_code", toAdd.OriginCountryCode));
                    command.Parameters.Add(new SqlParameter("remaining_tickets", toAdd.RemainingTickets));
                    command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt64(resultString);
                    return toAdd;
                }
            }

        }

        public Flight Get(long id)
        {
            Flight result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHT_BY_ID", Connection))
                {
                    command.Parameters.Add(new SqlParameter("flight_id", id));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            };
                        }
                    }
                }
            }

            return result;
        }

        public List<Flight> GetAll()
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ALL_FLIGHTS", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();

                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                           result.Add(new Flight()
                           {
                              ID = (long)reader["ID"],
                              AirlineCompanyID = (long)reader["Airline_Company_ID"],
                              DepartureTime = (DateTime)reader["Departure_Time"],
                              DestinationCountryCode = (long)reader["Destination_Country_Code"],
                              FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                              LandingTime = (DateTime)reader["Landing_Time"],
                              OriginCountryCode = (long)reader["Origin_Country_Code"],
                              RemainingTickets = (int)reader["Remaining_Tickets"]
                           });
                        }
                        
                    }
                }
            }

            return result;
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ALL_FLIGHTS", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new Dictionary<Flight, int>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]

                        }, (int)reader["Remaining_Tickets"]);

                        while (reader.Read())
                        {
                          result.Add(new Flight()
                          {
                             ID = (long)reader["ID"],
                             AirlineCompanyID = (long)reader["Airline_Company_ID"],
                             DepartureTime = (DateTime)reader["Departure_Time"],
                             DestinationCountryCode = (long)reader["Destination_Country_Code"],
                             FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                             OriginCountryCode = (long)reader["Origin_Country_Code"],
                             RemainingTickets = (int)reader["Remaining_Tickets"]

                          }, (int)reader["Remaining_Tickets"]);
                        }
                        
                    }
                }
            }

            return result;
        }

        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            if (customer is null)
                throw new NullReferenceException("Customer provided is empty.");

            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_CUSTOMER", Connection))
                {
                    command.Parameters.Add(new SqlParameter("customer_id", customer.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();

                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                               ID = (long)reader["ID"],
                               AirlineCompanyID = (long)reader["Airline_Company_ID"],
                               DepartureTime = (DateTime)reader["Departure_Time"],
                               DestinationCountryCode = (long)reader["Destination_Country_Code"],
                               FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                               LandingTime = (DateTime)reader["Landing_Time"],
                               OriginCountryCode = (long)reader["Origin_Country_Code"],
                               RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }
            return result;
        }

        public IList<Flight> GetFlightsByDeprtureDateAndTime(DateTime date)
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_DEPARTURE_DATE_AND_TIME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("departure_time", date));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                               ID = (long)reader["ID"],
                               AirlineCompanyID = (long)reader["Airline_Company_ID"],
                               DepartureTime = (DateTime)reader["Departure_Time"],
                               DestinationCountryCode = (long)reader["Destination_Country_Code"],
                               FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                               LandingTime = (DateTime)reader["Landing_Time"],
                               OriginCountryCode = (long)reader["Origin_Country_Code"],
                               RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }

                return result;
            }
        }

        //add one with time > add to all needeed facade... etc.
        public IList<Flight> GetFlightsByDeprtureDate(DateTime date)
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_DEPARTURE_DATE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("departure_time", date));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }

                return result;
            }
        }

        public IList<Flight> GetFlightsByDestinationCountry(long countryID) 
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_DESTINATION_COUNTRY", Connection))
                {
                    command.Parameters.Add(new SqlParameter("destination_country_code", countryID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<Flight> GetFlightsByLandingDateAndTime(DateTime date)
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_LANDING_DATE_AND_TIME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("landing_time", date));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }

            return result;
        }
        // add one with time > add to all needeed facade... etc.

        public IList<Flight> GetFlightsByLandingDate(DateTime date)
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_LANDING_DATE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("landing_time", date));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<Flight> GetFlightsByOriginCountry(long countryID)
        {
            List<Flight> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_ORIGIN_COUNTRY", Connection))
                {
                    command.Parameters.Add(new SqlParameter("origin_country_code", countryID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();
                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }

                    }
                }
            }

            return result;
        }

        //Also removes the tickets assigned to this flight.
        public void Remove(Flight toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Flight provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("REMOVE_FLIGHT", Connection))
                {
                    command.Parameters.Add(new SqlParameter("flight_id", toRemove.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }

        }

        public void Update(Flight toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Country provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE_FLIGHT", Connection))
                {
                    command.Parameters.Add(new SqlParameter("airline_company_id", toUpdate.AirlineCompanyID));
                    command.Parameters.Add(new SqlParameter("departure_time", toUpdate.DepartureTime));
                    command.Parameters.Add(new SqlParameter("destination_country_code", toUpdate.DestinationCountryCode));
                    command.Parameters.Add(new SqlParameter("flight_status", toUpdate.FlightStatus));
                    command.Parameters.Add(new SqlParameter("landing_time", toUpdate.LandingTime));
                    command.Parameters.Add(new SqlParameter("origin_country_code", toUpdate.OriginCountryCode));
                    command.Parameters.Add(new SqlParameter("remaining_tickets", toUpdate.RemainingTickets));
                    command.Parameters.Add(new SqlParameter("flight_id", toUpdate.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
            

        }

        //Checks wheather or not the flight exist in the DB (before removing, updating).
        public bool CheckExistance (Flight flightToCheck)
        {
            if (flightToCheck is null)
                return false;
            if (flightToCheck.ID <= 0)
                return false;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHT_BY_ID", Connection))
                {
                    command.Parameters.Add(new SqlParameter("flight_id", flightToCheck.ID));
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

        public IList<Flight> GetFlightsByAirline(long airlineID)
        {
            List<Flight> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_FLIGHTS_BY_AIRLINE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("airline_id", airlineID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Flight>();

                        result.Add(new Flight()
                        {
                            ID = (long)reader["ID"],
                            AirlineCompanyID = (long)reader["Airline_Company_ID"],
                            DepartureTime = (DateTime)reader["Departure_Time"],
                            DestinationCountryCode = (long)reader["Destination_Country_Code"],
                            FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                            LandingTime = (DateTime)reader["Landing_Time"],
                            OriginCountryCode = (long)reader["Origin_Country_Code"],
                            RemainingTickets = (int)reader["Remaining_Tickets"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Flight()
                            {
                                ID = (long)reader["ID"],
                                AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                DepartureTime = (DateTime)reader["Departure_Time"],
                                DestinationCountryCode = (long)reader["Destination_Country_Code"],
                                FlightStatus = (FlightStatus)Convert.ToInt32(reader["Flight_Status"]),
                                LandingTime = (DateTime)reader["Landing_Time"],
                                OriginCountryCode = (long)reader["Origin_Country_Code"],
                                RemainingTickets = (int)reader["Remaining_Tickets"]
                            });
                        }
                    }
                }
            }

            return result;
        }

        //Moves the flights which has landed 3 hours ago to the FlightHistory table.
        public void MoveFlightsToHistory ()
        {
            List<Flight> allFlights = GetAll();
            if (allFlights.Count > 0)
            {
                List<Flight> flightToMove = new List<Flight>();

                //saves the flights that landed 3 hours ago
                for (int i = 0; i < allFlights.Count; i++)
                {
                    if (allFlights[i].LandingTime.Year <= DateTime.Now.Year)
                    {
                        if (allFlights[i].LandingTime.Date <= DateTime.Now.Date)
                        {
                            if (allFlights[i].LandingTime.Hour + 3 == DateTime.Now.Hour)
                                flightToMove.Add(allFlights[i]);
                        }
                    }
                }

                //a loop that inserts the flights to the procedure
                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();

                    using (SqlCommand command = new SqlCommand("MOVE_FLIGHT_TO_HISTORY", Connection))
                    {

                        for (int i = 0; i < flightToMove.Count; i++)
                        {
                            command.Parameters.Add(new SqlParameter("flight_id", flightToMove[i].ID));
                            command.CommandType = CommandType.StoredProcedure;
                            SqlDataReader reader = command.ExecuteReader();
                        }

                    }
                }
            }
        }

        public List<Flight> GetFlightHistory()
         {
             List<Flight> flightHistory;

             using (Connection = new SqlConnection(_connectionString))
             {
                 Connection.Open();

                 using (SqlCommand command = new SqlCommand("GET_FLIGHTS_HISTORY", Connection))
                 {
                     command.CommandType = CommandType.StoredProcedure;

                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                          if (!reader.Read())
                             throw new FlightNotFoundException("No flights found in the History.");
                          else
                          {
                              flightHistory = new List<Flight>();
                              flightHistory.Add(new Flight()
                              {
                                 ID = (long)reader["ID"],
                                 AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                 DepartureTime = (DateTime)reader["Departure_Time"],
                                 DestinationCountryCode = (long)reader["Destunation_Country_Code"],
                                 FlightStatus = (FlightStatus)reader["Flight_Status"],
                                 LandingTime = (DateTime)reader["Landing_Time"],
                                 OriginCountryCode = (long)reader["Origin_Country_Code"],
                                 RemainingTickets = (int)reader["Remaining_Tickets"]
                              });

                              while (reader.Read())
                              {
                                  flightHistory.Add(new Flight()
                                  {
                                      ID = (long)reader["ID"],
                                      AirlineCompanyID = (long)reader["Airline_Company_ID"],
                                      DepartureTime = (DateTime)reader["Departure_Time"],
                                      DestinationCountryCode = (long)reader["Destunation_Country_Code"],
                                      FlightStatus = (FlightStatus)reader["Flight_Status"],
                                      LandingTime = (DateTime)reader["Landing_Time"],
                                      OriginCountryCode = (long)reader["Origin_Country_Code"],
                                      RemainingTickets = (int)reader["Remaining_Tickets"]
                                  });
                              }
                          }
                     }
                 }
             }

           return flightHistory;
         }

        //Removes all the flights from the Replica DB > will not work on the actual DB. 
        public void RemoveAllReplica()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_FLIGHTS", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Close();
                    }

                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_FLIGHTSHISTORY", Connection))
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

