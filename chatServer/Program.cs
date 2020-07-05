using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.Security.Principal;
using System.Windows.Forms;

namespace chatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();

            if (obj.IsRunningAdmin())
            {
                obj.RunServer();
            }
            else
            {
                MessageBox.Show("NO Esta corriendo como administrador",
                                "Alerta", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
            }
        }

        private void RunServer()
        {
            using(ServiceHost host = new ServiceHost(typeof(chatServ.ChatService)))
            {
                host.Open();
                Console.WriteLine("Servidor Iniciado");

                Console.WriteLine("Presione Cualquier tecla para salir...");
                Console.ReadLine();
                host.Close();
            }
        }

        private bool IsRunningAdmin()
        {
            bool isAdmin = false;
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

            if(currentIdentity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(currentIdentity);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                principal = null;
            }
            return isAdmin;

        }
    }
}
