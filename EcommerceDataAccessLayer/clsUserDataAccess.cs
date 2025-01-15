using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;

namespace EcommerceDataAccessLayer
{

    public class UserDTO
    {

     
        public UserDTO(int UserID,string UserName,string PasswordHash,string FUllName,
            string Email,string Phone,string Role)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.PasswordHash = PasswordHash;
            this.FullName = FUllName;
            this.Email = Email;
            this.Phone = Phone;
            this.Role = Role;

        }

      




        public int UserID { get; set; }

       
        public string UserName { get; set; }

      public string PasswordHash { get; set; }
        public string  FullName { get; set; }
       public string Email { get; set; }
      public string  Phone { get; set; }
       public string Role { get; set; }

    



    }




    public class clsUserDataAccess
    {

        public static List<UserDTO> GetAllUsernameAndUseridOfUsers()
        {
            var UsersList = new List<UserDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UsersList.Add(new UserDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("PasswordHash")),
                                reader.GetString(reader.GetOrdinal("FUllName")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Role"))

                            ));
                        }
                    }
                }


                return UsersList;
            }

        }



        public static List<UserDTO> GetAllAdminsUsers()
        {
            var StudentsList = new List<UserDTO>();

            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllAdminUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new UserDTO
                            (

                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("PasswordHash")),
                                reader.GetString(reader.GetOrdinal("FUllName")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Role"))
                               // reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                            ));
                        }
                    }
                }


                return StudentsList;
            }

        }


        public static UserDTO GetInfoUserById(int UserId)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_GetUserByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO
                        (

                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Username")),
                                reader.GetString(reader.GetOrdinal("PasswordHash")),
                                reader.GetString(reader.GetOrdinal("FUllName")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Role"))
                                //reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                               

                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static int AddNewUser(UserDTO userDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Username", userDTO.UserName);
                command.Parameters.AddWithValue("@PasswordHash", userDTO.PasswordHash);
                command.Parameters.AddWithValue("@FullName", userDTO.FullName);
                command.Parameters.AddWithValue("@Email", userDTO.Email);
                command.Parameters.AddWithValue("@Phone", userDTO.Phone);
                command.Parameters.AddWithValue("@Role", userDTO.Role);
                var outputIdParam = new SqlParameter("@NewUserId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                 command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }



        public static bool UpdateUser(UserDTO userDTO)
        {
            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))
            using (var command = new SqlCommand("SP_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", userDTO.UserID);
                command.Parameters.AddWithValue("@Username", userDTO.UserName);
                command.Parameters.AddWithValue("@PasswordHash", userDTO.PasswordHash);
                command.Parameters.AddWithValue("@FullName", userDTO.FullName);
                command.Parameters.AddWithValue("@Email", userDTO.Email);
                command.Parameters.AddWithValue("@Phone", userDTO.Phone);
                command.Parameters.AddWithValue("@Role", userDTO.Role);

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }


        public static bool DeleteUser(int UserId)
        {

            using (var connection = new SqlConnection(clsDataAccessSettings._connectionString))

            using (var command = new SqlCommand("SP_DeleteUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserId);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }


    }
}
