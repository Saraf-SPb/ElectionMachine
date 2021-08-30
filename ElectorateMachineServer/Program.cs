using ElectionMachine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ElectorateMachineServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var serviceAddress = "192.168.1.17:10001";
            var serviceAddress = ConfigurationManager.AppSettings["address"];
            var serviceName = "ElectorateMachineServer";

            var host = new ServiceHost(typeof(TransferObject), new Uri($"net.tcp://{serviceAddress}/{serviceName}"));
            var serverBinding = new NetTcpBinding();
            serverBinding.Security.Mode = SecurityMode.None;
            host.AddServiceEndpoint(typeof(ITransferObject), serverBinding, "");
            host.Open();
            
            Console.WriteLine($"{DateTime.Now}: Service started...");
            Console.WriteLine($"{DateTime.Now}: Address: {serviceAddress}");
            Console.ReadKey();
        }
    }
}
