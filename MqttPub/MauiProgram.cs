using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MqttPub.Application.DependencyInjection;
using MqttPub.Infrastructure;
using MqttPub.Infrastructure.DependencyInjection;
using MqttPub.Infrastructure.Initialization;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(Path.Combine(FileSystem.AppDataDirectory, AppDbContext.DbName));

            RegisterPages(builder.Services);

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
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .First(m => m.Key == "BuildId").Value;

            if (Preferences.Get(migrationKey, "") == version)
            {
                return;
            }

            var context = serviceProvider.GetRequiredService<AppDbContext>();

            InitilizeDbContext.Initialize(context);

            Preferences.Set(migrationKey, version);
        }
    }
}