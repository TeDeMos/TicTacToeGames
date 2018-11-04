using System.Windows;
using System.Windows.Input;

namespace TicTacToe
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>

    public enum Figure { Cross, Circle }
    public enum Status { Nothing, Tie, CrossWin, CircleWin }
    public enum GameType { Small, Big, Huge }
    public enum PlayerType { Player, AI }

    public partial class MainWindow : Window
    {
        GameManager game;
        int sqaureSize;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this); //Get mouse position
            game.ClickHandler((int)p.X / sqaureSize, (int)p.Y / sqaureSize); //Handle the click
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            grdGrid.Children.Clear(); //Clear the buttons
            game = new GameManager(GameType.Small, 300, grdGrid, chckPlayer1.IsChecked == true ? PlayerType.AI : PlayerType.Player, chckPlayer2.IsChecked == true ? PlayerType.AI : PlayerType.Player); //Start small game
            sqaureSize = 300;
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            grdGrid.Children.Clear(); //Clear the buttons
            game = new GameManager(GameType.Big, 100, grdGrid, chckPlayer1.IsChecked == true ? PlayerType.AI : PlayerType.Player, chckPlayer2.IsChecked == true ? PlayerType.AI : PlayerType.Player); //Start big game
            sqaureSize = 100;
        }

        private void btnHuge_Click(object sender, RoutedEventArgs e)
        {
            grdGrid.Children.Clear(); //Clear the buttons
            game = new GameManager(GameType.Huge, 33, grdGrid, chckPlayer1.IsChecked == true ? PlayerType.AI : PlayerType.Player, chckPlayer2.IsChecked == true ? PlayerType.AI : PlayerType.Player); //Start huge game
            sqaureSize = 33;
        }
    }
}
