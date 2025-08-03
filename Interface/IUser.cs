using ProjectManagementAPI.Models;

namespace ProjectManagementAPI.Interface
{
    public interface IUser
    {
        //Task<DbResponse> CreateUser(User request);
        Task<List<User>> GetAllUser();
        Task<DbResponse> UpdateUser(long Id, string UserName, string Email);
        Task<DbResponse> UpdateUserRole(long Id, UpdateRoleUpdateDTO request);
        Task<DbResponse> DeleteUser(long Id);
    }
}
