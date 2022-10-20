using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace battlerofl
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); // exit the app
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.chkdoRemember == true)
            {
                txtEmail.Text = Properties.Settings.Default.usrEmail;
                psbPassword.Password = Properties.Settings.Default.usrPassword;
                txtSoldierName.Text = Properties.Settings.Default.usrSoldier;
                chkRemember.IsChecked = true;
            }
            else
            {
                // do nothing
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // handle the data
            HandleData.handleLogin(txtEmail.Text, psbPassword.Password);

            if (txtEmail.Text == null && psbPassword.Password == null)
            {
                MessageBox.Show("Please enter a valid email and password to log in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (txtSoldierName.Text == null)
                {
                    MessageBox.Show("You must enter a soldier name!");
                }
                else
                {
                    Battlelog.m_playerID = txtSoldierName.Text;

                    // connect and authenticate
                    Battlelog.doConnection();
                    Battlelog.doAuthentication();

                    // once all that is done, populate the stats
                    PlayerInfo.fetchSoldierData(Battlelog.m_playerID);
                }
            }

            // if the connection and authentication succeeded, proceed to the next window ;)
            if (Battlelog.isReady())
            {
                this.Hide(); // hide this form
                new MainWindow().Show(); // create a new instance of the form
            }
            else
            {
                // do nothing
            }
        }

        private void chkRemember_Checked(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == null && psbPassword.Password == null)
            {
                MessageBox.Show("Please enter a valid email and password to log in", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (txtSoldierName.Text == null)
                {
                    MessageBox.Show("You must enter a soldier name!");
                }
                else
                {
                    Properties.Settings.Default.usrEmail = txtEmail.Text;
                    Properties.Settings.Default.usrPassword = psbPassword.Password;
                    Properties.Settings.Default.usrSoldier = txtSoldierName.Text;
                    Properties.Settings.Default.chkdoRemember = chkRemember.IsChecked == true;
                    Properties.Settings.Default.Save(); // save the settings
                }
            }
        }

        
    }
}
