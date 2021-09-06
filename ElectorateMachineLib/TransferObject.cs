using ElectionMachine.Core.DB;
using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace ElectionMachine
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class TransferObject : ITransferObject
    {
        DBContext db;
        public int GetSum(int a, int b)
        {
            return a + b;
        }
        public int GetMultiPly(int a, int b)
        {
            return a * b;
        }

        public (User, string) Auth(string login, string password)
        {
            try
            {
                db = new DBContext();
                return (db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault(), "OK");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public string AddElectorat(string fio, string phone, int userId)
        {
            try
            {
                db = new DBContext();

                ElectorateWithUserName electorateWithUserName = new ElectorateWithUserName();

                List<Electorate> electorates = db.Electorates.ToList();

                bool f = false;


                foreach (var el in electorates)
                {
                    if (el.FIO.Equals(fio, StringComparison.OrdinalIgnoreCase))
                    {
                        var temp = db.Users.FirstOrDefault(u => u.Id == el.UserId);

                        electorateWithUserName.FIO = el.FIO;
                        electorateWithUserName.CreateDate = el.CreateDate;
                        electorateWithUserName.UserName = temp.Name;
                        f = true;
                    }
                }


                //var electorate = db.Electorates.Where(e => e.FIO.ToLower() == fio.ToLower()).FirstOrDefault();

                //var electorate = (from e in db.Electorates
                //                  //join u in db.Users on e.UserId equals u.Id
                //                  where e.FIO.Equals(fio, StringComparison.OrdinalIgnoreCase)
                //                  select e).FirstOrDefault()
                //                 //select new { FIO = e.FIO, Phone = e.Phone, CreateDate = e.CreateDate, UserName = u.Name })                                  
                //                 ;                

                //var electorate = db.Electorates.Where(e => e.FIO.Equals(fio, StringComparison.OrdinalIgnoreCase)).Join(db.Users,
                //    e => e.UserId,
                //    u => u.Id,
                //    (e, u) => new
                //    {
                //        e.FIO,
                //        e.Phone,
                //        e.CreateDate,
                //        u.Name
                //    }).FirstOrDefault();

                if (f == false)
                {
                    //SQLiteConnection.CreateFile(baseName);

                    //SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                    //using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                    //{
                    //    connection.ConnectionString = "Data Source = " + baseName;
                    //    connection.Open();

                    //    using (SQLiteCommand command = new SQLiteCommand(connection))
                    //    {
                    //        command.CommandText = $@"INSERT INTO electorates (fio, phone, userid, createdate) 
                    //            VALUES ('{fio}','{phone}',{userId}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                    //        command.CommandType = CommandType.Text;
                    //        command.ExecuteNonQuery();
                    //    }
                    //}

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
                    return $"{fio} был добавлен оператором {electorateWithUserName.UserName} " +
                        $"{electorateWithUserName.CreateDate}";
                }

                //electorate = db.Electorates.Where(e => e.FIO == fio).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "Ошибка при работе с базой данных." + Environment.NewLine + ex.Message + Environment.NewLine
                    + ex.InnerException;
            }
        }

        public List<ElectorateWithUserName> GetAllElectorateList()
        {
            try
            {
                db = new DBContext();

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
                        new ElectorateWithUserName()
                        {
                            FIO = i.FIO,
                            Phone = i.Phone,
                            UserName = i.UserName,
                            CreateDate = i.CreateDate
                        }
                        );
                }
                return ElectorateWithUserName;
            }
            catch (Exception ex)
            {
                return new List<ElectorateWithUserName>();
            }
        }

        public int Amount()
        {
            db = new DBContext();
            return db.Users.Count();
        }

        public Electorate GetElectorate(int id)
        {
            db = new DBContext();
            return db.Electorates.Where(e => e.Id == id).FirstOrDefault();
        }

        public List<User> GetAllUsers()
        {
            db = new DBContext();
            return db.Users.ToList();
        }

        public string SaveUsers(List<User> usersLIst)
        {
            try
            {
                db = new DBContext();
                User tempUser = new User();
                List<User> users = db.Users.ToList();
                foreach (User u in usersLIst)
                {
                    tempUser = null;
                    if (u.Id > 0)
                        tempUser = users.FirstOrDefault(i => i.Id == u.Id);
                    if (tempUser == null)
                    {
                        db.Users.Add(new User
                        {
                            Login = u.Login,
                            Name = u.Name,
                            Password = u.Password,
                            IsAdmin = u.IsAdmin
                        });
                    }
                    else
                    {
                        db.Entry(tempUser).CurrentValues.SetValues(u);
                        tempUser = u;
                    }
                }
                db.SaveChanges();
                return "Сохранено";
            }
            catch (Exception ex)
            {
                return ex.Message + Environment.NewLine + ex.InnerException;
            }
        }
    }
}
