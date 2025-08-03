using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Models.Registration;
using ProjectManagementAPI.Services;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;   
        }
       
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var result = await _userService.GetAllUser();

                if (result.Count==0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Get all user", error = ex.Message });
            }
        }
        [HttpPut("{Id}/role")]
        public async Task<IActionResult>UpdateUserRole(long Id, [FromBody] UpdateRoleUpdateDTO request)
        {
            var result = await _userService.UpdateUserRole(Id,request);
            if (result == null || result.ResponseCode != 200)
                return BadRequest(new { message = result?.ResponseMessage ?? "Update User Role failed" });
            return Ok(result);

        }

        [HttpGet("{Id}/{UserName}/{Email}")]
        public async Task<IActionResult> UpdateUser(long Id, string UserName,string Email)
        {
            var result = await _userService.UpdateUser(Id, UserName, Email);
            if (result == null || result.ResponseCode != 200)
                return BadRequest(new { message = result?.ResponseMessage ?? "Update User Role failed" });
            return Ok(result);

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            var result = await _userService.DeleteUser(Id);
            if (result == null || result.ResponseCode != 200)
                return BadRequest(new { message = result?.ResponseMessage ?? "Update User Role failed" });
            return Ok(result);

        }

    }
}
