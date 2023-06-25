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
    /// <summary>
    /// Interaction logic for ConnectFourWindow.xaml
    /// </summary>
    public partial class ConnectFourWindow : Window
    {
        public ConnectFourWindow()
        {
            InitializeComponent();
            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    AddGamePiece(i, j, Brushes.Black, 0);
                }
            }
        }
        private void AddGamePiece(int row, int col, Brush color, double opacity)
        {
            double ellipseHeight = 65;  // Height of the ellipse
            double ellipseWidth = 66;  // Width of the ellipse
            double topSpacing = 69;  // Spacing between the ellipses
            double sideSpacing = 95;  // Spacing between the ellipses

            var gamePiece = new Ellipse
            {
                Width = ellipseWidth,
                Height = ellipseHeight,
                Fill = color,  
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Opacity = opacity
            };
            // Set the row and column as attached properties on the ellipse
            EllipseProperties.SetRow(gamePiece, row);
            EllipseProperties.SetColumn(gamePiece, col);

            gamePiece.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

            double topPosition = row * (ellipseWidth + sideSpacing);
            double leftPosition = col * (ellipseHeight + topSpacing);
            Canvas.SetTop(gamePiece, topPosition);
            Canvas.SetLeft(gamePiece, leftPosition);

            GameCanvas.Children.Add(gamePiece);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedEllipse = (Ellipse)sender;

            // Get row and col
            int row = EllipseProperties.GetRow(clickedEllipse);
            int col = EllipseProperties.GetColumn(clickedEllipse);


            
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
