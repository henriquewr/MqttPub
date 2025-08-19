using Android.Content;
using CachedEfCore.Cache;
using CachedEfCore.Cache.Helper;
using CachedEfCore.ExpressionKeyGen;
using CachedEfCore.Interceptors;
using CachedEfCore.SqlAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MqttPub.Data;
using MqttPub.Services.MqttPub;
using MqttPub.ViewModels;
using MqttPub.ViewModels.AppActionModels.Create;
using MqttPub.ViewModels.AppActionModels.List;
using MqttPub.ViewModels.MqttActionModels.Create;
using MqttPub.ViewModels.MqttActionModels.List;
using MqttPub.ViewModels.MqttConnectionModels.Create;
using MqttPub.ViewModels.MqttConnectionModels.List;
using System.Reflection;

namespace MqttPub
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<KeyGeneratorVisitor>();
            builder.Services.AddSingleton<IDbQueryCacheHelper, DbQueryCacheHelper>();
            builder.Services.AddSingleton<IDbQueryCacheStore, DbQueryCacheStore>();
            builder.Services.AddSingleton<ISqlQueryEntityExtractor, GenericSqlQueryEntityExtractor>();
            builder.Services.AddSingleton<DbStateInterceptor>();

            builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlite("Data Source=" + Path.Combine(FileSystem.AppDataDirectory, AppDbContext.DbName)).AddInterceptors(serviceProvider.GetRequiredService<DbStateInterceptor>()); 
            });

            RegisterPages(builder.Services);

            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddSingleton<MqttClientFactory>();
            builder.Services.AddTransient<IMqttPublisher, MqttPublisher>();

            var builtBuilder = builder.Build();
            ApplyDatabasesMigrations(builtBuilder.Services);
            return builtBuilder;
        }

        private static void RegisterPages(IServiceCollection services)
        {
            services.AddSingleton<MainPageViewModel>();

            services.AddTransient<ListMqttConnectionViewModel>();
            services.AddTransient<CreateMqttConnectionViewModel>();

            services.AddTransient<ListMqttActionViewModel>();
            services.AddTransient<CreateMqttActionViewModel>();

            services.AddTransient<ListAppActionViewModel>();
            services.AddTransient<CreateAppActionViewModel>();
        }

        private static void ApplyDatabasesMigrations(IServiceProvider serviceProvider)
        {
            const string migrationKey = "AppliedMigration";

            var version = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version?.ToString() ?? AppInfo.VersionString;

            if (Preferences.Get(migrationKey, "") == version)
            {
                return;
            }

            var context = serviceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            Preferences.Set(migrationKey, version);
        }
    }
}