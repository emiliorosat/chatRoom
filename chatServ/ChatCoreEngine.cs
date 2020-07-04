using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatServ
{
    public class ChatCoreEngine
    {
        private List<ChatUser> connectedUsers = new List<ChatUser>();

        private Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();

        public List<ChatUser> ConnectedUsers { get => connectedUsers; set => connectedUsers = value; }
        public Dictionary<string, List<ChatMessage>> IncomingMessages { get => incomingMessages; set => incomingMessages = value; }

        // metodos

        public ChatUser AddNewUser(ChatUser user)
        {
            var exist =
                from ChatUser e in this.connectedUsers
                where e.Username == user.Username
                select e;
            if(exist.Count() == 0)
            {
                this.connectedUsers.Add(user);
                this.incomingMessages.Add(user.Username, new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        User = user, Date = DateTime.Now, Message = "Bienvenido al Chat"
                    }
                });

                Console.WriteLine("/nuser connected: {0}", user.Username);
                return user;
            }
            else
            {
                return null;
            }

        }

        public void AddNewMessage(ChatMessage message)
        {
            Console.WriteLine(message.User.Username + " Says: " + message.Message + " at "+ message.Date);

            foreach (var user in this.connectedUsers)
            {
                if (!message.User.Username.Equals(user.Username))
                {
                    incomingMessages[user.Username].Add(message);
                }
            }
        }

        public List<ChatMessage> GetNewChatMessages(ChatUser user)
        {
            List<ChatMessage> newMessage = incomingMessages[user.Username];
            incomingMessages[user.Username] = new List<ChatMessage>();

            if (newMessage.Count > 0)
            {
                return newMessage;
            }
            else
            {
                return null;
            }
        }

        public void RemoveUser(ChatUser user)
        {
            this.connectedUsers.RemoveAll(u => u.Username == user.Username);
        }

    }
}
