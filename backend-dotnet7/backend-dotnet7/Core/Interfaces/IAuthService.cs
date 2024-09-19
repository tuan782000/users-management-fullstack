using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.General;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDto>SeedRolesAsync();        
        Task<GeneralServiceResponseDto>RegisterAsync(RegisterDto registerDto);
        Task<LoginServiceResponseDto>LoginAsync(LoginDto loginDto);
        Task<GeneralServiceResponseDto>UpdateRolesAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
        Task<LoginServiceResponseDto>MeAsync(MeDto meDto);
        Task<IEnumerable<UserInfoResult>> GetUsersListAsync();
        Task<UserInfoResult> GetUserDetailsByUserName(string userName);
        Task<IEnumerable<string>> GetUsernameListAsync();
    }
}