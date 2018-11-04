using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe
{
    public class BigGrid
    {
        public SmallGrid[,] Board { get; set; }
        public Status Status { get; set; }
        private int xPos;
        private int yPos;
        private List<Line> lines;
        private GameType gameType;
        private Grid refGrid;
        private int squareSize;

        public BigGrid(int xPosition, int yPosition, GameType type, int cellSize, Grid grid)
        {
            xPos = xPosition;
            yPos = yPosition;
            gameType = type;
            refGrid = grid;
            squareSize = cellSize;
            Board = new SmallGrid[3, 3];
            lines = new List<Line>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = new SmallGrid(i, j, xPos, yPos, gameType, squareSize, refGrid); //Create small grids inside the big grid
                }
            }
            DrawSelf(); //Draw big grid
        }

        private void DrawSelf()
        {
            //xPos * 9 * sqaureSize - Offset of the big grid inside the huge grid
            //i * 3 * sqaureSize (3 * squareSize is the width of a small grid
            //9 * squareSize - width of the big board
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(xPos * 9 * squareSize + i * 3 * squareSize, yPos * 9 * squareSize, xPos * 9 * squareSize + i * 3 * squareSize, yPos * 9 * squareSize + 9 * squareSize, 5)); //Save the vertical lines in the list
                refGrid.Children.Add(lines[i - 1]); //Draw the lines
            }
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(xPos * 9 * squareSize, yPos * 9 * squareSize + i * 3 * squareSize, xPos * 9 * squareSize + 9 * squareSize, yPos * 9 * squareSize + i * 3 * squareSize, 5)); //Save the horizontal lines in the list
                refGrid.Children.Add(lines[i + 1]); //Draw the lines
            }
        }

        public void Color(int x, int y, Brush color)
        {
            if (x == -1)
            {
                foreach (SmallGrid s in Board)
                {
                    s.Color(color); //Color all the small grids
                }

                foreach (Line l in lines)
                {
                    refGrid.Children.Remove(l); //Remove the old lines
                    l.Stroke = color; //Color the lines
                    refGrid.Children.Add(l); //Add the new lines
                }
            }
            else
            {
                Board[x, y].Color(color); //Color the specified small grid
            }
        }

        private void DrawFigure(int x, int y, Figure figure)
        {
            //xPos * 9 * sqaureSize - offset of the big grid inside the huge grid
            //x * 3 * sqaureSize - offset of the small grid inside the big grid
            //sqaureSize / 4 - small offset from the left
            //11 * sqaureSize / 4 - bigoffset from the left to leave space on the right
            //5 * sqaureSize / 2 - diameter
            if (figure == Figure.Cross) //Draw two lines forming a cross
            {
                refGrid.Children.Add(Drawer.Line(xPos * 9 * squareSize + x * 3 * squareSize + squareSize / 4, yPos * 9 * squareSize + y * 3 * squareSize + squareSize / 4, xPos * 9 * squareSize + x * 3 * squareSize + 11 * squareSize / 4, yPos * 9 * squareSize + y * 3 * squareSize + 11 * squareSize / 4, 5));
                refGrid.Children.Add(Drawer.Line(xPos * 9 * squareSize + x * 3 * squareSize + squareSize / 4, yPos * 9 * squareSize + y * 3 * squareSize + 11 * squareSize / 4, xPos * 9 * squareSize + x * 3 * squareSize + 11 * squareSize / 4, yPos * 9 * squareSize + y * 3 * squareSize + squareSize / 4, 5));
            }
            else
            {
                refGrid.Children.Add(Drawer.Circle(xPos * 9 * squareSize + x * 3 * squareSize + squareSize / 4, yPos * 9 * squareSize + y * 3 * squareSize + squareSize / 4, squareSize * 5 / 2)); //Draw a circle
            }
        }

        public void AddFigure(int x1, int y1, int x2, int y2, Figure figure)
        {
            Board[x1, y1].AddFigure(x2, y2, figure); //Call AddFigure in the specified small grid
            if (Board[x1, y1].Status == Status.CrossWin)
            {
                DrawFigure(x1, y1, Figure.Cross); //Draw a big cross
            }
            else if (Board[x1, y1].Status == Status.CircleWin)
            {
                DrawFigure(x1, y1, Figure.Circle); //Draw a big circle
            }
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
                if (Board[0, i].Status == Status.CrossWin && Board[1, i].Status == Status.CrossWin && Board[2, i].Status == Status.CrossWin || Board[i, 0].Status == Status.CrossWin && Board[i, 1].Status == Status.CrossWin && Board[i, 2].Status == Status.CrossWin)
                {
                    return Status.CrossWin;
                }
            }
            if (Board[0, 0].Status == Status.CrossWin && Board[1, 1].Status == Status.CrossWin && Board[2, 2].Status == Status.CrossWin || Board[0, 2].Status == Status.CrossWin && Board[1, 1].Status == Status.CrossWin && Board[2, 0].Status == Status.CrossWin)
            {
                return Status.CrossWin;
            }

            for (int i = 0; i < 3; i++)
            {
                if (Board[0, i].Status == Status.CircleWin && Board[1, i].Status == Status.CircleWin && Board[2, i].Status == Status.CircleWin || Board[i, 0].Status == Status.CircleWin && Board[i, 1].Status == Status.CircleWin && Board[i, 2].Status == Status.CircleWin)
                {
                    return Status.CircleWin;
                }
            }
            if (Board[0, 0].Status == Status.CircleWin && Board[1, 1].Status == Status.CircleWin && Board[2, 2].Status == Status.CircleWin || Board[0, 2].Status == Status.CircleWin && Board[1, 1].Status == Status.CircleWin && Board[2, 0].Status == Status.CircleWin)
            {
                return Status.CircleWin;
            }
            //If no one won check if there is a small grid which is not complete
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j].Status == Status.Nothing)
                    {
                        return Status.Nothing;
                    }
                }
            }
            //If all the small grids are complete it is a tie
            return Status.Tie;
        }
    }
}
