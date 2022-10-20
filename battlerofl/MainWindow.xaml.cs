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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BrushConverter m_bc = new BrushConverter();
        private PlayerPage m_playerPage = new PlayerPage();
        private ServersPage m_serversPage = new ServersPage();
        private friendsPage m_friendsPage = new friendsPage();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0); // kill from environment due to login form being 'hidden'
        }

        #region Events
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Application.Current.MainWindow.DragMove();
            }
        }

        /// <summary>
        /// Changes the player button to DOWN.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            checkButton(1);
        }

        private void checkButton(int button)
        {
            switch (button)
            {
                case 1: // btnPlayer
                    btnServers.Foreground = (Brush)m_bc.ConvertFrom("White");
                    btnPlayer.Foreground = (Brush)m_bc.ConvertFrom("#FF5C5C5C"); // highlighted
                    btnFriends.Foreground = (Brush)m_bc.ConvertFrom("White");
                    framePages.Navigate(m_playerPage);
                    break;
                case 2: // btnServers
                    btnPlayer.Foreground = (Brush)m_bc.ConvertFrom("White");
                    btnServers.Foreground = (Brush)m_bc.ConvertFrom("#FF5C5C5C"); // this is highlighted
                    btnFriends.Foreground = (Brush)m_bc.ConvertFrom("White");
                    framePages.Navigate(m_serversPage);
                    break;

                case 3: // btnFriends
                    btnServers.Foreground = (Brush)m_bc.ConvertFrom("White");
                    btnPlayer.Foreground = (Brush)m_bc.ConvertFrom("White");
                    btnFriends.Foreground = (Brush)m_bc.ConvertFrom("#FF5C5C5C"); // this one is highlighted
                    framePages.Navigate(m_friendsPage);
                    break;
            }

            if (framePages.CanGoBack == true)
            {
                framePages.RemoveBackEntry();
            }
        }

        private void btnServers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            checkButton(2);
        }

        private void btnFriends_MouseDown(object sender, MouseButtonEventArgs e)
        {
            checkButton(3);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set the player tab to on
            checkButton(2);

            // change the apps title
            this.Title = "Battlerofl | " + Battlelog.m_playerID + " | BETA - REL 2 | .NET CLR 4.0.30319.1";
        }

        private void btnLogout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
            {
                // log the user out
                string output = "";
                Battlelog.fetchPage(ref output, "http://battlelog.battlefield.com/bf3/session/logout/", true, "GET");
                Environment.Exit(0);
            }
            else
            {
                // do nothing
            }
        }

        private void btnForums_MouseDown(object sender, MouseButtonEventArgs e)
        {
            checkButton(4);
        }

        private void btnCampaign_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Executable.ExecuteSP(PlayerInfo.playerData.playerPersonaID, HandleData.getAuthToken(), HandleData.getLoginToken());
        }
        #endregion
    }
}
