using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataAccessLayer
{
    public class CustomerDTO
    {

        public CustomerDTO(int CustomerID,string FullName,string Email,string Phone,string City)

        {
            this.CustomerID = CustomerID;
            this.FullName = FullName;
            this.Email = Email;
            this.Phone = Phone;
            this.City = City;


        }
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

    }

    public class clsCustomersDataAccess
    {
        public static int AddNewCostumer(CustomerDTO customerDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewCustomer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FullName", customerDTO.FullName);
                command.Parameters.AddWithValue("@Email", customerDTO.Email);
                command.Parameters.AddWithValue("@Phone", customerDTO.Phone);
                command.Parameters.AddWithValue("@City", customerDTO.City);
                var outputIdParam = new SqlParameter("@NewCostomerId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }


        public static CustomerDTO GetInfoCustomerById(int CustomerID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetCustomerByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", CustomerID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CustomerDTO
                        (
                                reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                reader.GetString(reader.GetOrdinal("FullName")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("City"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



        public static bool IsCustomerExist(int CustomerID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))

            using (var command = new SqlCommand("SP_CheckCustomerExistence", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", CustomerID);

                var outputIdParam = new SqlParameter("@ExistCustomer", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value==1;

            }
        }

        public static bool UpdateCustomer(CustomerDTO customerDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateCustomer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerID", customerDTO.CustomerID);
                command.Parameters.AddWithValue("@FullName", customerDTO.FullName);
                command.Parameters.AddWithValue("@Email", customerDTO.Email);
                command.Parameters.AddWithValue("@Phone", customerDTO.Phone);
                command.Parameters.AddWithValue("@City", customerDTO.City);


                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }


        public static bool DeleteCustomer(int CustomerID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))

            using (var command = new SqlCommand("SP_DeleteCustomer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", CustomerID);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }

    }
}
