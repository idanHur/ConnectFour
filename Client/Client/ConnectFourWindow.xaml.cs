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


    }
}
