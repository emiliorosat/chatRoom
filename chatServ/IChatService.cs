using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace chatServ
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IChatService
    {
        [OperationContract]
        ChatUser ClientConnect(string username);
        [OperationContract]
        List<ChatUser> GetAllUsers();
        [OperationContract]
        void RemoveUser(ChatUser user);
        [OperationContract]
        List<ChatMessage> GetNewChatMessages(ChatUser user);
        [OperationContract]
        void SendNewMessage(ChatMessage newMessage);


    }

    [DataContract]
    public class ChatUser
    {
        private string username, ipaddress, hostname;

        [DataMember]
        public string Username { get => username; set => username = value; }
        [DataMember]
        public string Ipaddress { get => ipaddress; set => ipaddress = value; }
        [DataMember]
        public string Hostname { get => hostname; set => hostname = value; }
    }

    [DataContract]
    public class ChatMessage
    {
        private ChatUser user;
        private string message;
        private DateTime date;

        [DataMember]
        public ChatUser User { get => user; set => user = value; }
        [DataMember]
        public string Message { get => message; set => message = value; }
        [DataMember]
        public DateTime Date { get => date; set => date = value; }
    }

}
