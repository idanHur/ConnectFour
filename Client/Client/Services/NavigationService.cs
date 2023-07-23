using Client.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateToGame()
        {
            ConnectFourWindow gameWindow = _serviceProvider.GetRequiredService<ConnectFourWindow>();
            gameWindow.Show();
        }

        public void NavigateToLogin()
        {
            LoginWindow loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
        public void NavigateToReplayGames()
        {
            ReplayGames replayWindow = _serviceProvider.GetRequiredService<ReplayGames>();
            replayWindow.Show();
        }
        public void NavigateToMain()
        {
            MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
