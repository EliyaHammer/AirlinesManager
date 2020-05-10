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
    public class CountryDAOMSSQL : BasicDAO<Country> ,ICountryDAO, IDAOTest
    {
        private SqlConnection Connection;

        public CountryDAOMSSQL() { }

        public Country Add(Country toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Country provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("ADD_COUNTRY", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_name", toAdd.CountryName));
                    command.CommandType = CommandType.StoredProcedure;

                    object result = command.ExecuteScalar();
                    string resultString = result.ToString();
                    toAdd.ID = Convert.ToInt64(resultString);
                    return toAdd;
                }
            }
        }

        public Country Get(long id)
        {
            if (id <= 0)
                throw new IllegalValueException($"Provided ID: '{id}' is invalid, less or equal to zero");

            Country result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_COUNTRY_BY_ID", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_id", id));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new Country()
                            {
                                ID = (long)reader["ID"],
                                CountryName = (string)reader["Country_Name"]
                            };
                        }
                    }
                }
            }

            return result;
        }//not used

        public List<Country> GetAll()
        {
            List<Country> result;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_ALL_COUNTRIES", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;
                        else
                        {
                            result = new List<Country>();
                            result.Add(new Country()
                            {
                                ID = (long)reader["ID"],
                                CountryName = (string)reader["Country_Name"]
                            });

                            while (reader.Read())
                            {
                                result.Add(new Country()
                                {
                                    ID = (long)reader["ID"],
                                    CountryName = (string)reader["Country_Name"]
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }//not used.

        public Country GetCountryByName(string name)
        {
            if (name is null)
                throw new NullReferenceException("Name provided is empty.");

            Country result;
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("GET_COUNTRY_BY_NAME", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_name", name));
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null; ;

                        result = new Country()
                        {
                            ID = (long)reader["ID"],
                            CountryName = (string)reader["Country_Name"]
                        };    
                    }
                }
            }

            return result;
        }//not used

        /// <summary>
        /// Removes also the airlines assigned to this country.
        /// </summary>
        /// <param name="toRemove">country to remove</param>
        public void Remove(Country toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Country provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("REMOVE_COUNTRY", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_id", toRemove.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        public void Update(Country toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Country provided is empty.");

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE_COUNTRY", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_name", toUpdate.CountryName));
                    command.Parameters.Add(new SqlParameter("country_id", toUpdate.ID));
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        //Checks weather the name of the country already exists in the database.
        public bool CheckNameExistance (string name, long id) 
        {
            if (name is null)
                return false;

            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();

                using (SqlCommand command = new SqlCommand("CHECK_COUNTRY_NAME_EXISTANCE", Connection))
                {
                    command.Parameters.Add(new SqlParameter("country_name", name));
                    command.Parameters.Add(new SqlParameter("country_id", id));
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

        //Removes all the countries from the replica db -> ONLY WORKS ON THE REPLICA DB.
        public void RemoveAllReplica()
        {
            using (Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                {
                    using (SqlCommand command = new SqlCommand("REMOVE_ALL_COUNTRIES", Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                    }
                }
            }
        }
    }
}

