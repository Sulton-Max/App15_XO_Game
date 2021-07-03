using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App15_XO_Game.Windows
{
    /// <summary>
    /// Interaction logic for MainGame.xaml
    /// </summary>
    public partial class MainGame : Window
    {
        public MainGame(RoutedEventHandler showBack, Player player1, Player player2, GameArea gameArea)
        {
            InitializeComponent();
            GameEngine gameEngine = new GameEngine(gameArea, MainArea);
            GameLogic game = new GameLogic(gameEngine, player1, player2, Status, Username, showBack);
            game.Run();

            CloseBtn.Click += showBack as RoutedEventHandler;
            CloseBtn.Click += CloseBtnClick;
        }

        private void CloseBtnClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
