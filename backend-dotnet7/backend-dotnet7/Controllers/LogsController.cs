using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        // sử dụng để lưu trữ tham chiếu đến service
        // chỉ thực hiện hành động liên quan đến ghi hoặc đọc log.
        private readonly ILogService _logService;

        // constructor của class LogsController
        public LogsController(ILogService logService)
        {
            _logService = logService; 
            // để truy cập các phương thức của service mà không cần truyền lại trong các phương thức đó.
        }

        // OwnerAdmin sự kết hợp OWNER và ADMIN ở trong core constants
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetLogs()
        {
            var logs = await _logService.GetLogsAsync();
            return Ok(logs);
        }

        [HttpGet]
        [Route("mine")]
        [Authorize] // Bất kỳ ai đã đăng nhập vào không cần xét quyền thì đều dùng được
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetMyLogs()
        {
            var logs = await _logService.GetMyLogsAsync(User);
            return Ok(logs);
        }
    }
}