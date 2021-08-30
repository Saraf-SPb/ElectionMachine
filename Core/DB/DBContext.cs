using System.Configuration;
using System.Data.Entity;

namespace ElectionMachine.Core.DB
{
    class DBContext : DbContext
    {
        private static readonly Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private readonly static KeyValueConfigurationCollection Settings = currentConfig.AppSettings.Settings;
        public DBContext() : base("ElectionMachineConnectionString")
        {
            Database.SetInitializer<DBContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Electorate> Electorates { get; set; }
    }
}
