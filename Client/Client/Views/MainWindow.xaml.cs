using Client.Services;
using GameLogicClient.Models;
using GameLogicClient.Services;
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
        private readonly GameDatabaseService _dbService;

        public MainWindow(ApiService apiService, INavigationService navigationService, AuthenticationService authService, GameDatabaseService dbService)
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            _apiService = apiService;
            _navigationService = navigationService;
            _authService = authService;
            _dbService = dbService;

            Player player = _dbService.GetPlayer(_authService.currentPlayerId);
            PlayerCountryLabel.Content += player.country;
            PlayerIdLabel.Content += player.playerId.ToString();
            PlayerNameLabel.Content += player.playerName;
            PlayerPhoneNumberLabel.Content += player.phoneNumber;

        }
        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to close?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            try
            {
                await _apiService.EndGameAsync();
                Application.Current.Shutdown();
            }
            catch(Exception ex)
            {
                if(ex.Message == "There are no played games")
                    Application.Current.Shutdown();
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
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
