using Client.Services;
using Microsoft.Extensions.DependencyInjection;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly INavigationService _navigationService;
        public MainWindow(ApiService apiService, INavigationService navigationService)
        {
            InitializeComponent();
            _apiService = apiService;
            _navigationService = navigationService;
        }

        private void EndGameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectFourWindow.currentInstance == null)
            {
                new ConnectFourWindow().Show();
            }
            else
            {
                ConnectFourWindow.currentInstance.Activate();
            }
        }

    }
}
