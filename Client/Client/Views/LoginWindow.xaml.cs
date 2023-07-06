using Client.Services;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly INavigationService _navigationService;

        public LoginWindow(ApiService apiService, INavigationService navigationService)
        {
            InitializeComponent();
            _apiService = apiService;
            _navigationService = navigationService;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string playerId = PlayerIdTextBox.Text;
            string password = PasswordTextBox.Text;

            try
            {
                bool result = await _apiService.LoginAsync(playerId, password);
                if (result)
                {
                    // Open the main application window
                    _navigationService.NavigateToMain();

                    // Close the login window after opening the main window
                    this.Close();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage();

            }


        }
        private void ShowErrorMessage()
        {
            ErrorLabel.Content = "Login failed. Please check your player ID and password.";
            // Clear the PlayerIdTextBox
            PlayerIdTextBox.Text = "";

            // Clear the PasswordTextBox
            PasswordTextBox.Text = "";
        }
    }
}
