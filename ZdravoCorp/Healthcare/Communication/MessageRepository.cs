using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Healthcare.Communication
{
    public class MessageRepository
    {
        public const string MessagesFilePath = "..\\..\\..\\Healthcare\\Communication\\messages.json";
        public ObservableCollection<Message> Messages = new();

        public MessageRepository()
        {
            if (!File.Exists(MessagesFilePath)) return;

            string json = File.ReadAllText(MessagesFilePath);
            Messages = JsonConvert.DeserializeObject<ObservableCollection<Message>>(json);
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(Messages, Formatting.Indented);
            File.WriteAllText(MessagesFilePath, json);
        }

        public void Add(Message message)
        {
            if (message.Id == -1)
            {
                AssignId(message);
            }
            Messages.Add(message);
            Save();
        }

        private void AssignId(Message message)
        {
            do
            {
                message.Id = GenerateId();
            } while (ContainsId(message.Id));
        }

        private bool ContainsId(int id)
        {
            foreach (Message message in Messages)
            {
                if (id == message.Id) return true;
            }

            return false;
        }

        private int GenerateId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 99999);
        }

        public ObservableCollection<Message> GetAllUserMessages(string username)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();

            foreach (Message message in Messages)
            {
                if (message.SenderUsername == username || message.ReceiverUsername == username)
                {
                    messages.Add(message);
                }
            }
            if (messages.Count == 0) return null;

            return messages;
        }

        public ObservableCollection<Message> GetSentUserMessages(string loggedUserUsername)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();

            foreach (Message message in Messages)
            {
                if (message.SenderUsername == loggedUserUsername)
                {
                    messages.Add(message);
                }
            }
            if (messages.Count == 0) return null;

            return messages;
        }

        public ObservableCollection<Message> GetReceivedMessages(string loggedUserUsername)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();

            foreach (Message message in Messages)
            {
                if (message.ReceiverUsername == loggedUserUsername)
                {
                    messages.Add(message);
                }
            }
            if (messages.Count == 0) return null;

            return messages;
        }
    }
}
