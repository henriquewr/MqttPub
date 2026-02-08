using CachedEfCore.Cache;
using CachedEfCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using MqttPub.Domain.Entities;
using System;
using System.Reflection;

namespace MqttPub.Infrastructure
{
    /*
     * Migration command:
     * dotnet ef migrations add InitialMigration -c AppDbContext -p MqttPub.Infrastructure
     * dotnet ef migrations add MIGRATION_NAME -c AppDbContext -p MqttPub.Infrastructure
     * dotnet ef database update -c AppDbContext -p MqttPub.Infrastructure
     */

    [DbContext(typeof(AppDbContext))]
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MqttAction> MqttAction => Set<MqttAction>();
        public DbSet<MqttConnection> MqttConnection => Set<MqttConnection>();
        public DbSet<MqttMessage> MqttMessage => Set<MqttMessage>();
        public DbSet<AppActionEntity> AppAction => Set<AppActionEntity>();
        public DbSet<AppActionMqttAction> AppActionMqttAction => Set<AppActionMqttAction>();
    }
}