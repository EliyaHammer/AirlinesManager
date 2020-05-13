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
    public class CustomerDAOMSSQL : BasicDAO<Customer>, ICustomerDAO
    {
        private SqlConnection Connection;

        public CustomerDAOMSSQL() { }

        public Customer Add(Customer toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Customer provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("ADD_CUSTOMER", Connection))
                {
                    command.Parameters.Add(new SqlParameter("first_name", toAdd.FirstName));
                    command.Parameters.Add(new SqlParameter("last_name", toAdd.LastName));
                    command.Parameters.Add(new SqlParameter("address", toAdd.Address));
                    command.Parameters.Add(new SqlParameter("card_number", toAdd.CreditCardNumber));
                    command.Parameters.Add(new SqlParameter("password", toAdd.Password));
                    command.Parameters.Add(new SqlParameter("phone_number", toAdd.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("username", toAdd.UserName));
                    command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt64(resultString);
                    return toAdd;
                }
            }
        }

        public Customer Get(long id)
        {
            if (id <= 0)
                throw new IllegalValueException($"ID provided: '{id}' is invalid; less or equal to zero.");

            Customer result; 

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_CUSTOMER_BY_ID", Connection))
                {
                    command.Parameters.Add(new SqlParameter("id", id));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Customer()
                            {
                                ID = (long)reader["ID"],
                                Address = (string)reader["Address"],
                                CreditCardNumber = (string)reader["Credit_Card_Number"],
                                FirstName = (string)reader["First_Name"],
                                LastName = (string)reader["Last_Name"],
                                Password = (string)reader["Password"],
                                PhoneNumber = (string)reader["Phone_Number"],
                                UserName = (string)reader["User_Name"]
                            };
                        }
                    }
                }
            }

            return result;
        }//not used

        public List<Customer> GetAll()
        {
            List<Customer> result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ALL_CUSTOMERS", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new List<Customer>();
                            result.Add(new Customer()
                            {
                                ID = (long)reader["ID"],
                                Address = (string)reader["Address"],
                                CreditCardNumber = (string)reader["Credit_Card_Number"],
                                FirstName = (string)reader["First_Name"],
                                LastName = (string)reader["Last_Name"],
                                Password = (string)reader["Password"],
                                PhoneNumber = (string)reader["Phone_Number"],
                                UserName = (string)reader["User_Name"]
                            });

                            while (reader.Read())
                            {
                                result.Add(new Customer()
                                {
                                    ID = (long)reader["ID"],
                                    Address = (string)reader["Address"],
                                    CreditCardNumber = (string)reader["Credit_Card_Number"],
                                    FirstName = (string)reader["First_Name"],
                                    LastName = (string)reader["Last_Name"],
                                    Password = (string)reader["Password"],
                                    PhoneNumber = (string)reader["Phone_Number"],
                                    UserName = (string)reader["User_Name"]
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }//not used

        public List<Customer> GetCustomerByFullName(string firstName, string lastName)
        {
            if (firstName is null || lastName is null)
                throw new NullReferenceException("First Name or Last Name was not filled.");

            List<Customer> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_CUSTOMER_BY_FULL_NAME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("first_name", firstName));
                    command.Parameters.Add(new SqlParameter("last_name", lastName));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        result = new List<Customer>();
                        result.Add(new Customer()
                        {
                            ID = (long)reader["ID"],
                            Address = (string)reader["Address"],
                            CreditCardNumber = (string)reader["Credit_Card_Number"],
                            FirstName = (string)reader["First_Name"],
                            LastName = (string)reader["Last_Name"],
                            Password = (string)reader["Password"],
                            PhoneNumber = (string)reader["Phone_Number"],
                            UserName = (string)reader["User_Name"]
                        });

                        while (reader.Read())
                        {
                            result.Add(new Customer()
                            {
                               ID = (long)reader["ID"],
                               Address = (string)reader["Address"],
                               CreditCardNumber = (string)reader["Credit_Card_Number"],
                               FirstName = (string)reader["First_Name"],
                               LastName = (string)reader["Last_Name"],
                               Password = (string)reader["Password"],
                               PhoneNumber = (string)reader["Phone_Number"],
                               UserName = (string)reader["User_Name"]
                            });
                        }
                        
                    }
                }
            }

            return result;
        }//not used

        public Customer GetCustomerByUsername(string username)
        {
            if (username is null)
                throw new NullReferenceException("Username provided is empty.");

            Customer result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_CUSTOMER_BY_USERNAME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("username", username));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Customer()
                            {
                                ID = (long)reader["ID"],
                                Address = (string)reader["Address"],
                                CreditCardNumber = (string)reader["Credit_Card_Number"],
                                FirstName = (string)reader["First_Name"],
                                LastName = (string)reader["Last_Name"],
                                Password = (string)reader["Password"],
                                PhoneNumber = (string)reader["Phone_Number"],
                                UserName = (string)reader["User_Name"]
                            };
                        }
                    }
                }
            }

            return result;
        }//not used

        public void Remove(Customer toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Customer provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("REMOVE_CUSTOMER", Connection))
                {
                    command.Parameters.Add(new SqlParameter("customer_id", toRemove.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        public void Update(Customer toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Customer provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE_CUSTOMER", Connection))
                {
                    command.Parameters.Add(new SqlParameter("first_name", toUpdate.FirstName));
                    command.Parameters.Add(new SqlParameter("address", toUpdate.Address));
                    command.Parameters.Add(new SqlParameter("card_number", toUpdate.CreditCardNumber));
                    command.Parameters.Add(new SqlParameter("last_name", toUpdate.LastName));
                    command.Parameters.Add(new SqlParameter("password", toUpdate.Password));
                    command.Parameters.Add(new SqlParameter("phone_number", toUpdate.PhoneNumber));
                    command.Parameters.Add(new SqlParameter("username", toUpdate.UserName));
                    command.Parameters.Add(new SqlParameter("customer_id", toUpdate.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        //Checks weather or not the username already exists in the db (before updating or adding).
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
                    command.Parameters.Add(new SqlParameter("customer_id", id));
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

        //Removes all the customers from the Replica DB. Will not work on the actual DB.
        public void RemoveAllReplica()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_CUSTOMERS", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
            }
        }
    }
}
