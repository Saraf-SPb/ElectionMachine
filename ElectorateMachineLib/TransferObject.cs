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
                List<Electorate> electorates = db.Electorates.ToList();

                /* 
                 * Из-за особенности работы SQLite с кириллицей приходится делать костыль.
                 * Переводя в LINQ-запросе сравниваемые значения в нижний регистр, на сторое SQLite
                 * в запросе все равно присутствуют буквы разного регистра. 
                 * С латиницей работает нормально, проблема наблюдается только с кириллицей.
                */
                foreach (var el in electorates)
                {
                    if (el.FIO.Equals(fio, StringComparison.OrdinalIgnoreCase))
                    {
                        var tempUser = db.Users.FirstOrDefault(u => u.Id == el.UserId);

                        return $"{el.FIO} был добавлен оператором {tempUser.Name} "
                            + $"{el.CreateDate}";
                    }
                }

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
            catch (Exception ex)
            {
                return "Ошибка при работе с базой данных." + Environment.NewLine
                    + ex.Message + Environment.NewLine
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
                        });
                }
                return ElectorateWithUserName;
            }
            catch
            {
                return new List<ElectorateWithUserName>();
            }
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

                /* В цикле делается сопоставление записей из DGV с данными в БД по Id.
                 * Если Id в DGV отсутствует, то запись добавляется в БД.
                 * Удаление отсутствует по требованию заказчика.
                 */
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
