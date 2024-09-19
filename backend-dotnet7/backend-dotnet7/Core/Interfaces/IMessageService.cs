using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.Message;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto);
        Task<IEnumerable<GetMessageDto>> GetMessageAsync();
        Task<IEnumerable<GetMessageDto>> GetMyMessageAsync(ClaimsPrincipal User);
    }
}