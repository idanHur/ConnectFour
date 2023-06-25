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
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var slot = new Ellipse
                    {
                        Width = 50,  // or any other size
                        Height = 50,
                        Fill = Brushes.White, // for an empty slot
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    Grid.SetRow(slot, row);
                    Grid.SetColumn(slot, col);

                    this.LayoutRoot.Children.Add(slot);
                }
            }
        }
    }
}
