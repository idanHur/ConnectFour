﻿using Client.Services;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IHttpClientServiceImplementation _httpClientService;
        public static ConnectFourWindow currentInstance;

        private const int Rows = 6;
        private const int Columns = 7;
        private const int FallingDelay = 650;
        private Ellipse[,] gameBoard = new Ellipse[Rows, Columns];  // 2D array to store the game board

        public ConnectFourWindow()
        {
            InitializeComponent();
            _httpClientService = App.ServiceProvider.GetService<IHttpClientServiceImplementation>();

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
                    gameBoard[i, j] = ellipse;  // Add the ellipse to the game board
                }
            }
            // Set the current instance
            currentInstance = this;
            // Handle the Closed event
            this.Closed += (s, e) => { currentInstance = null; };
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

        private async void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedEllipse = (Ellipse)sender;
            int column = Grid.GetColumn(clickedEllipse); // Get the column of the clicked ellipse
            await FallingAnimation(column, Brushes.Red); // TODO: change logic to fit current player color
        }


        private async Task FallingAnimation(int col, Brush colorOfPlayer)
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

    }
}