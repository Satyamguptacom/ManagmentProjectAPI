using static ProjectManagementAPI.Models.Enum.Enums;

namespace ProjectManagementAPI.Models.Registration
{
    public class RegistrationDTO
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
