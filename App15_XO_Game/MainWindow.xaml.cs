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

            _mainGame = new MainGame(ShowBack);
            _mainGame.Show();
            this.Visibility = Visibility.Hidden;
        }

        public void ShowBack(object sender, RoutedEventArgs args)
        {
            this.Visibility = Visibility.Visible;
            _mainGame.Close();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void UsernameChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Player1Name.Text) && !string.IsNullOrEmpty(Player2Name.Text))
                StartBtn.IsEnabled = true;
        }
    }
}