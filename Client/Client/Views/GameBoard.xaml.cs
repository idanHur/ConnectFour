using Client.Services;
using GameLogicClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl, INotifyPropertyChanged
    {
        public readonly INavigationService _navigationService;
        public static GameBoard currentInstance;
        private ObservableCollection<string> _myGames;
        public ObservableCollection<string> MyGames
        {
            get { return _myGames; }
            set
            {
                if (_myGames != value)
                {
                    _myGames = value;
                    OnPropertyChanged(nameof(MyGames));
                }
            }
        }
        public const int Rows = 6;
        public const int Columns = 7;
        public const int FallingDelay = 650;
        public Ellipse[,] gameBoard = new Ellipse[Rows, Columns];  // 2D array to store the game Board
        public bool gameEnded;
        public bool isBoardEnabled;
        public int yellowCoins;
        public int redCoins;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public GameBoard(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            isBoardEnabled = true;
            gameEnded = false;
            yellowCoins = 0;
            redCoins = 0;
            // Create the grid cells and ellipses dynamically
            for (int i = 0; i < Rows; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < Columns; j++)
                {
                    if (i == 0)
                    {
                        BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    var ellipse = AddGamePiece(i, j, Brushes.White, 1);
                    gameBoard[i, j] = ellipse;  // Add the ellipse to the game Board
                }
            }
            // Set the current instance
            currentInstance = this;
            // Subscribe to the Loaded event
            this.Loaded += (s, e) =>
            {
                // Get the parent Window of the UserControl
                var window = Window.GetWindow(this);
                // If the Window is not null, subscribe to its Closed event
                if (window != null)
                {
                    window.Closed += (s1, e1) => { currentInstance = null; };
                }
            };

        }
        private Ellipse AddGamePiece(int row, int col, Brush color, double opacity)
        {
            double ellipseHeight = 55;  // Height of the ellipse
            double ellipseWidth = 55;  // Width of the ellipse

            var gamePiece = new Ellipse
            {
                Width = ellipseWidth,
                Height = ellipseHeight,
                Fill = color,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Opacity = opacity,
                Margin = new Thickness(5)  // Add a margin of 5 units
            };

            Grid.SetRow(gamePiece, row);
            Grid.SetColumn(gamePiece, col);

            BoardGrid.Children.Add(gamePiece);
            return gamePiece;
        }
        public async Task FallingAnimation(int col, Brush colorOfPlayer)
        {
            for (int i = 0; i < Rows; i++)
            {
                SolidColorBrush brush = gameBoard[i, col].Fill as SolidColorBrush;
                if (brush != null)
                {
                    Color color = brush.Color;
                    if (color != Brushes.White.Color)
                    {
                        // If the current ellipse isnt white (one of the players color), exit the loop
                        break;
                    }
                    else
                    {
                        // Temporarily color the current ellipse red
                        gameBoard[i, col].Fill = colorOfPlayer;

                        // Wait for 1 second
                        await Task.Delay(FallingDelay);

                        // If it's not the last white ellipse, revert the color back to white
                        if (i != Rows - 1 && !((gameBoard[i + 1, col].Fill as SolidColorBrush)?.Color != Brushes.White.Color))
                        {
                            gameBoard[i, col].Fill = Brushes.White;
                        }
                    }
                }
            }
        }
        public void ResetBoard()
        {
            for (int i = 0; i < Rows; i++)
                for (int J = 0; J < Columns; J++)
                {
                    gameBoard[i, J].Fill = Brushes.White;
                }
            redCoins = 0;
            yellowCoins = 0;
            RedCoinsLabel.Content = redCoins;
            YellowCoinsLabel.Content = yellowCoins;
            isBoardEnabled = true;
        }


    }
}
