using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Healthcare.Communication
{
    public static class MessageService
    {
        private static MessageRepository MessageRepository = new();
        public static void Add(Message message)
        {
            MessageRepository.Add(message);
        }

        public static ObservableCollection<Message> GetAllUserMessages(string username)
        {
            return MessageRepository.GetAllUserMessages(username);
        }

        public static ObservableCollection<Message> GetSentUserMessages(string loggedUserUsername)
        {
            return MessageRepository.GetSentUserMessages(loggedUserUsername);
        }

        public static ObservableCollection<Message> GetReceivedUserMessages(string loggedUserUsername)
        {
            return MessageRepository.GetReceivedMessages(loggedUserUsername);
        }
    }
}
