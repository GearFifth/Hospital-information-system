using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.MainUI.Users
{
    public class UserRepository
    {
        private const string UsersFilePath = "..\\..\\..\\MainUI\\Users\\users.csv";
        public  List<User> Users = new();
        public  Serializer<User> UserSerializer = new();

        public UserRepository()
        {
            Users = UserSerializer.fromCSV(UsersFilePath);
        }

        public  void SaveRepository()
        {
            UserSerializer.toCSV(UsersFilePath, Users);
        }

        public  User? GetUser(string username)
        {
            return Users.FirstOrDefault(user => user.Username == username);
        }

        public  bool DoesUsernameAlreadyExist(string username)
        {
            return Users.Any(user => user.Username == username);
        }
        public  bool IsUsernameValidForModification(string oldUsername, string newUsername)
        {
            return Users.All(user => newUsername != user.Username || oldUsername == user.Username);
        }

        public  void EditUser(string oldUsername, string newUsername, string password, User.UserRole role)
        {
            User? user = GetUser(oldUsername);
            user.Username = newUsername;
            user.Password = password;
            user.Role = role;
            SaveRepository();
        }

        public  void DeleteUser(string username)
        {
            User user = GetUser(username)!;
            Users.Remove(user);
            SaveRepository();
        }

        internal List<string> GetUsernamesOfRole(string roleName)
        {
            List<string> usernames = new List<string>();

            foreach (User user in Users)
            {
                if (user.Role.ToString() == roleName)
                {
                    usernames.Add(user.Username);
                }
            }
            return usernames;
        }
    }
}
