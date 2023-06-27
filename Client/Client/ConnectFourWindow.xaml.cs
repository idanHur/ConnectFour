using Client.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{

    public partial class ConnectFourWindow : Window
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private Ellipse[,] gameBoard = new Ellipse[Rows, Columns];  // 2D array to store the game board

        public ConnectFourWindow()
        {
            InitializeComponent();

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

                    var ellipse = AddGamePiece(i, j, Brushes.Transparent, 1);
                    gameBoard[i, j] = ellipse;  // Add the ellipse to the game board
                }
            }
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

            // Attach a click event handler to the ellipse
            gamePiece.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

            Grid.SetRow(gamePiece, row);
            Grid.SetColumn(gamePiece, col);

            BoardGrid.Children.Add(gamePiece);
            return gamePiece;
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedEllipse = (Ellipse)sender;
            clickedEllipse.Fill = Brushes.Red;  

        }

        private void StartFallingAnimation(Ellipse ellipse, Color targetColor)
        {
            Storyboard fillStoryboard = (Storyboard)FindResource("FillAnimation");
            ColorAnimation fillAnimation = (ColorAnimation)fillStoryboard.Children[0];
            fillAnimation.To = targetColor;

            DoubleAnimation fallAnimation = (DoubleAnimation)fillStoryboard.Children[1];
            Canvas.SetTop(ellipse, -50); // Set initial position above the canvas

            Storyboard.SetTarget(fillAnimation, ellipse);
            Storyboard.SetTarget(fallAnimation, ellipse);
            fillStoryboard.Begin();
        }
    }
}
