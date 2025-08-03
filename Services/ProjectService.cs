using Microsoft.Data.SqlClient;
using ProjectManagementAPI.Data;
using static ProjectManagementAPI.Models.Enum.Enums;
using System.Data;
using ProjectManagementAPI.Interface;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Services
{
    public class ProjectService : IProject
    {
        private readonly IConfiguration _configuration;

        public ProjectService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DbResponse> CreateProject(Project project)
        {
            var response = new DbResponse();

            try
            {
                SqlParameter[] projectParams = new[]
                {
                new SqlParameter("@Id", project.Id),
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", project.Description),
                new SqlParameter("@ProjectManagerId", project.ProjectManagerId),
                new SqlParameter("@Status", project.Status.ToString()),
                new SqlParameter("@CreatedAt", DateTime.Now)
            };

                using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_CreateProject", CommandType.StoredProcedure, projectParams))
                {
                    if (reader.Read())
                    {
                        response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                        response.ResponseMessage = reader["ResponseMessage"].ToString();
                        response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
                    }
                }

                foreach (var devId in project.DeveloperIds)
                {
                    SqlParameter[] devParams = new[]
                    {
                    new SqlParameter("@ProjectId",project.Id),
                    new SqlParameter("@DeveloperId", devId)
                };

                    using (var data = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                        "SP_AddDeveloperToProject", CommandType.StoredProcedure, devParams))
                    {
                        if (data.Read())
                        {
                            response.ResponseCode = Convert.ToInt32(data["ResponseCode"]);
                            response.ResponseMessage = data["ResponseMessage"].ToString();
                            response.ResponseId = Convert.ToInt64(data["ResponseId"]);
                        }
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

        public async Task<List<Project>> GetAllProjects()
        {
            var projects = new List<Project>();

            try
            {
                using var reader = await SqlHelper.ExecuteReaderAsync(
                    _configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_GetAllProjects", CommandType.StoredProcedure);

                while (reader.Read())
                {
                    projects.Add(new Project
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        Status = Enum.TryParse<ProjectStatus>(reader["Status"].ToString(), out var status) ? status : ProjectStatus.NotStarted,
                        DeveloperIds = new List<int>()
                    });
                }
            }
            catch (Exception ex)
            {
                // optionally log exception
            }

            return projects;
        }

        public async Task<Project> GetProjectById(int id)
        {
            Project project = null;

            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Id", id) };

                using (var reader = await SqlHelper.ExecuteReaderAsync(
                    _configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_GetProjectById", CommandType.StoredProcedure, parameters))
                {
                    if (reader.Read())
                    {
                        project = new Project
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            ProjectManagerId = Convert.ToInt32(reader["ProjectManagerId"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            Status = Enum.TryParse<ProjectStatus>(reader["Status"].ToString(), out var status) ? status : ProjectStatus.NotStarted,
                            DeveloperIds = new List<int>()
                        };
                    }
                }

                // Get DeveloperIds
                var devParams = new[] { new SqlParameter("@ProjectId", id) };
                using (var devReader = await SqlHelper.ExecuteReaderAsync(
                    _configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SELECT DeveloperId FROM ProjectDevelopers WHERE ProjectId = @ProjectId",
                    CommandType.Text, devParams))
                {
                    while (devReader.Read())
                    {
                        project?.DeveloperIds.Add(Convert.ToInt32(devReader["DeveloperId"]));
                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            return project;
        }

        public async Task<DbResponse> UpdateProject(int id, Project project)
        {
            var response = new DbResponse();

            try
            {
                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@Id", id),
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", project.Description),
                new SqlParameter("@Status", project.Status.ToString())
            };

                using (var reader = await SqlHelper.ExecuteReaderAsync(
                    _configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_UpdateProject", CommandType.StoredProcedure, parameters))
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

        public async Task<DbResponse> DeleteProject(int id)
        {
            var response = new DbResponse();

            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Id",id) };

                using (var reader = await SqlHelper.ExecuteReaderAsync(
                    _configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_DeleteProject", CommandType.StoredProcedure, parameters))
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

        public async Task<DbResponse> AssignDevelopers(long projectId, List<int> developerIds)
        {
            var response = new DbResponse();

            try
            {
                SqlParameter[] deleteParams = {
                new SqlParameter("@ProjectId", projectId)
            };

                using (var deleteReader = await SqlHelper.ExecuteReaderAsync(
                    _configuration,
                    _configuration.GetConnectionString("DefaultConnection"),
                    "SP_DeleteAllDevelopersFromProject",
                    CommandType.StoredProcedure,
                    deleteParams))
                {
                    if (deleteReader.Read())
                    {
                        response.ResponseCode = Convert.ToInt32(deleteReader["ResponseCode"]);
                        response.ResponseMessage = deleteReader["ResponseMessage"].ToString();
                    }
                }

                foreach (var devId in developerIds)
                {
                    SqlParameter[] assignParams = {
                    new SqlParameter("@ProjectId", projectId),
                    new SqlParameter("@DeveloperId", devId)
                };

                    using (var assignReader = await SqlHelper.ExecuteReaderAsync(
                        _configuration,
                        _configuration.GetConnectionString("DefaultConnection"),
                        "SP_AddDeveloperToProject",
                        CommandType.StoredProcedure,
                        assignParams))
                    {
                        if (assignReader.Read())
                        {
                            response.ResponseCode = Convert.ToInt32(assignReader["ResponseCode"]);
                            response.ResponseMessage = assignReader["ResponseMessage"].ToString();
                            response.ResponseId = Convert.ToInt32(assignReader["ResponseId"]);
                        }
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
    }
}
