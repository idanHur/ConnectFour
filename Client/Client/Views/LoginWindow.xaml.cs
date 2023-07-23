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
            int playerId = int.Parse(PlayerIdTextBox.Text);
            string password = passwordBox.Password;
            
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
                // Show a message box with the error message
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ShowErrorMessage();

            }
        }
        private void ShowErrorMessage()
        {
            ErrorLabel.Content = "Login failed. Please check your player ID and password.";
            // Clear the PlayerIdTextBox
            PlayerIdTextBox.Text = "";

            // Clear the PasswordTextBox
            textBox.Text = "";
            passwordBox.Password = "";

        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // When password changes, update the text in the textbox.
            textBox.Text = passwordBox.Password;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // When the text in the textbox changes, update the password in the PasswordBox.
            passwordBox.Password = textBox.Text;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // When the checkbox is checked, show the textbox and hide the PasswordBox.
            passwordBox.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // When the checkbox is unchecked, show the PasswordBox and hide the textbox.
            passwordBox.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Collapsed;
        }

    }
}
