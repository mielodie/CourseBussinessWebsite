using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.ContactAdminData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class ContactAdminConverter
    {
        public ContactAdminDTO EntityToDTO(ContactAdmin contactAdmin)
        {
            return new ContactAdminDTO()
            {
                ID = contactAdmin.ID,
                ContactAt = contactAdmin.ContactAt,
                ContactPersonName = contactAdmin.ContactPersonName,
                IsContacted = contactAdmin.IsContacted,
                PhoneNumber = contactAdmin.PhoneNumber
            };
        }
    }
}
