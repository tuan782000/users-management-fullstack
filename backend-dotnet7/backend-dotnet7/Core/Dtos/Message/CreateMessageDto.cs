using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Dtos.Message
{
    public class CreateMessageDto
    {
        public string ReceiverUserName { get; set; }
        public string Text { get; set; }
    }
}