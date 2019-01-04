using JerryPlat.DAL.Context;
using JerryPlat.DAL.Migrations;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.DAL
{
    public static class DbContextHelper
    {
        public static void Init()
        {
            using (JerryPlatDbContext context = new JerryPlatDbContext())
            {
                if (context.Database.Exists())
                {
                    Database.SetInitializer<JerryPlatDbContext>(null);
                    var migrator = new DbMigrator(new Configuration());
                    // if (doseed || !context.Database.CompatibleWithModel(false))
                    if (migrator.GetPendingMigrations().Any())
                        migrator.Update();
                }
                else
                {
                    Database.SetInitializer(new CreateAndMigrateDatabaseInitializer<Configuration>());
                }

                context.Database.Initialize(false);

                SystemConfigModel.Reset(context.SystemConfigs.ToDictionary(o => o.Name, o => o.Config));
            }
        }
    }
}
