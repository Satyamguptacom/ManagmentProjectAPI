using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Interface
{
    public interface IProject
    {
        Task<DbResponse> CreateProject(Project project);
        Task<List<Project>> GetAllProjects();
        Task<Project> GetProjectById(int id);
        Task<DbResponse> UpdateProject(int id, Project project);
        Task<DbResponse> DeleteProject(int id);
       Task<DbResponse> AssignDevelopers(long projectId, List<int> developerIds);
    }
}
