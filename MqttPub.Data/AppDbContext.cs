using CachedEfCore.Cache;
using CachedEfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using MqttPub.Data.Entities;
using System;

namespace MqttPub.Data
{
    /*
     * Migration command:
     * dotnet ef migrations add InitialMigration -c AppDbContext -p MqttPub.Data
     * dotnet ef migrations add MIGRATION_NAME -c AppDbContext -p MqttPub.Data
     * dotnet ef database update -p Telo.Data -c AppDbContext -p MqttPub.Data
     */

    [DbContext(typeof(AppDbContext))]
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public static AppDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseSqlite("Data Source=" + AppDbContext.DbName);

            return new AppDbContext(optionsBuilder.Options, new DbQueryCacheStore(new MemoryCache(new MemoryCacheOptions())));
        }
    }

    public class AppDbContext : CachedDbContext
    {
        public const string DbName = "mqttpublisher.db";
        public AppDbContext(DbContextOptions options, IDbQueryCacheStore dbQueryCacheStore) : base(options, dbQueryCacheStore)
        {
        }

        public AppDbContext(IDbQueryCacheStore dbQueryCacheStore) : base(dbQueryCacheStore)
        {
        }

        public DbSet<MqttAction> MqttAction { get; set; }
        public DbSet<MqttConnection> MqttConnection { get; set; }
        public DbSet<MqttMessage> MqttMessage { get; set; }
        public DbSet<AppActionEntity> AppAction { get; set; }
        public DbSet<AppActionMqttAction> AppActionMqttAction { get; set; }
    }
}