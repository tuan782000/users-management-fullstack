using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Constants
{
    // This class will be used to avoid typing errors - Lớp này sẽ được sử dụng để avoid typing errors
    public static class StaticUserRoles
    {
        public const string OWNER = "OWNER";
        public const string ADMIN = "ADMIN";
        public const string MANAGER = "MANAGER";
        public const string USER = "USER";

        public const string OwnerAdmin = "OWNER,ADMIN";
        public const string OwnerAdminManager = "OWNER,ADMIN,MANAGER";
        public const string OwnerAdminManagerUser = "OWNER,ADMIN,MANAGER,USER";
    }
}