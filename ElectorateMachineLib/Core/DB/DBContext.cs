using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;

namespace ElectionMachine.Core.DB
{
    public class DBContext : DbContext
    {           
        static readonly string connectionString = ConfigurationManager.AppSettings["baseName"];
        public DBContext()
            : base(new SQLiteConnection() { ConnectionString = connectionString }, true)
            {
            }

        public DbSet<User> Users { get; set; }
        public DbSet<Electorate> Electorates { get; set; }
    }
}
