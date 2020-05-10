using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.DAO.MSSQL
{
    public class AdministratorDAOMSSQL : BasicDAO<Administrator>, IAdministratorDAO, IDAOTest
    {
        private SqlConnection Connection;
        public Administrator Add(Administrator toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Customer provided is empty.");
            if (toAdd.Password is null || toAdd.Username is null)
                throw new IllegalValueException($"One of the field: '{toAdd.Username}', '{toAdd.Password}' are not filled.");
            if (CheckUsernameExistance(toAdd.Username, toAdd.ID))
                throw new AlreadyExistsException($"The username: '{toAdd.Username}' already exists.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("ADD_ADMINISTRATOR", Connection))
                {
                    command.Parameters.Add(new SqlParameter("password", toAdd.Password));
                    command.Parameters.Add(new SqlParameter("username", toAdd.Username));
                    command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt32(resultString);
                    return toAdd;
                }
            }
        }//not used

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
                    commandTwo.Parameters.Add(new SqlParameter("airline_id", invisibleID));
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
                    commandThree.Parameters.Add(new SqlParameter("id", id));
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
        }//not used outside
    
        public Administrator Get(long id)
        {
            if (id <= 0)
                throw new IllegalValueException($"ID: '{id}' is less or equal to zero.");

            Administrator result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ADMINISTRATOR_BY_ID", Connection))
                {
                    command.Parameters.Add(new SqlParameter("id", id));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Administrator()
                            {
                                ID = (int)reader["ID"],
                                Username = (string)reader["User_Name"]
                            };
                        }
                    }
                }
            }

            return result;
        }//not used

        public Administrator GetAdminByUsername(string username)
        {
            if (username is null)
                throw new NullReferenceException("Username provided is empty.");

            Administrator result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ADMINISTRATOR_BY_USERNAME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("username", username));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Administrator()
                            {
                                ID = (int)reader["ID"],
                                Username = (string)reader["User_Name"],
                                Password = (string)reader["Password"]
                            };
                        }
                    }
                }
            }

            return result;
        }//not used

        public List<Administrator> GetAll()
        {
            List<Administrator> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ALL_ADMINISTRATORS", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new List<Administrator>();
                            result.Add(new Administrator()
                            {
                                ID = (int)reader["ID"],
                                Username = (string)reader["User_Name"],
                                Password = (string)reader["Password"]
                            });

                            while (reader.Read())
                            {
                                result.Add(new Administrator()
                                {
                                    ID = (int)reader["ID"],
                                    Username = (string)reader["User_Name"],
                                    Password = (string)reader["Password"]
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }// not used

        public void Remove(Administrator toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Administrator provided is empty.");
            if (toRemove.ID <= 0)
                throw new IllegalValueException($"Administrator's ID: '{toRemove.ID}' is not valid; less or equal to zero.");
            if (Get(toRemove.ID) == null)
                throw new UserNotFoundException($"The administrator: id '{toRemove.ID}' could not be found.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("REMOVE_ADMINISTRATOR", Connection))
                {
                    command.Parameters.Add(new SqlParameter("id", toRemove.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }// not used

        public void RemoveAllReplica()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_ADMINISTRATORS", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
            }
        }

        public void Update(Administrator toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Customer provided is empty.");
            if (toUpdate.ID <= 0)
                throw new IllegalValueException($"Customer's ID: '{toUpdate.ID}' is not valid; equal or less than zero.");
            if (Get(toUpdate.ID) == null)
                throw new UserNotFoundException($"Customer id: '{toUpdate.ID}' could not be found.");
            if (toUpdate.Password is null || toUpdate.Username is null)
                throw new IllegalValueException($"Not all fields are filled: '{toUpdate.Username}', '{toUpdate.Password}'.");
            if (CheckUsernameExistance(toUpdate.Username, toUpdate.ID))
                throw new AlreadyExistsException($"Username provided: '{toUpdate.Username}' already exists.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE_ADMINISTRATOR", Connection))
                {
                    command.Parameters.Add(new SqlParameter("id", toUpdate.ID));
                    command.Parameters.Add(new SqlParameter("username", toUpdate.Username));
                    command.Parameters.Add(new SqlParameter("password", toUpdate.Password));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }//not used
    }
}
