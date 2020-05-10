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
   public class AirlineDAOMSSQL : BasicDAO<AirlineCompany>, IAirlineDAO, IDAOTest
    {
        private SqlConnection Connection;

        public AirlineDAOMSSQL() { }

        public AirlineCompany Add(AirlineCompany toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Airline provided is empty.");

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("ADD_AIRLINE_COMPANY", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_name", toAdd.AirLineName));
                        command.Parameters.Add(new SqlParameter("username", toAdd.UserName));
                        command.Parameters.Add(new SqlParameter("password", toAdd.Password));
                        command.Parameters.Add(new SqlParameter("country_code", toAdd.CountryCode));
                        command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt64(resultString);
                    return toAdd;

                    }
                }
        }

        public AirlineCompany Get(long id)
        {
            AirlineCompany airlineResult;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_AIRLINE_BY_ID", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_id", id));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                                return null;

                            airlineResult = new AirlineCompany()
                            {
                                ID = (long)reader["ID"],
                                AirLineName = (string)reader["Airline_Name"],
                                CountryCode = (long)reader["Country_Code"],
                                Password = (string)reader["Password"],
                                UserName = (string)reader["User_Name"]
                            };
                        }
                    }
                }
            return airlineResult;
        }

        public AirlineCompany GetAirlineByName(string name) //not used
        {
            if (name is null)
                throw new NullReferenceException("Name provided is empty.");

            AirlineCompany airlineResult;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_AIRLINE_BY_NAME", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_name", name));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;
                        else
                            {
                                airlineResult = new AirlineCompany()
                                {
                                    ID = (long)reader["ID"],
                                    AirLineName = name,
                                    CountryCode = (long)reader["Country_Code"],
                                    Password = (string)reader["Password"],
                                    UserName = (string)reader["User_Name"]
                                };
                            }
                        }
                    }
                }
            return airlineResult;
        }

        public AirlineCompany GetAirlineByUsername(string username)
        {
            if (username is null)
                throw new NullReferenceException("Username provided is empty.");

            AirlineCompany airlineResult;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_AIRLINE_BY_USER_NAME", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_username", username));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null; 

                            else
                            {
                                airlineResult = new AirlineCompany()
                                {
                                    ID = (long)reader["ID"],
                                    AirLineName = (string)reader["Airline_Name"],
                                    CountryCode = (long)reader["Country_Code"],
                                    Password = (string)reader["Password"],
                                    UserName = username
                                };
                            }
                        }
                    }
                }
            return airlineResult;
        }//not used

        public IList<AirlineCompany> GetAirlinesByCountry(long countryID)
        {
            if (countryID <= 0)
                throw new IllegalValueException($"Country ID: '{countryID}' is invalid ; less or equal to zero.");

            List<AirlineCompany> result;
                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_AIRLINES_BY_COUNTRY_ID", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("country_id", countryID));
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                                return null;
                         else
                            {
                            result = new List<AirlineCompany>();

                            result.Add(new AirlineCompany()
                            {
                                ID = (long)reader["ID"],
                                AirLineName = (string)reader["Airline_Name"],
                                CountryCode = (long)reader["Country_Code"],
                                Password = (string)reader["Password"],
                                UserName = (string)reader["User_Name"]
                            });

                                while (reader.Read())
                                {
                                    result.Add(new AirlineCompany()
                                    {
                                        ID = (long)reader["ID"],
                                        AirLineName = (string)reader["Airline_Name"],
                                        CountryCode = (long)reader["Country_Code"],
                                        Password = (string)reader["Password"],
                                        UserName = (string)reader["User_Name"]
                                    });
                                }
                            }
                        }
                    }
                }
            return result;
        }//not used

        public List<AirlineCompany> GetAll()
        {
            List<AirlineCompany> result;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("GET_ALL_AIRLINES", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            return null;

                        else
                            {
                                result = new List<AirlineCompany>();

                            result.Add(new AirlineCompany()
                            {
                                ID = (long)reader["ID"],
                                AirLineName = (string)reader["Airline_Name"],
                                CountryCode = (long)reader["Country_Code"],
                                Password = (string)reader["Password"],
                                UserName = (string)reader["User_Name"]
                            });

                                while (reader.Read())
                                {
                                    result.Add(new AirlineCompany()
                                    {
                                        ID = (long)reader["ID"],
                                        AirLineName = (string)reader["Airline_Name"],
                                        CountryCode = (long)reader["Country_Code"],
                                        Password = (string)reader["Password"],
                                        UserName = (string)reader["User_Name"]
                                    });
                                }
                            }
                        }
                    }
                }
            return result;
        }

        //Also removes the flights assigned to this airline.

        public void Remove(AirlineCompany toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Airline provided is empty or does not exist.");

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("REMOVE_AIRLINE", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_id", toRemove.ID));
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
        }

        public void Update(AirlineCompany toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Airline provided is empty or does not exist.");
            
                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("UPDATE_AIRLINE", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("airline_name", toUpdate.AirLineName));
                        command.Parameters.Add(new SqlParameter("airline_username", toUpdate.UserName));
                        command.Parameters.Add(new SqlParameter("password", toUpdate.Password));
                        command.Parameters.Add(new SqlParameter("country_code", toUpdate.CountryCode));
                        command.Parameters.Add(new SqlParameter("airline_id", toUpdate.ID));
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
        } 

        //Checks weather the username already exists in the database or not, before updating or adding.
        public bool CheckUsernameExistance(string username, long id)
        {
            if (username is null)
                return false;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                int invisibleID = 0;

                using (SqlCommand command = new SqlCommand("CHECK_CUSTOMER_USERNAME_EXISTANCE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("username", username));
                    command.Parameters.Add(new SqlParameter("customer_id", invisibleID));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                    }

                }

                using (SqlCommand commandTwo = new SqlCommand("CHECK_AIRLINE_USERNAME_EXISTANCE", Connection))
                {
                    commandTwo.Parameters.Add(new SqlParameter("airline_id", id));
                    commandTwo.Parameters.Add(new SqlParameter("username", username));
                    commandTwo.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader readerTwo = commandTwo.ExecuteReader())
                    {
                        if (readerTwo.Read())
                            return true;
                    }
                }

                using (SqlCommand commandThree = new SqlCommand("CHECK_ADMINISTRATOR_USERNAME_EXISTANCE", Connection))
                {
                    commandThree.Parameters.Add(new SqlParameter("id", invisibleID));
                    commandThree.Parameters.Add(new SqlParameter("username", username));
                    commandThree.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader readerThree = commandThree.ExecuteReader())
                    {
                        if (readerThree.Read())
                            return true;
                        else
                            return false;
                    }
                }
            }
        }

        //Checks weather the name already exists in the airline companies table or not, before adding or updating.
        public bool CheckNameExistance(string name, long id)
        {
            if (name is null)
                return false;

                using (Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    using (SqlCommand command = new SqlCommand("CHECK_AIRLINE_NAME_EXISTANCE", Connection))
                    {
                        command.Parameters.Add(new SqlParameter("name", name));
                        command.Parameters.Add(new SqlParameter("airline_id", id));
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                return true;
                            else
                                return false;
                        }
                    }
                }
        }

        //Removes all the airline companies in the database, CAN BE RUNNED ONLY ON THE REPLICA DB -> no such a procedure created for the regular db.
        public void RemoveAllReplica ()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_AIRLINES", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
            }
        }
    }
}
