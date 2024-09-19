using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos.Log;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ILogService
    {
        Task SaveNewLog(string UserName, string Description);
        Task <IEnumerable<GetLogDto>> GetLogsAsync();
        Task <IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);

    }
}