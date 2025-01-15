using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataAccessLayer
{


    public class OrderDTO
    {
        public OrderDTO(int OrderID,int CustomerID,string ShippingAddress,string OrderStutas)
        {
            this.OrderID = OrderID;
            this.CustomerID = CustomerID;
         
            this.ShippingAddress = ShippingAddress;
            this.OrderStutas = OrderStutas;
            
        }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }

        public string ShippingAddress { get; set; }
        public string OrderStutas { get; set; }

    }

    public class clsOrdersDataAccess
    {
        public static int AddNewOrder(OrderDTO orderDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerID", orderDTO.CustomerID);
                command.Parameters.AddWithValue("@ShippingAddress", orderDTO.ShippingAddress);
                command.Parameters.AddWithValue("@OrderStatus", orderDTO.OrderStutas);

                var outputIdParam = new SqlParameter("@NewOrderId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }



        public static OrderDTO GetInfoOrderById(int OrderID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetOrderByOrderID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderDTO
                        (

                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                reader.GetString(reader.GetOrdinal("ShippingAddress")),
                                reader.GetString(reader.GetOrdinal("OrderStatus"))

                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static List<OrderDTO> GetAllOrders()
        {
            var OrdersList = new List<OrderDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllOrders", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersList.Add(new OrderDTO
                            (

                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                reader.GetString(reader.GetOrdinal("ShippingAddress")),
                                reader.GetString(reader.GetOrdinal("OrderStatus"))

                            ));
                        }
                    }
                }


                return OrdersList;
            }

        }



        public static bool IsOrderExist(int OrderID)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))

            using (var command = new SqlCommand("SP_CheckOrderExistence", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderID);

                var outputIdParam = new SqlParameter("@ExistOrder", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value == 1;

            }
        }



    }
}
