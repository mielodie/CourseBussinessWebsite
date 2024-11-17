using CourseBusinessWebsite.DataContext;
using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Handle.PaginationHandle;
using CourseBusinessWebsite.Payloads.Converters;
using CourseBusinessWebsite.Payloads.RequestData.ContactAdminRequest;
using CourseBusinessWebsite.Payloads.Responses;
using CourseBusinessWebsite.Payloads.ReturnData.ContactAdminData;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseBusinessWebsite.Services.Implements
{
    public class ContactAdminService : IContactAdminService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<ContactAdminDTO> _responseObjectContactAdminDTO;
        private readonly ContactAdminConverter _converter;

        public ContactAdminService(ResponseObject<ContactAdminDTO> responseObjectContactAdminDTO, ContactAdminConverter converter)
        {
            _context = new AppDbContext();
            _responseObjectContactAdminDTO = responseObjectContactAdminDTO;
            _converter = converter;
        }

        public async Task<ResponseObject<ContactAdminDTO>> CreateContact(RequestCreateContactAdmin request)
        {
            ContactAdmin contactAdmin = new ContactAdmin();
            contactAdmin.ContactAt = DateTime.Now;
            contactAdmin.ContactPersonID = request.ContactPersonID;
            contactAdmin.ContactPersonName = _context.users.SingleOrDefault(x => x.ID == request.ContactPersonID).Username;
            contactAdmin.PhoneNumber = _context.users.SingleOrDefault(x => x.ID == request.ContactPersonID).PhoneNumber;
            contactAdmin.IsContacted = true;

            await _context.contactAdmins.AddAsync(contactAdmin);
            await _context.SaveChangesAsync();
            return _responseObjectContactAdminDTO.ResponseSuccess("Liên hệ ADMIN thành công", _converter.EntityToDTO(contactAdmin));
        }

        public async Task<PageResult<ContactAdminDTO>> GetAll(int pageSize, int pageNumber)
        {
            var list = await _context.contactAdmins.AsNoTracking().ToListAsync();
            var result = list.Select(x => _converter.EntityToDTO(x)).AsQueryable();
            var pageResult = Pagination.GetPagedData(result, pageSize, pageNumber);
            return pageResult;
        }

        public async Task<ResponseObject<ContactAdminDTO>> GetByID(int contactAdminID)
        {
            var contact = await _context.contactAdmins.SingleOrDefaultAsync(x => x.ID == contactAdminID && x.IsContacted == true);
            return _responseObjectContactAdminDTO.ResponseSuccess("Lấy dữ liệu thành công", _converter.EntityToDTO(contact));
        }

        public async Task<string> RemoveContact(int contactAdminID)
        {
            ContactAdmin contactAdmin = await _context.contactAdmins.FirstOrDefaultAsync(x => x.ID == contactAdminID);
            if(contactAdmin is null)
            {
                return "Không tồn tại liên hệ";
            }
            contactAdmin.IsContacted = false;
            _context.contactAdmins.Update(contactAdmin);
            await _context.SaveChangesAsync();
            return "Đã xóa liên hệ";
        }
    }
}
