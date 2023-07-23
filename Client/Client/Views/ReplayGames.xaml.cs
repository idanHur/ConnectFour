using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Client.Services;
using GameLogicClient.Models;
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
        private Game selectedGame;
        public ReplayGames(AuthenticationService authService, GameBoard gameBoard, GameDatabaseService dbService) // GameBoard is injected via DI
        {
            InitializeComponent();
            _dbService = dbService;
            _gameBoard = gameBoard;
            _authService = authService;
            _gameBoard.DataContext = _gameBoard;

            gameBoardFrame.Content = _gameBoard; // Setting GameBoard instance to the Frame
            _gameBoard.NewGameButton.Visibility = Visibility.Collapsed;
            _gameBoard.QuitGameButton.Visibility = Visibility.Collapsed;
            List<string> myGames = _dbService.GetPlayerGameIds(_authService.currentPlayerId).Select(i => i.ToString()).ToList();
            _gameBoard.MyGames = new ObservableCollection<string>(myGames);
            _gameBoard.gamesComboBox.SelectionChanged += gamesComboBox_SelectionChanged;
        }
        private async void gamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedValue = comboBox.SelectedItem.ToString();
            selectedGame = _dbService.GetGame(_authService.currentPlayerId);
            await ReplayGame();
        }
        private async Task ReplayGame()
        {
            _gameBoard.ResetBoard();
            foreach (Move move in selectedGame.Moves)
            {
                if(move.Player == PlayerType.Human)
                {
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Red);
                }
                else
                {
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Gold);

                }
            }

        }

    }
}
