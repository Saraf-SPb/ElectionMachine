using ElectionMachine.Core.DB;
using ElectionMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ElectionMachine
{
    static class Program
    {
        public static User GlobalUser = null;
        public static ITransferObject service = null;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var serviceAddress = "192.168.1.17:10001";
            var serviceAddress = ConfigurationManager.AppSettings["address"];
            var serviceName = "ElectorateMachineServer";

            try
            {
                Uri tcpUri = new Uri($"net.tcp://{serviceAddress}/{serviceName}");
                EndpointAddress address = new EndpointAddress(tcpUri);
                NetTcpBinding clientBinding = new NetTcpBinding();
                clientBinding.Security.Mode = SecurityMode.None;
                ChannelFactory<ITransferObject> factory = new ChannelFactory<ITransferObject>(clientBinding, address);
                service = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.Auth());
        }
    }
}
