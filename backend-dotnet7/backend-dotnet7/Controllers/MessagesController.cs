using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        // constructor của class MessagesController
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService; 
            // để truy cập các phương thức của service mà không cần truyền lại trong các phương thức đó.
        }

        // Route -> Create a new message to send to another user 
        [HttpPost]
        [Route("create")]
        [Authorize] // mọi user bất kể role đều dùng được
        // CreateNewMessage nhận vào 1 tham số 
        // [FromBody] là một attribute của ASP.NET Core, cho biết rằng dữ liệu cho tham số
        // CreateMessageDto createMessageDto là một Data Transfer Object (DTO). DTO này có thể chứa các thuộc tính
        // ReceiverUserName - là người nhận và Text là tin nhắn đó
        public async Task<IActionResult> CreateNewMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var result = await _messageService.CreateNewMessageAsync(User, createMessageDto);
            if (result.IsSucceed)
                return Ok(result.Message);

            return StatusCode(result.StatusCode, result.Message);
        }

        // Route -> Get All Messages for current user, Either as Sender or as Receiver
        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMyMessages()
        {
            var messages = await _messageService.GetMyMessagesAsync(User);
            return Ok(messages);
        }

        // Route -> Get all messages With Owner access and Admin access
        // Nhận tất cả tin nhắn Với quyền truy cập của Chủ sở hữu và quyền truy cập của Quản trị viên
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            return Ok(messages);
        }
    }
}