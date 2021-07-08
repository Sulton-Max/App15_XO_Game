using App15_XO_Game.Windows;
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

namespace App15_XO_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainGame _mainGame;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowBack(object sender, RoutedEventArgs args)
        {
            this.Visibility = Visibility.Visible;
            _mainGame.Close();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isPlayer1X = (Player1Sign.SelectedIndex == 0);
            Player player1 = new Player(Player1Name.Text, ((isPlayer1X == true) ? GameSigns.XSign : GameSigns.OSign), false);
            Player player2 = new Player(Player2Name.Text, ((isPlayer1X == true) ? GameSigns.OSign : GameSigns.XSign), IsAgainstPc.IsChecked ?? false);
            GameArea gameArea = new GameArea((XLength.SelectedIndex + 3), (YLength.SelectedIndex + 3));

            _mainGame = new MainGame(ShowBack, player1, player2, gameArea);
            _mainGame.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void UsernameChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Player1Name.Text) && !string.IsNullOrEmpty(Player2Name.Text))
                StartBtn.IsEnabled = true;
            else
                StartBtn.IsEnabled = false;
        }

        private void IsAgainstPc_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked??false)
            {
                Player2Name.Text = "PC";
                Player2Name.IsEnabled = false;
            }
            else
            {
                Player2Name.Text = string.Empty;
                Player2Name.IsEnabled = true;
                StartBtn.IsEnabled = false;
            }
        }
    }
}