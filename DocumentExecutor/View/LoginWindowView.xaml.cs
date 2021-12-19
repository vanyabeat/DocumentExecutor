using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using DocumentExecutor.ViewModel;

namespace DocumentExecutor.View
{
    /// <summary>
    /// Interaction logic for LoginWindowView.xaml
    /// </summary>
    public partial class LoginWindowView : Window
    {
        public LoginWindowView()
        {
            InitializeComponent();
            DataContext = new DataManageVm();
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (WindowsCredentialsCheckBox.IsChecked == true)
            {
                UserNameTextBox.Text = DataManageVm.SelectedUserName;
                PasswordBox.Password = DataManageVm.SelectedPassword;
            }
            else
            {
                UserNameTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
            }
        }

        private void BtnActionMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnActionSystemInformation_OnClick(object sender, RoutedEventArgs e)
        {
            var systemInformationWindow = new SystemInformationWindow();
            systemInformationWindow.Show();
        }

        private void btnActionClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            Left = Left + e.HorizontalChange;
            Top = Top + e.VerticalChange;
        }

        private void PasswordChangedHandler(object sender, RoutedEventArgs e)
        {
            DataManageVm.SelectedPassword = PasswordBox.Password;
        }
    }
}
