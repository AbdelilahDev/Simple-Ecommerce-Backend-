using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceDataAccessLayer
{
    public class ProductDTO
    {

        public ProductDTO(int ProductID, string Name, string Description, decimal Price, int StockQauntity )
        {
            this.ProductID = ProductID;
            this.Name = Name;
            this.Description = Description;
            this.Price = Price;
            this.StockQauntity = StockQauntity;
        }
        public int ProductID { get; set; }
        public string Name   { get; set; }
        public string Description { get; set; }

        public decimal Price     { get; set; }

        public int StockQauntity { get; set; }

    }


    public class clsProductDataAccess
    {

        public static List<ProductDTO> GetAllProducts()
        {
            var ProductsList = new List<ProductDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllProducts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductsList.Add(new ProductDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("StockQuantity"))




                            ));
                        }
                    }
                }


                return ProductsList;
            }

        }

        public static int AddNewProduct(ProductDTO productDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_AddNewProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", productDTO.Name);
                command.Parameters.AddWithValue("@Description",productDTO.Description);
                command.Parameters.AddWithValue("@Price",productDTO.Price);
                command.Parameters.AddWithValue("@StockQuantity", productDTO.StockQauntity);
            
                var outputIdParam = new SqlParameter("@NewProductId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }


        public static ProductDTO GetInfoProductById(int ProductID)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetProductByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", ProductID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProductDTO
                        (

                                reader.GetInt32(reader.GetOrdinal("ProductID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetString(reader.GetOrdinal("Description")),
                                reader.GetDecimal(reader.GetOrdinal("Price")),
                                reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



        public static bool UpdateProduct(ProductDTO productDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", productDTO.ProductID);
                command.Parameters.AddWithValue("@Name", productDTO.Name);
                command.Parameters.AddWithValue("@Description", productDTO.Description);
                command.Parameters.AddWithValue("@Price", productDTO.Price);
                command.Parameters.AddWithValue("@StockQuantity ", productDTO.StockQauntity);
             

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }




        public static bool DeleteProduct(int ProductId)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))

            using (var command = new SqlCommand("SP_DeleteProduct", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", ProductId);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }

    }
}
