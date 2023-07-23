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
        private Game selectedGame; // The selected game for replay
        private CancellationTokenSource _cancellationTokenSource; // A CancellationTokenSource which can be used to cancel the game replay
        public static ReplayGames currentInstance;

        public ReplayGames(AuthenticationService authService, GameBoard gameBoard, GameDatabaseService dbService) // Injected via DI
        {
            InitializeComponent();
            _dbService = dbService; 
            _gameBoard = gameBoard; 
            _authService = authService; 
            _gameBoard.DataContext = _gameBoard;

            gameBoardFrame.Content = _gameBoard; // Setting GameBoard instance to the Frame
            _gameBoard.NewGameButton.Visibility = Visibility.Collapsed; // Hide the NewGameButton
            _gameBoard.QuitGameButton.Visibility = Visibility.Collapsed; // Hide the QuitGameButton
            List<string> myGames = _dbService.GetPlayerGameIds(_authService.currentPlayerId).Select(i => i.ToString()).ToList(); // Get the list of games played by the current player
            _gameBoard.MyGames = new ObservableCollection<string>(myGames); // Set the games in the ComboBox

            _cancellationTokenSource = new CancellationTokenSource(); // Initialize the CancellationTokenSource

            // Adding event handlers
            _gameBoard.gamesComboBox.SelectionChanged += gamesComboBox_SelectionChanged; // Add an event handler for ComboBox SelectionChanged event
            _gameBoard.StopGameButton.Click += StopReplayGameButton_Click; // Add an event handler for StopGameButton Click event

            // Set the current instance
            currentInstance = this;
            // Handle the Closed event
            this.Closed += (s, e) => { currentInstance = null; };
        }

        // Event handler for ComboBox SelectionChanged event
        private async void gamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Disable the ComboBox to prevent new selections while the animation is running
            _gameBoard.gamesComboBox.IsEnabled = false;

            _cancellationTokenSource.Cancel(); // Cancel the current animation if it's running
            _cancellationTokenSource = new CancellationTokenSource(); // Create a new CancellationTokenSource

            ComboBox comboBox = (ComboBox)sender; // The ComboBox that triggered the event
            string selectedValue = comboBox.SelectedItem.ToString(); // The selected game id
            selectedGame = _dbService.GetGame(int.Parse(selectedValue)); // Get the selected game
            await ReplayGame(_cancellationTokenSource.Token); // Start the replay of the game

            // Re-enable the ComboBox after the animation has finished
            _gameBoard.gamesComboBox.IsEnabled = true;
        }

        // Method to replay the game
        private async Task ReplayGame(CancellationToken cancellationToken)
        {
            _gameBoard.ResetBoard(); // Reset the game board
            // Replay each move of the game
            foreach (Move move in selectedGame.Moves)
            {
                if (cancellationToken.IsCancellationRequested) // If cancellation is requested, stop the animation
                    return;
                if (move.Player == PlayerType.Human) 
                {
                    _gameBoard.redCoins += 1; 
                    _gameBoard.RedCoinsLabel.Content = _gameBoard.redCoins; // Update the RedCoinsLabel
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Red); // Show the falling animation for red coin
                }
                else 
                {
                    _gameBoard.yellowCoins += 1; 
                    _gameBoard.YellowCoinsLabel.Content = _gameBoard.yellowCoins; // Update the YellowCoinsLabel
                    await _gameBoard.FallingAnimation(move.ColumnNumber, Brushes.Gold); // Show the falling animation for yellow coin
                }
            }
        }

        private void StopReplayGameButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel(); // Cancel the current animation when Stop is clicked
        }
    }
}

