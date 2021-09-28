using ElectionMachine;
using ElectionMachine.Core.DB;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace ElectorateMachineServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceAddress = ConfigurationManager.AppSettings["address"];
            var serviceName = "ElectorateMachineServer";
            try
            {
                var host = new ServiceHost(typeof(TransferObject), new Uri($"net.tcp://{serviceAddress}/{serviceName}"));
                var serverBinding = new NetTcpBinding();
                serverBinding.Security.Mode = SecurityMode.None;
                serverBinding.MaxReceivedMessageSize = int.MaxValue;
                serverBinding.MaxBufferPoolSize = int.MaxValue;
                host.AddServiceEndpoint(typeof(ITransferObject), serverBinding, "");
                host.Open();
                Console.WriteLine($"{DateTime.Now}: Service started...");
                Console.WriteLine($"{DateTime.Now}: Address: {serviceAddress}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DBContext db = new DBContext();
            try
            {
                db.Users.Count();
                Console.WriteLine($"{DateTime.Now}: Database connection established. Path: "
                    + $"{ConfigurationManager.AppSettings["baseName"]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }
    }
}
