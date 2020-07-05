using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceModel;
using chatServ;

namespace ChatCliente
{
    public partial class MainForm : Form
    {
        private ChannelFactory<IChatService> remoteFactory;
        private IChatService remoteProxy;
        private ChatUser clientUser;
        private bool isConnected = false;

        public MainForm()
        {
            InitializeComponent();
            LbStatus.Text = "Desconectado";
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LbStatus.Text = "Conectando...";
                LoginForm loginDialog = new LoginForm();
                loginDialog.ShowDialog(this);
                if (!String.IsNullOrEmpty(loginDialog.UserName))
                {
                    remoteFactory = new ChannelFactory<IChatService>("ChatConfig");
                    remoteProxy = remoteFactory.CreateChannel();
                    clientUser = remoteProxy.ClientConnect(loginDialog.UserName);
                    if(clientUser != null)
                    {
                        timerMessage.Enabled = true;
                        timerUser.Enabled = true;
                        iniciarSesionToolStripMenuItem.Enabled = false;
                        btnSend.Enabled = true;
                        txtMessage.Enabled = true;
                        isConnected = true;
                        LbStatus.Text = "Conectado Como: " + clientUser.Username;
                    }
                }
                else
                {
                    LbStatus.Text = "Desconectado";
                }
            }catch(Exception err)
            {
                MessageBox.Show("Ha ocurrido un error\nNo Se Puede Conectar\nMensaje: " + err.Message,
                    "Error Faltal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerUser_Tick(object sender, EventArgs e)
        {
            List<ChatUser> listUsers = remoteProxy.GetAllUsers();
            listBox1.DataSource = listUsers;
        }

        private void timerMessage_Tick(object sender, EventArgs e)
        {
            List<ChatMessage> messages = remoteProxy.GetNewChatMessages(clientUser);
            if(messages != null)
            {
                foreach (var message in messages)
                {
                    InsertMessage(message);
                }
            }
        }

        private void InsertMessage(ChatMessage message)
        {
            txtChat.AppendText("\n\n" + message.User.Username + " Dice (" + message.Date + "):\n" + message.Message + "\n\n");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMessage.Text))
            {
                ChatMessage newMessage = new ChatMessage()
                {
                    Date = DateTime.Now,
                    Message = txtMessage.Text,
                    User = clientUser
                };
                remoteProxy.SendNewMessage(newMessage);
                InsertMessage(newMessage);
                txtMessage.Text = String.Empty;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isConnected)
            {
                remoteProxy.SendNewMessage(new ChatMessage()
                {
                    Date = DateTime.Now,
                    Message = "Cerrando Sesion",
                    User = clientUser
                });

                remoteProxy.RemoveUser(clientUser);
            }
        }
    }
}
