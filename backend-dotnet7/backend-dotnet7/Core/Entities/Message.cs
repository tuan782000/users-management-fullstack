using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Entities
{
    public class Message:BaseEntity<long>
    {
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set;}
        public string Text { get; set; }
    }
}