using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Client.Services;
using GameLogicClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for ReplayGames.xaml
    /// </summary>
    public partial class ReplayGames : Window
    {
        private readonly GameBoard _gameBoard;
        private readonly GameDatabaseService _dbService;
        private readonly AuthenticationService _authService;

        public ReplayGames(AuthenticationService authService, GameBoard gameBoard, GameDatabaseService dbService) // GameBoard is injected via DI
        {
            InitializeComponent();
            _dbService = dbService;
            _gameBoard = gameBoard;
            _authService = authService;

            gameBoardFrame.Content = _gameBoard; // Setting GameBoard instance to the Frame
            _gameBoard.NewGameButton.Visibility = Visibility.Collapsed;
            _gameBoard.QuitGameButton.Visibility = Visibility.Collapsed;
            List<string> myGames = _dbService.GetPlayerGameIds(_authService.currentPlayerId).Select(i => i.ToString()).ToList();
            _gameBoard.MyGames = new ObservableCollection<string>(myGames);
            _gameBoard.gamesComboBox.SelectionChanged += gamesComboBox_SelectionChanged;
        }
        private void gamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedValue = comboBox.SelectedItem.ToString();

            // Do something with the selected value...
        }

    }
}
