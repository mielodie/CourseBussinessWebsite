using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.RoleData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly RoleConverter _converter;
        private ResponseObject<RoleDTO> _responseObjectRoleDTO;
        public async Task<ResponseObject<RoleDTO>> CreateRole(string roleName)
        {
            Role role = new Role();
            
            role.RoleName = roleName;
            await _context.roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return _responseObjectRoleDTO.ResponseSuccess("Thêm quyền hạn thành công", _converter.EntityToDTO(role));
        }

        public async Task<PageResult<RoleDTO>> GetAll(int pageSize, int pageNumber)
        {
            var listRole = _context.roles.AsNoTracking().AsQueryable();
            var result = listRole.Select(x => _converter.EntityToDTO(x));
            var listPage = Pagination.GetPagedData(result, pageSize, pageNumber);
            return listPage;
        }

        public async Task<ResponseObject<RoleDTO>> GetByID(int roleID)
        {
            Role role = await _context.roles.SingleOrDefaultAsync(x => x.ID == roleID);
            if (role is null)
            {
                return _responseObjectRoleDTO.ResponseError(StatusCodes.Status404NotFound, "Quyền hạn không tồn tại", null);
            }
            return _responseObjectRoleDTO.ResponseSuccess("Quyền hạn có ID = " + roleID + "...", _converter.EntityToDTO(role));
        }

        public async Task<string> RemoveRole(int roleID)
        {
            Role role = await _context.roles.SingleOrDefaultAsync(x => x.ID == roleID);
            if (role is null)
            {
                return "Không tìm thấy quyền hạn";
            }
            _context.Remove(role);
            await _context.SaveChangesAsync();
            return "Xóa quyền hạn thành công";
        }

        public async Task<ResponseObject<RoleDTO>> UpdateRole(int roleID, string roleName)
        {
            Role role = await _context.roles.SingleOrDefaultAsync(x => x.ID == roleID);
            if(role is null)
            {
                return _responseObjectRoleDTO.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy quyền hạn", null);
            }
            role.RoleName = roleName;
            _context.Update(role);
            await _context.SaveChangesAsync();
            return _responseObjectRoleDTO.ResponseSuccess("Sửa quyền hạn thành công", _converter.EntityToDTO(role));
        }
    }
}
