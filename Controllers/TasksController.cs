using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Interface;
using ProjectManagementAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("GetTasksByProjectId/{projectId}")]
    [Authorize(Roles = "Admin,ProjectManager,Developer")]
    public async Task<IActionResult> GetTasks(long projectId)
    {
        try
        {
            var result = await _taskService.GetTasksByProjectId(projectId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("CreateTask{projectId}/task")]
    [Authorize(Roles = "Admin,ProjectManager,Developer")]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] ProjectTask task)
    {
        try
        {
            task.ProjectId = projectId;
            var result = await _taskService.CreateTask(task);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UpdateTask{id}/task")]
    [Authorize(Roles = "Admin,ProjectManager,Developer")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] ProjectTask task)
    {
        try
        {
            task.Id = id;
            var result = await _taskService.UpdateTask(task);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UpdateTaskStatus/{id}/status")]
    [Authorize(Roles = "Admin,ProjectManager,Developer")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TaskStatus status)
    {
        try
        {
            var result = await _taskService.UpdateTaskStatus(id, status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("DeleteTask/{id}")]
    [Authorize(Roles = "Admin,ProjectManager")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var result = await _taskService.DeleteTask(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
