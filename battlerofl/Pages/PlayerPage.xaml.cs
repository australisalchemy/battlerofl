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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Web;

namespace battlerofl
{
    /// <summary>
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            InitializeComponent();

            // images
            BitmapImage rnkImg = new BitmapImage();
            BitmapImage kitImg = new BitmapImage();
            BitmapImage b2kImg = new BitmapImage();
            b2kImg.BeginInit();
            rnkImg.BeginInit();
            kitImg.BeginInit();
            kitImg.UriSource = new Uri(PlayerInfo.getKitImage("assault", "l", "us"), UriKind.Absolute);
            rnkImg.UriSource = new Uri(PlayerInfo.getRankImage(PlayerInfo.playerData.playerRank, "large"), UriKind.Absolute);
            b2kImg.UriSource = new Uri("http://battlelog-cdn.battlefield.com/public/common/tags/b2k.gif", UriKind.RelativeOrAbsolute);
            rnkImg.EndInit();
            kitImg.EndInit();
            b2kImg.EndInit();

            // populate the controls
            lblInfoTitleRANK.Content = "RANK " + PlayerInfo.playerData.playerRank;
            lblKills.Content = PlayerInfo.playerData.playerKills;
            lblDeaths.Content = PlayerInfo.playerData.playerDeaths;
            lblAccuracy.Content = string.Format("{0}%", PlayerInfo.playerAccuracy());
            lblQuitPercentage.Content = PlayerInfo.playerQuits() + "%";
            lblKDR.Content = PlayerInfo.playerKDR();
            lblSPM.Content = PlayerInfo.playerData.playerScorePM;
            lblTimePlayed.Content = "Timed Played: " + PlayerInfo.playerPlayedTime();
            lblRepairs.Content = PlayerInfo.playerData.playerRepairs;
            lblResupplies.Content = PlayerInfo.playerData.playerResupplies;
            lblKillAssists.Content = PlayerInfo.playerData.playerKillAssists;
            lblTotalScore.Content = PlayerInfo.getTotalScore();
            imgKit.Source = kitImg;
            imgbRank.Source = rnkImg;

            if (PlayerInfo.playerData.playerLicense == "b2k")
            {
                imgB2KSupport.Source = b2kImg;
            }
            else
            {
                // doesn't support B2K, don't show the image
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
          
        }


    }
}
