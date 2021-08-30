using ElectionMachine.Core.DB;
using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
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

        public User Auth(string login, string password)
        {
            try
            {
                db = new DBContext();
                return db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string AddElectorat(string fio, string phone, int userId, string baseName)
        {
            try
            {
                db = new DBContext();


                //var electorate = db.Electorates.Where(e => e.FIO.ToLower() == fio.ToLower()).FirstOrDefault();

                var electorate = db.Electorates.Where(e => e.FIO.ToLower() == fio.ToLower()).Join(db.Users,
                    e => e.UserId,
                    u => u.Id,
                    (e, u) => new
                    {
                        e.FIO,
                        e.Phone,
                        e.CreateDate,
                        u.Name
                    }).FirstOrDefault();

                if (electorate == null)
                {
                    //SQLiteConnection.CreateFile(baseName);

                    SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                    using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                    {
                        connection.ConnectionString = "Data Source = " + baseName;
                        connection.Open();

                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandText = $@"INSERT INTO electorates (fio, phone, userid, createdate) 
                                VALUES ('{fio}','{phone}',{userId}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }

                    //db.Electorates.Add(new Electorate()
                    //{
                    //    FIO = fio,
                    //    Phone = phone,
                    //    CreateDate = DateTime.Now,
                    //    UserId = userId
                    //});
                    //db.SaveChanges();
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
            return db.Electorates.Where(e => e.ID == id).FirstOrDefault();
        }
    }
}
