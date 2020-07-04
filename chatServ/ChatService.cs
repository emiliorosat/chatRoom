using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace chatServ
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        private ChatCoreEngine mainEngine = new ChatCoreEngine();
        public ChatUser ClientConnect(string username)
        {
            return this.mainEngine.AddNewUser(new ChatUser() { Username = username });
        }

        public List<ChatUser> GetAllUsers()
        {
            return mainEngine.ConnectedUsers;
        }

        public List<ChatMessage> GetNewChatMessages(ChatUser user)
        {
            return mainEngine.GetNewChatMessages(user);
        }

        public void RemoveUser(ChatUser user)
        {
            mainEngine.RemoveUser(user);
        }

        public void SendNewMessage(ChatMessage newMessage)
        {
            mainEngine.AddNewMessage(newMessage);
        }
    }
}
