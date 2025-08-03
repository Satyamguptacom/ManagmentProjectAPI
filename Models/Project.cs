using static ProjectManagementAPI.Models.Enum.Enums;

namespace ProjectManagementAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ProjectManagerId { get; set; }
        public List<int> DeveloperIds { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public ProjectStatus Status { get; set; }
    }

    public class CreateProjectRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
