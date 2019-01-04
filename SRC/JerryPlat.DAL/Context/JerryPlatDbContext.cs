using JerryPlat.DAL.Migrations;
using JerryPlat.Models;
using JerryPlat.Utils.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace JerryPlat.DAL.Context
{
    public class JerryPlatDbContext : DbContext
    {
        public JerryPlatDbContext():this("JerryPlatDbContext"){}

        public JerryPlatDbContext(string strName)
            : base("name=" + strName)
        {
#if DEBUG
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Map();
        }

        #region Roles Tables
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> NavigationRoles { get; set; }
        #endregion

        #region Owin Tables
        public DbSet<OwinToken> OwinTokens { get; set; }
        public DbSet<WxTicket> WxTickets { get; set; }
        public DbSet<OwinConfig> OwinConfigs { get; set; }
        #endregion
        
        #region Db Operation Tables
        public DbSet<DbBackupRestore> DbBackupRestores { get; set; }
        #endregion

        #region System Setting Tables
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        #endregion

        #region Commons
        public DbSet<Banner> Banners { get; set; }
        #endregion

        public DbSet<Member> Members { get; set; }
    }
}