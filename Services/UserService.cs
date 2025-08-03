using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectManagementAPI.Data;
using ProjectManagementAPI.Interface;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Registration;
using System.Data;
using System.Text;

namespace ProjectManagementAPI.Services
{
    public class UserService:IUser
    {
        private readonly IConfiguration _configuration;

        public UserService( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public async Task<DbResponse> CreateUser(User request)
        //{
        //    DbResponse response = new DbResponse();
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");

        //    try
        //    {
        //        SqlParameter[] parameters =
        //        {
        //            new SqlParameter("@Username", request.Username),
        //            new SqlParameter("@Email", request.Email),
        //            new SqlParameter("@PasswordHash", HashPassword(request.Password)),
        //            new SqlParameter("@Role", request.Role.ToString()),
        //           new SqlParameter("@CreatedAt", DateTime.UtcNow)
        //        };

        //        using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration, connectionString, "SP_Register_User", CommandType.StoredProcedure, parameters))
        //        {
        //            if (reader.Read())
        //            {
        //                response.ResponseMessage = reader["ResponseMessage"]?.ToString();
        //                response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
        //                response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
        //            }
        //            else
        //            {
        //                response.ResponseMessage = "No response from database";
        //                response.ResponseCode = 500;
        //                response.ResponseId = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.ResponseMessage = $"Error: {ex.Message}";
        //        response.ResponseCode = 500;
        //        response.ResponseId = 0;
        //    }

        //    return response;
        //}

        public async Task<List<User>> GetAllUser()
        {
            List<User> userList = new List<User>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                SqlParameter[] parameters = { };

                using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration,connectionString, "SP_GetAllUsers", CommandType.StoredProcedure, parameters))
                {
                    while (reader.Read())
                    {
                        userList.Add(new User
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt64(reader["Id"]) : 0,
                            Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : string.Empty,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                            RoleName = reader["Role"] != DBNull.Value ? Convert.ToString(reader["Role"]) : string.Empty,
                            CreatedAt = reader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedAt"]) : DateTime.MinValue
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                // Logging can be done here
                throw new Exception("Error fetching users: " + ex.Message);
            }
            return userList;
        }

        //[HttpPut("{Id}/{UserName}/{Email}")]
        public async Task<DbResponse> UpdateUser(long Id,string UserName,string Email)
        {
            var response = new DbResponse();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@Id", Id),
                new SqlParameter("@Username",UserName),
                new SqlParameter("@Email", Email)
            };

                using (var reader = await SqlHelper.ExecuteReaderAsync( _configuration, connectionString, "SP_UpdateUser", CommandType.StoredProcedure,parameters))
                {
                    if (reader.Read())
                    {
                        response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                        response.ResponseMessage = reader["ResponseMessage"].ToString();
                        response.ResponseId =Convert.ToInt64(reader["ResponseId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseId = 0;
            }
            return response;
        }
        public async Task<DbResponse> UpdateUserRole(long Id, UpdateRoleUpdateDTO request)
        {
            var response = new DbResponse();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@Id", Id),
                new SqlParameter("@Role", request.Role)
            };

                using (var reader = await SqlHelper.ExecuteReaderAsync( _configuration, connectionString, "SP_UpdateUserRole",CommandType.StoredProcedure,parameters))
                {
                    if (reader.Read())
                    {
                        response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                        response.ResponseMessage = reader["ResponseMessage"].ToString();
                        response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseId = 0;
            }
            return response;
        }

        public async Task<DbResponse> DeleteUser(long Id)
        {
            var response = new DbResponse();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@Id", Id)
            };

                using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration, connectionString, "SP_UpdateUserRole", CommandType.StoredProcedure, parameters))
                {
                    if (reader.Read())
                    {
                        response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                        response.ResponseMessage = reader["ResponseMessage"].ToString();
                        response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseId = 0;
            }
            return response;
        }


        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
