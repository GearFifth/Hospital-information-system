using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.Communication
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }

        public Message(DateTime time, string content, string senderUsername, string receiverUsername)
        {
            Id = -1;
            Time = time;
            Content = content;
            SenderUsername= senderUsername;
            ReceiverUsername= receiverUsername;
        }
    }
}
