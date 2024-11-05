using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.CourseData;
using CourseBusinessWebsite.Payloads.ReturnData.RoleData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseObject<RoleDTO>> CreateRole(string roleName);
        Task<ResponseObject<RoleDTO>> UpdateRole(int roleID, string roleName);
        Task<string> RemoveRole(int roleID);
        Task<PageResult<RoleDTO>> GetAll(int pageSize, int pageNumber);
        Task<ResponseObject<RoleDTO>> GetByID(int roleID);
    }
}
