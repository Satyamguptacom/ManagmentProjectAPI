using static ProjectManagementAPI.Models.Enum.Enums;

namespace ProjectManagementAPI.Models
{
    public class User
    {
        public long Id { get; set; } // SQL Server uses int
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UpdateRoleUpdateDTO
    {
        public UserRole Role { get; set; }
    }

}
