using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using ProjectManagementAPI.Data;
using System.Text;
using System.Security.Cryptography;
using ProjectManagementAPI.Models.Registration;
using System.Data;
using ProjectManagementAPI.Models.Login;
using ProjectManagementAPI.Models;
using static ProjectManagementAPI.Models.Enum.Enums;

namespace ProjectManagementAPI.Services
{
    public class AuthService
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthService(JwtService jwtService, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDTO request)
        {
            SqlParameter[] parameters =
            {
        new SqlParameter("@Email", request.Email)
    };
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration, connectionString, "SP_Login_User_By_Email", CommandType.StoredProcedure, parameters))
            {
                if (!reader.HasRows)
                    return null;

                await reader.ReadAsync();

                string dbHash = reader["PasswordHash"].ToString();

                if (dbHash != HashPassword(request.Password))
                    return null;

                var user = new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Email = reader["Email"].ToString(),
                    Username = reader["Username"].ToString(),
                    Role = Enum.Parse<UserRole>(reader["Role"].ToString())
                };

                return _jwtService.GenerateToken(user);
            }
        }

        public async Task<DbResponse> RagisterUser(User request)
        {
            DbResponse response = new DbResponse();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Username", request.Username),
                    new SqlParameter("@Email", request.Email),
                    new SqlParameter("@PasswordHash", HashPassword(request.Password)),
                    new SqlParameter("@Role", request.Role.ToString()),
                   new SqlParameter("@CreatedAt", DateTime.UtcNow)
                };

                using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration, connectionString, "SP_Register_User", CommandType.StoredProcedure, parameters))
                {
                    if (reader.Read())
                    {
                        response.ResponseMessage = reader["ResponseMessage"]?.ToString();
                        response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                        response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
                    }
                    else
                    {
                        response.ResponseMessage = "No response from database";
                        response.ResponseCode = 500;
                        response.ResponseId = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseMessage = $"Error: {ex.Message}";
                response.ResponseCode = 500;
                response.ResponseId = 0;
            }

            return response;
        }
        //public async Task<bool> RegisterAsync(RegistrationDTO request)
        //{
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //    SqlParameter[] parameters =
        //    {
        //        new SqlParameter("@Username", request.Username),
        //        new SqlParameter("@Email", request.Email),
        //        new SqlParameter("@PasswordHash", HashPassword(request.Password)),
        //        new SqlParameter("@Role", request.Role.ToString()),
        //        new SqlParameter("@CreatedAt", DateTime.UtcNow)
        //    };

        //    var result = await SqlHelper.ExecuteNonQueryAsync(connectionString, "SP_Register_User",CommandType.StoredProcedure,parameters);

        //    return result > 0;
        //}

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

}
