using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe
{
    public class SmallGrid
    {
        public Figure?[,] Board { get; set; }
        public Status Status { get; set; }
        private int xPos;
        private int yPos;
        private int bigXPos;
        private int bigYPos;
        private List<Line> lines;
        private GameType gameType;
        private Grid refGrid;
        private int squareSize;

        public SmallGrid(int xPosition, int yPosition, int bigXPosition, int bigYPosition, GameType type, int cellSize, Grid grid)
        {
            xPos = xPosition;
            yPos = yPosition;
            bigXPos = bigXPosition;
            bigYPos = bigYPosition;
            gameType = type;
            refGrid = grid;
            squareSize = cellSize;
            lines = new List<Line>();
            Board = new Figure?[3, 3];
            DrawSelf(); //Draw the small grid
        }

        private void DrawSelf()
        {
            //bigXPos * 9 * sqaureSize - offset of the big grid inside the huge grid
            //xPos * 3 * sqaureSize - offset of the small grid inside the big grid
            //3 * sqaureSize - width of the small grid
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(bigXPos * 9 * squareSize + xPos * 3 * squareSize + i * squareSize, bigYPos * 9 * squareSize + yPos * 3 * squareSize, bigXPos * 9 * squareSize + xPos * 3 * squareSize + i * squareSize, bigYPos * 9 * squareSize + yPos * 3 * squareSize + 3 * squareSize, 2)); //Add the vertical lines to the list
                refGrid.Children.Add(lines[i - 1]); //Draw the lines
            }
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(bigXPos * 9 * squareSize + xPos * 3 * squareSize, bigYPos * 9 * squareSize + yPos * 3 * squareSize + i * squareSize, bigXPos * 9 * squareSize + xPos * 3 * squareSize + 3 * squareSize, bigYPos * 9 * squareSize + yPos * 3 * squareSize + i * squareSize, 2)); //Add the horizontal lines to the list
                refGrid.Children.Add(lines[i + 1]); //Draw the lines
            }
        }

        public void Color(Brush color)
        {
            for (int i = 0; i < 4; i++)
            {
                refGrid.Children.Remove(lines[i]); //Remove the old lines
                lines[i].Stroke = color; //Color the lines
                refGrid.Children.Add(lines[i]); //Add the new lines
            }
        }

        private void DrawFigure(int x, int y, Figure figure)
        {
            //bigXPos * 9 * squareSize - offset of the big grid inside the huge grid
            //xPos * 3 * squareSize - offset of the small grid inside the big grid
            //sqareSize * x - offset of a cell inside the small grid
            //sqaureSize / 4 - small offset from the left
            //3 * sqaureSize / 4 - big offset from the left to leave space from the right
            if (figure == Figure.Cross) //Draw to lines to form a cross
            {
                refGrid.Children.Add(Drawer.Line(bigXPos * 9 * squareSize + xPos * 3 * squareSize + squareSize * x + squareSize / 4, bigYPos * 9 * squareSize + yPos * 3 * squareSize + squareSize * y + squareSize / 4, bigXPos * 9 * squareSize + xPos * 3 * squareSize + squareSize * x + 3 * squareSize / 4, bigYPos * 9 * squareSize + yPos * 3 * squareSize + squareSize * y + 3 * squareSize / 4, 5));
                refGrid.Children.Add(Drawer.Line(bigXPos * 9 * squareSize + xPos * 3 * squareSize + squareSize * x + squareSize / 4, bigYPos * 9 * squareSize + yPos * 3 * squareSize + squareSize * y + 3 * squareSize / 4, bigXPos * 9 * squareSize + xPos * 3 * squareSize + squareSize * x + 3 * squareSize / 4, bigYPos * 9 * squareSize + yPos * 3 * squareSize + squareSize * y + squareSize / 4, 5));
            }
            else
            {
                refGrid.Children.Add(Drawer.Circle(bigXPos * 9 * squareSize + xPos * 3 * squareSize + x * squareSize + squareSize / 4, bigYPos * 9 * squareSize + yPos * 3 * squareSize + y * squareSize + squareSize / 4, squareSize / 2)); //Draw a circle
            }
        }

        public void AddFigure(int x, int y, Figure figure)
        {
            Board[x, y] = figure; //Add the figure
            DrawFigure(x, y, figure); //Draw the figure
            Status = CheckStatus();
        }

        public Status CheckStatus()
        {
            //[0, i], [1, i], [2, i] - Horizontal
            //[i, 0], [i, 1], [i, 2] - Vertical
            //[0, 0], [1, 1], [2, 2] - Top-Left - Down-Right Diagonal
            //[0, 2], [1, 1], [2, 0] - Bottom-Left - Top-Right Diagonal
            //Check for cross / circle win
            for (int i = 0; i < 3; i++)
            {
                if (Board[0, i] == Figure.Cross && Board[1, i] == Figure.Cross && Board[2, i] == Figure.Cross || Board[i, 0] == Figure.Cross && Board[i, 1] == Figure.Cross && Board[i, 2] == Figure.Cross)
                {
                    return Status.CrossWin;
                }
            }
            if (Board[0, 0] == Figure.Cross && Board[1, 1] == Figure.Cross && Board[2, 2] == Figure.Cross || Board[0, 2] == Figure.Cross && Board[1, 1] == Figure.Cross && Board[2, 0] == Figure.Cross)
            {
                return Status.CrossWin;
            }

            for (int i = 0; i < 3; i++)
            {
                if (Board[0, i] == Figure.Circle && Board[1, i] == Figure.Circle && Board[2, i] == Figure.Circle || Board[i, 0] == Figure.Circle && Board[i, 1] == Figure.Circle && Board[i, 2] == Figure.Circle)
                {
                    return Status.CircleWin;
                }
            }
            if (Board[0, 0] == Figure.Circle && Board[1, 1] == Figure.Circle && Board[2, 2] == Figure.Circle || Board[0, 2] == Figure.Circle && Board[1, 1] == Figure.Circle && Board[2, 0] == Figure.Circle)
            {
                return Status.CircleWin;
            }
            //If no one won check if there is a cell which is not filled
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!Board[i, j].HasValue)
                    {
                        return Status.Nothing;
                    }
                }
            }
            //If all the cells are filled it is a tie
            return Status.Tie;
        }
    }
}
