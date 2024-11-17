using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.RequestData.ContactAdminRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.ContactAdminData;

namespace CourseBusinessWebsite.Services.Interfaces
{
    public interface IContactAdminService
    {
        Task<ResponseObject<ContactAdminDTO>> CreateContact(RequestCreateContactAdmin request);
        Task<string> RemoveContact(int contactAdminID);
        Task<PageResult<ContactAdminDTO>> GetAll(int pageSize, int pageNumber);
        Task<ResponseObject<ContactAdminDTO>> GetByID(int contactAdminID);
    }
}
