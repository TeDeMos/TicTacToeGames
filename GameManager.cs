using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    public class GameManager
    {
        public SmallGrid SmallGrid { get; private set; }
        public BigGrid BigGrid { get; private set; }
        public HugeGrid HugeGrid { get; private set; }
        public int BigGridCurrentPosX { get; private set; }
        public int BigGridCurrentPosY { get; private set; }
        public int HugeGridCurrentPosX { get; private set; }
        public int HugeGridCurrentPosY { get; private set; }
        private Figure turn;
        private GameType size;
        private int squareSize;
        private Grid refGrid;
        private AI player1;
        private AI player2;

        public GameManager(GameType gameSize, int cellSize, Grid grid, PlayerType playerOne, PlayerType playerTwo)
        {
            size = gameSize;
            refGrid = grid;
            squareSize = cellSize;
            if (playerOne == PlayerType.AI) //Create the AIs
            {
                player1 = new AI(gameSize);
            }
            if (playerTwo == PlayerType.AI)
            {
                player2 = new AI(gameSize);
            }
            if (size == GameType.Small)
            {
                SmallGrid = new SmallGrid(0, 0, 0, 0, GameType.Small, squareSize, refGrid); //Create a small grid and pass it to the AIS
                if (playerOne == PlayerType.AI)
                {
                    player1.AddGrid(SmallGrid);
                }
                if (playerTwo == PlayerType.AI)
                {
                    player2.AddGrid(SmallGrid);
                }
            }
            else if (size == GameType.Big)
            {
                BigGrid = new BigGrid(0, 0, GameType.Big, squareSize, refGrid); //Create a big grid and pass it to the AIs
                BigGridCurrentPosX = 1; //Set the available small grid
                BigGridCurrentPosY = 1;
                BigGrid.Color(1, 1, Brushes.Blue); //Color the available small gird
                if (playerOne == PlayerType.AI)
                {
                    player1.AddGrid(BigGrid);
                }
                if (playerTwo == PlayerType.AI)
                {
                    player2.AddGrid(BigGrid);
                }
            }
            else if (size == GameType.Huge)
            {
                HugeGrid = new HugeGrid(squareSize, refGrid); //Create a huge grid and pass it to the AIs
                HugeGridCurrentPosX = 1; //Set the available big grid
                HugeGridCurrentPosY = 1;
                BigGridCurrentPosX = 1; //Set the available small grid
                BigGridCurrentPosY = 1;
                HugeGrid.Color(HugeGridCurrentPosX, HugeGridCurrentPosY, BigGridCurrentPosX, BigGridCurrentPosY, Brushes.Blue); //Color the available small grid
                if (playerOne == PlayerType.AI)
                {
                    player1.AddGrid(HugeGrid);
                }
                if (playerTwo == PlayerType.AI)
                {
                    player2.AddGrid(HugeGrid);
                }
            }
            if (playerOne == PlayerType.AI)
            {
                player1.AddManager(this);
            }
            if (playerTwo == PlayerType.AI)
            {
                player2.AddManager(this);
            }
            if (playerOne == PlayerType.AI)
            {
                player1.MakeChoice();
            }
        }

        public async void ClickHandler(int x, int y)
        {
            if (size == GameType.Small)
            {
                //board[x, y] - cell clicked
                if (!SmallGrid.Board[x, y].HasValue) //If the cell is empty
                {
                    SmallGrid.AddFigure(x, y, turn); //Add figure
                    turn = turn == Figure.Cross ? Figure.Circle : Figure.Cross; //Change the turn
                    if (SmallGrid.Status != Status.Nothing)
                    {
                        EndHandler(SmallGrid.Status); //End the game if the small grid is full
                    }
                }
            }
            else if (size == GameType.Big)
            {
                //board[x / 3, y / 3] - small grid clicked
                //board[x / 3, y / 3].board[x % 3, y % 3] - cell clicked
                if ((x / 3 == BigGridCurrentPosX && y / 3 == BigGridCurrentPosY || BigGridCurrentPosX == -1) && BigGrid.Board[x / 3, y / 3].Status == Status.Nothing && !BigGrid.Board[x / 3, y / 3].Board[x % 3, y % 3].HasValue) //If the cell is empty and the small grid isn't locked
                {
                    BigGrid.Color(BigGridCurrentPosX, BigGridCurrentPosY, Brushes.Black); //Decolor the previous available small grid / small grids
                    BigGrid.AddFigure(x / 3, y / 3, x % 3, y % 3, turn); //Add figure
                    BigGridCurrentPosX = x % 3; //Set the new available small grid
                    BigGridCurrentPosY = y % 3;
                    if (BigGrid.Board[BigGridCurrentPosX, BigGridCurrentPosY].Status != Status.Nothing)
                    {
                        BigGridCurrentPosX = -1; //If the selected small grid is complete select all the small grids
                    }
                    BigGrid.Color(BigGridCurrentPosX, BigGridCurrentPosY, Brushes.Blue); //Color the new available small grid / small grids
                    turn = turn == Figure.Cross ? Figure.Circle : Figure.Cross; //Change the turn
                    if (BigGrid.Status != Status.Nothing)
                    {
                        EndHandler(BigGrid.Status); //End the game if the big grid is full
                    }
                }
            }
            else if (size == GameType.Huge)
            {
                //board[x / 9, y / 9] - big grid clicked
                //board[x / 9, y / 9].board[x % 9 / 3, y % 9 / 3] - small grid clicked
                //board[x / 9, y / 9].board[x % 9 / 3, y % 9 / 3].board[x % 3, y % 3] - cell clicked
                if ((HugeGridCurrentPosX == -1 || (HugeGridCurrentPosX == x / 9 && HugeGridCurrentPosY == y / 9)) && (BigGridCurrentPosX == -1 || (BigGridCurrentPosX == x % 9 / 3 && BigGridCurrentPosY == y % 9 / 3)) && HugeGrid.Board[x / 9, y / 9].Status == Status.Nothing && HugeGrid.Board[x / 9, y / 9].Board[x % 9 / 3, y % 9 / 3].Status == Status.Nothing && !HugeGrid.Board[x / 9, y / 9].Board[x % 9 / 3, y % 9 / 3].Board[x % 3, y % 3].HasValue) //If the cell is empty and the small grid isn't locked and the big grid isn't locked
                {
                    HugeGrid.Color(HugeGridCurrentPosX, HugeGridCurrentPosY, BigGridCurrentPosX, BigGridCurrentPosY, Brushes.Black); //Decolor the previous available small grid / big grid / big grids
                    HugeGrid.AddFigure(x / 9, y / 9, x % 9 / 3, y % 9 / 3, x % 9 % 3, y % 9 % 3, turn); //Add figure
                    HugeGridCurrentPosX = x % 9 / 3; //Set the new available big grid
                    HugeGridCurrentPosY = y % 9 / 3;
                    if (HugeGrid.Board[HugeGridCurrentPosX, HugeGridCurrentPosY].Status != Status.Nothing)
                    {
                        HugeGridCurrentPosX = -1; //If the selected big grid is complete select everything
                        BigGridCurrentPosX = -1;
                    }
                    else
                    {
                        BigGridCurrentPosX = x % 3; //Set the new available small grid
                        BigGridCurrentPosY = y % 3;
                        if (HugeGrid.Board[HugeGridCurrentPosX, HugeGridCurrentPosY].Board[BigGridCurrentPosX, BigGridCurrentPosY].Status != Status.Nothing)
                        {
                            BigGridCurrentPosX = -1; //If the selected small grid is complete select all the small grids in the same big grid
                        }
                    }
                    HugeGrid.Color(HugeGridCurrentPosX, HugeGridCurrentPosY, BigGridCurrentPosX, BigGridCurrentPosY, Brushes.Blue); //Color the new available small grid / big grid / big grids
                    turn = turn == Figure.Cross ? Figure.Circle : Figure.Cross; //Change the turn
                    if (HugeGrid.Status != Status.Nothing)
                    {
                        EndHandler(HugeGrid.Status); //End the game if the huge grid is full
                    }
                }
            }
            await Task.Delay(50); //Wait
            if (turn == Figure.Cross && player1 != null) //Make AIs make choices
            {
                player1.MakeChoice();
            }
            else if (turn == Figure.Circle && player2 != null)
            {
                player2.MakeChoice();
            }
        }

        private void EndHandler(Status status)
        {
            MessageBox.Show(status == Status.Tie ? "It's a tie!" : status == Status.CircleWin ? "Circles win!" : "Crosses win!"); //Show the result
            player1 = null; //Delete the AIs so the game stops
            player2 = null;
        }
    }
}
