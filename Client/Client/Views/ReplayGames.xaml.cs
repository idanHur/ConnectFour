using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
        private CancellationTokenSource _cancellationTokenSource; 

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

            _cancellationTokenSource = new CancellationTokenSource();

            // Adding event handlers
            _gameBoard.gamesComboBox.SelectionChanged += gamesComboBox_SelectionChanged;
            _gameBoard.StopGameButton.Click += StopReplayGameButton_Click;
        }
        private async void gamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Disable the ComboBox to prevent new selections while the animation is running
            _gameBoard.gamesComboBox.IsEnabled = false;

            _cancellationTokenSource.Cancel(); // Cancel the current animation if it's running
            _cancellationTokenSource = new CancellationTokenSource(); // Create a new CancellationTokenSource

            _gameBoard.isBoardEnabled = false;
            ComboBox comboBox = (ComboBox)sender;
            string selectedValue = comboBox.SelectedItem.ToString();
            selectedGame = _dbService.GetGame(_authService.currentPlayerId);
            _gameBoard.isBoardEnabled = true;
            await ReplayGame(_cancellationTokenSource.Token); // Pass the cancellation token to ReplayGame

            // Re-enable the ComboBox after the animation has finished
            _gameBoard.gamesComboBox.IsEnabled = true;
        }

        private async Task ReplayGame(CancellationToken cancellationToken)
        {
            _gameBoard.ResetBoard();
            foreach (Move move in selectedGame.Moves)
            {
                if (cancellationToken.IsCancellationRequested) // If cancellation is requested, stop the animation
                    return;
                if (move.Player == PlayerType.Human)
                {
                    _gameBoard.redCoins += 1;
                    _gameBoard.RedCoinsLabel.Content = _gameBoard.redCoins;
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Red);
                }
                else
                {
                    _gameBoard.yellowCoins += 1;
                    _gameBoard.YellowCoinsLabel.Content = _gameBoard.yellowCoins;
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Gold);
                }
            }
        }
        private void StopReplayGameButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel(); // Cancel the current animation when Stop is clicked
        }
    }
}
