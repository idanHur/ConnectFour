using Client.Services;
using Client.Utilities.Errors;
using GameLogicClient.Models;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for PlayGame.xaml
    /// </summary>
    public partial class PlayGame : Window
    {
        private readonly GameBoard _gameBoard;
        private readonly ApiService _apiService;
        public bool gameEnded;
        public static PlayGame currentInstance;

        public PlayGame(ApiService apiService, GameBoard gameBoard)
        {
            InitializeComponent();
            _gameBoard = gameBoard;
            _gameBoard.DataContext = _gameBoard;
            _apiService = apiService;
            gameEnded = false;
            this.Closing += PlayGame_Closing;

            gameBoardFrame.Content = _gameBoard; // Setting GameBoard instance to the Frame
            _gameBoard.StopGameButton.Visibility = Visibility.Collapsed; // Hide the NewGameButton
            _gameBoard.gamesComboBox.Visibility = Visibility.Collapsed; // Hide the QuitGameButton


            // Adding event handlers
            _gameBoard.NewGameButton.Click += NewGameButton_Click; // Add an event handler for ComboBox SelectionChanged event
            _gameBoard.QuitGameButton.Click += QuitGameButton_Click; // Add an event handler for StopGameButton Click event

            // Adding event handlers to all ellipses
            for (int i = 0; i < GameBoard.Rows; i++)
            {
                for (int j = 0; j < GameBoard.Columns; j++)
                {
                    _gameBoard.gameBoard[i, j].MouseDown += Ellipse_MouseLeftButtonDown;
                }
            }

            // Set the current instance
            currentInstance = this;

        }
        private async void PlayGame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if(!gameEnded)
                    await _apiService.EndGameAsync();
                currentInstance = null;
                gameEnded = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_gameBoard.isBoardEnabled) // To disable the Board 
                return;
            var clickedEllipse = (Ellipse)sender;
            int column = Grid.GetColumn(clickedEllipse); // Get the column of the clicked ellipse
            try
            {
                Move lastmove = await _apiService.MakeMoveAsync(column);
                if (lastmove == null)
                {
                    _gameBoard.ErrorLabel.Opacity = 1;
                    return;
                }
                _gameBoard.redCoins += 1;
                _gameBoard.RedCoinsLabel.Content = _gameBoard.redCoins;
                _gameBoard.isBoardEnabled = false;
                _gameBoard.TurnLabel.Opacity = 0;
                await _gameBoard.FallingAnimation(lastmove.ColumnNumber, Brushes.Red);
                if (!gameEnded) // To not make ai move if quit game button is pressed after player move
                {
                    Move aiMove = await _apiService.AiMoveAsync();
                    _gameBoard.yellowCoins += 1;
                    _gameBoard.YellowCoinsLabel.Content = _gameBoard.yellowCoins;
                    await _gameBoard.FallingAnimation(aiMove.ColumnNumber, Brushes.Gold);
                    _gameBoard.isBoardEnabled = true;
                    _gameBoard.TurnLabel.Opacity = 1;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorCodes.PlayerNotFound))
                {
                    // Close all windows and open the login application window
                    CloseAllWindowsExceptAndOpenLogin();
                }
                _gameBoard.ErrorLabel.Opacity = 1;
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private async void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: enable Board if disabled
            try
            {
                await _apiService.StartGameAsync();
                _gameBoard.ResetBoard();
                gameEnded = false;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorCodes.PlayerNotFound))
                {
                    // Close all windows and open the login application window
                    CloseAllWindowsExceptAndOpenLogin();
                }
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: disable Board
            try
            {
                await _apiService.EndGameAsync();
                _gameBoard.isBoardEnabled = false;
                gameEnded = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorCodes.PlayerNotFound))
                {
                    // Close all windows and open the login application window
                    CloseAllWindowsExceptAndOpenLogin();
                }
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void CloseAllWindowsExceptAndOpenLogin()
        {
            _gameBoard._navigationService.NavigateToLogin();
            for (int intCounter = Application.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
            {
                Window win = Application.Current.Windows[intCounter];
                if (win is MainWindow mainWindow)
                {
                    // Unsubscribe from the Closing event to prevent executing its closing logic
                    mainWindow.UnsubscribeClosingEvent();
                }
                if (!(win is LoginWindow))
                    win.Close();
            }
        }
    }
}
