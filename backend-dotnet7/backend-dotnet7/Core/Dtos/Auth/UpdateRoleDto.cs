using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        public RoleType NewRole { get; set; }
    }

    public enum RoleType {
        ADMIN,
        MANAGER,
        USER
    }
}

// 4 role trong dự án này
// ADMIN MANAGER USER và OWNER - Nhưng vì OWNER cao nhất không hiển thị phía client nên chỉ cần 3 cái