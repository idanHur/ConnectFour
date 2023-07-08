using Client.Services;
using GameLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly INavigationService _navigationService;
        private readonly AuthenticationService _authService;

        public MainWindow(ApiService apiService, INavigationService navigationService, AuthenticationService authService)
        {
            InitializeComponent();
            _apiService = apiService;
            _navigationService = navigationService;
            _authService = authService;

            Player player = _authService.GetCurrentPlayer();
            PlayerCountryLabel.Content += player.country;
            PlayerIdLabel.Content += player.playerId.ToString();
            PlayerNameLabel.Content += player.playerName;
            PlayerPhoneNumberLabel.Content += player.phoneNumber;

        }

        private void EndGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectFourWindow.currentInstance == null)
            {
                try
                {
                    await _apiService.StartGameAsync();
                    // Open the game window
                    _navigationService.NavigateToGame();
                }
                catch(Exception ex)
                {
                    ErrorLabel.Content = ex.Message.ToString();
                }
            }
            
        }

    }
}
