using Microsoft.Data.SqlClient;
using ProjectManagementAPI.Data;
using ProjectManagementAPI.Interface;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Enum;
using System.Data;

namespace ProjectManagementAPI.Services
{
        public class TaskService : ITaskService
        {
            private readonly IConfiguration _configuration;

            public TaskService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<DbResponse> CreateTask(ProjectTask task)
            {
                var response = new DbResponse();

                SqlParameter[] parameters =
                {
            new SqlParameter("@Id", task.Id),
            new SqlParameter("@Title", task.Title),
            new SqlParameter("@Description", (object)task.Description ?? DBNull.Value),
            new SqlParameter("@ProjectId",task.ProjectId),
            new SqlParameter("@AssignedToId",task.AssignedToId),
            new SqlParameter("@Status", task.Status.ToString()),
            new SqlParameter("@CreatedAt", DateTime.UtcNow),
            new SqlParameter("@DueDate", (object?)task.DueDate ?? DBNull.Value)
        };

                using var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_CreateTask", CommandType.StoredProcedure, parameters);

                if (reader.Read())
                {
                    response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                    response.ResponseMessage = reader["ResponseMessage"].ToString();
                response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
            }

                return response;
            }

            public async Task<DbResponse> UpdateTask(ProjectTask task)
            {
                var response = new DbResponse();

                SqlParameter[] parameters =
                {
            new SqlParameter("@Id",task.Id),
            new SqlParameter("@Title", task.Title),
            new SqlParameter("@Description", (object?)task.Description ?? DBNull.Value),
            new SqlParameter("@Status", task.Status.ToString()),
            new SqlParameter("@DueDate", (object?)task.DueDate ?? DBNull.Value)
        };

                using var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_UpdateTask", CommandType.StoredProcedure, parameters);

                if (reader.Read())
                {
                    response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                    response.ResponseMessage = reader["ResponseMessage"].ToString();
                response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
            }

                return response;
            }

            public async Task<DbResponse> UpdateTaskStatus(long id, TaskStatus status)
            {
                var response = new DbResponse();

                SqlParameter[] parameters =
                {
            new SqlParameter("@Id",id),
            new SqlParameter("@Status", status.ToString())
        };

                using var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_UpdateTaskStatus", CommandType.StoredProcedure, parameters);

                if (reader.Read())
                {
                    response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                    response.ResponseMessage = reader["ResponseMessage"].ToString();
                    response.ResponseId = Convert.ToInt64(reader["ResponseId"]);
                }

                return response;
            }

            public async Task<List<ProjectTask>> GetTasksByProjectId(long projectId)
            {
                var list = new List<ProjectTask>();

                SqlParameter[] parameters =
                {
            new SqlParameter("@ProjectId", projectId)
        };

                using var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_GetTasksByProjectId", CommandType.StoredProcedure, parameters);

                while (reader.Read())
                {
                    list.Add(new ProjectTask
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        ProjectId = Convert.ToInt32(reader["ProjectId"]),
                        AssignedToId = Convert.ToInt32(reader["AssignedToId"]),
                        Status = (TaskStatus)(Enum.TryParse<Enums.TaskStatus>( reader["Status"]?.ToString(),out var status) ? status : Enums.TaskStatus.NotStarted),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        DueDate = reader["DueDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["DueDate"])
                    });
                }
                return list;
            }

            public async Task<DbResponse> DeleteTask(long id)
            {
                var response = new DbResponse();
                SqlParameter[] parameters =
                {
                 new SqlParameter("@Id",id)
                };

                using var reader = await SqlHelper.ExecuteReaderAsync(_configuration, _configuration.GetConnectionString("DefaultConnection"),
                    "SP_DeleteTask", CommandType.StoredProcedure, parameters);

                if (reader.Read())
                {
                    response.ResponseCode = Convert.ToInt32(reader["ResponseCode"]);
                    response.ResponseMessage = reader["ResponseMessage"].ToString();
                    response.ResponseId =Convert.ToInt64(reader["ResponseId"]);
                }

                return response;
            }
        }

    }
