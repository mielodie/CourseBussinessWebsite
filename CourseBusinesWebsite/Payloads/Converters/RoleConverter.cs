using CourseBusinessWebsite.Entities;
using CourseBusinessWebsite.Payloads.ReturnData.RoleData;

namespace CourseBusinessWebsite.Payloads.Converters
{
    public class RoleConverter
    {
        public RoleDTO EntityToDTO(Role role)
        {
            return new RoleDTO
            {
                ID = role.ID,
                RoleName = role.RoleName
            };
        }
    }
}
