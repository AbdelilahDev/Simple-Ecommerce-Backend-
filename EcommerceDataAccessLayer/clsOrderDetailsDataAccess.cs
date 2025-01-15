using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceDataAccessLayer
{
    public class OrderDetailsDTO
    {
        public OrderDetailsDTO(int OrderDetailID,int OrderID,int ProductID,int Quantity,decimal Price,decimal TotalAmount)
        {
            this.OrderDetailID = OrderDetailID;
            this.OrderID = OrderID;
            this.ProductID = ProductID;
            this.Quantity = Quantity;
            this.Price = Price;
            this.TotalAmount = TotalAmount;
            
        }


        public int   OrderDetailID { get; set; }
        public int  OrderID { get; set; }
        public int    ProductID { get; set; }
        public int     Quantity { get; set; }
        public decimal    Price { get; set; }
        public decimal   TotalAmount { get; set; }
  
    }

    public class clsOrderDetailsDataAccess
    {

        public static int AddNewOrder(OrderDetailsDTO orderDetailDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewOrderDetail", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", orderDetailDTO.OrderID);
                command.Parameters.AddWithValue("@ProductID", orderDetailDTO.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderDetailDTO.Quantity);
                command.Parameters.AddWithValue("@Price", orderDetailDTO.Price);
                command.Parameters.AddWithValue("@TotalAmount", orderDetailDTO.TotalAmount);


                var outputIdParam = new SqlParameter("@NewOrderDetailID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }



        public static OrderDetailsDTO GetInfoOrderDetailsById(int OrderDetailsID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetOrderDetailsByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", OrderDetailsID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderDetailsDTO
                        (

                                reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetDecimal(reader.GetOrdinal("TotalAmount"))

                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



        public static List<OrderDetailsDTO> GetAllOrdersDetials()
        {
            var OrdersDetialsList = new List<OrderDetailsDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllOrderDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersDetialsList.Add(new OrderDetailsDTO
                            (


                                reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                                reader.GetInt32(reader.GetOrdinal("OrderID")),
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetInt32(reader.GetOrdinal("Quantity")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetDecimal(reader.GetOrdinal("TotalAmount"))

                            ));
                        }
                    }
                }


                return OrdersDetialsList;
            }

        }
    }
}
