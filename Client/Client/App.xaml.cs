using Client.Services;
using Client.Utilities.Json;
using Client.Views;
using GameLogicClient.Data;
using GameLogicClient.Models;
using GameLogicClient.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup EF Core with SQL Server
            services.AddDbContext<GameContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("GameContext")));


            // Register services 
            services.AddSingleton<ApiService>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<GameDatabaseService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Register windows
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<ConnectFourWindow>();
            services.AddTransient<GameBoard>();
            services.AddTransient<ReplayGames>();

            // Configure JSON serialization settings
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new MoveConverter(),
                    new GameConverter(),
                    new PlayerConverter()
                }
            };
        }
    }
}
