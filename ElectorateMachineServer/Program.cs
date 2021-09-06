using ElectionMachine;
using ElectionMachine.Core.DB;
using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace ElectorateMachineServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var serviceAddress = "192.168.1.17:10001";
            var serviceAddress = ConfigurationManager.AppSettings["address"];
            var serviceName = "ElectorateMachineServer";
            try
            {
                var host = new ServiceHost(typeof(TransferObject), new Uri($"net.tcp://{serviceAddress}/{serviceName}"));
                var serverBinding = new NetTcpBinding();
                serverBinding.Security.Mode = SecurityMode.None;
                host.AddServiceEndpoint(typeof(ITransferObject), serverBinding, "");
                host.Open();
                Console.WriteLine($"{DateTime.Now}: Service started...");
                Console.WriteLine($"{DateTime.Now}: Address: {serviceAddress}");


                //string temp = AddElectorat("Сумина ОльгА", "", 2);
                //Console.WriteLine(temp);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DBContext db = new DBContext();
            try
            {
                db.Users.Count();
                Console.WriteLine($"{DateTime.Now}: Database connection established. Path: " +
                    $"{ConfigurationManager.AppSettings["baseName"]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();

            string AddElectorat(string fio, string phone, int userId)
            {
                try
                {
                    db = new DBContext();

                    ElectorateWithUserName electorateWithUserName = new ElectorateWithUserName();

                    List<Electorate> electorates = db.Electorates.ToList();

                    foreach (var el in electorates)
                    {
                        if (el.FIO.Equals(fio, StringComparison.OrdinalIgnoreCase))
                        {
                            var temp = db.Users.FirstOrDefault(u => u.Id == el.UserId);

                            electorateWithUserName.FIO = el.FIO;
                            //electorateWithUserName.CreateDate = el.CreateDate;
                            //electorateWithUserName.UserName = temp.Name;
                            return el.FIO;
                        }
                    }

                    if (electorateWithUserName == null)
                    {
                        db.Electorates.Add(new Electorate()
                        {
                            FIO = fio,
                            Phone = phone,
                            CreateDate = DateTime.Now,
                            UserId = userId
                        });
                        db.SaveChanges();
                        return $"{fio} успешно добавлен.";
                    }
                    else
                    {
                        //return $"{fio} был добавлен оператором {electorate.Name} {electorate.FirstOrDefault().CreateDate}";
                        return $"{fio} был добавлен ранее";
                    }

                    //electorate = db.Electorates.Where(e => e.FIO == fio).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return "Ошибка при работе с базой данных." + Environment.NewLine + ex.Message + Environment.NewLine
                        + ex.InnerException;
                }
            }
        }
    }
}
