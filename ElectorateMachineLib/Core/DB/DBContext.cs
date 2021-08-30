using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;

namespace ElectionMachine.Core.DB
{
    class DBContext : DbContext
    {
        //public DBContext() : base("ElectionMachineConnectionString")
        //{
        //    Database.SetInitializer<DBContext>(null);
        //}
        private static readonly string baseName = ConfigurationManager.AppSettings["baseName"];
        //static string connectionString = @"Data Source=F:\temp\Database.db";
        static string connectionString = baseName;
        public DBContext()
            : base(new SQLiteConnection() { ConnectionString = connectionString }, true)
            {
            }

        public DbSet<User> Users { get; set; }
        public DbSet<Electorate> Electorates { get; set; }
    }
}
