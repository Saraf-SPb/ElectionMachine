using ElectionMachine.Core.DB;
using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectionMachine.Core
{
    public static class DBHelper
    {
        private static DBContext db = new DBContext();
        public static User Auth(string login, string password)
        {
            try
            {
                return db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault(); ;
            }
            catch
            {
                return null;
            }
        }

        public static string AddElectorat(string fio, string phone)
        {            
            try
            {
                var electorate = db.Electorates.Where(e => e.FIO.ToLower() == fio.ToLower()).Join(db.Users,
                    e => e.UserId,
                    u => u.Id,
                    (e, u) => new
                    {
                        e.FIO,
                        Phone = e.Phone,
                        CreateDate = e.CreateDate,
                        Name = u.Name
                    }).FirstOrDefault();
                if (electorate == null)
                {
                    db.Electorates.Add(new Electorate()
                    {
                        FIO = fio,
                        Phone = phone,
                        CreateDate = DateTime.Now,
                        //UserId = Program.GlobalUser.Id
                    });
                    db.SaveChanges();
                    return $"{fio} успешно добавлен.";
                }
                else
                {
                    return $"{fio} был добавлен оператором {electorate.Name} {electorate.CreateDate}";
                }

                //electorate = db.Electorates.Where(e => e.FIO == fio).FirstOrDefault();
            }
            catch
            {
                return "Ошибка при работе с базой данных";
            }
        }

        public static List<ElectorateWithUserName> GetAllElectorateList()
        {
            var list = db.Electorates.Join(db.Users,
                            e => e.UserId,
                            u => u.Id,
                            (e, u) => new
                            {                                
                                e.FIO,
                                e.Phone,
                                UserName = u.Name,
                                e.CreateDate
                            }
                            ).ToList();
            List<ElectorateWithUserName> ElectorateWithUserName = new List<ElectorateWithUserName>();
            foreach (var i in list)
            {
                ElectorateWithUserName.Add(
                    new ElectorateWithUserName() { 
                        FIO = i.FIO, 
                        Phone = i.Phone, 
                        UserName = i.UserName,
                        CreateDate = i.CreateDate
                    }
                    );
            }
            return ElectorateWithUserName;
        }
    }
}
