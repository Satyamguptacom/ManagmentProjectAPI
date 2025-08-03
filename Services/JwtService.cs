﻿
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using global::ProjectManagementAPI.Models;
using Microsoft.IdentityModel.Tokens;
namespace ProjectManagementAPI.Services
{

        public class JwtService
        {
            private readonly IConfiguration _config;

            public JwtService(IConfiguration config)
            {
                _config = config;
            }
           
            public string GenerateToken(User user)
            {

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["JwtSettings:Issuer"],
                    audience: _config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiryMinutes"])),
                    signingCredentials: creds
                );
            try
            {
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                Console.WriteLine("Generated Token: " + tokenString);
                return tokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token Generation Error: " + ex.Message);
                return null;
            }
        }
        }
    }

