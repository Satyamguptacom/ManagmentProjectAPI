using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Interface
{
    public interface ITaskService
    {
        Task<DbResponse> CreateTask(ProjectTask task);
        Task<DbResponse> UpdateTask(ProjectTask task);
        Task<DbResponse> UpdateTaskStatus(long id, TaskStatus status);
        Task<List<ProjectTask>> GetTasksByProjectId(long projectId);
        Task<DbResponse> DeleteTask(long id);
    }
}
