using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe
{
    public class HugeGrid
    {
        public BigGrid[,] Board { get; set; }
        public Status Status { get; set; }
        private List<Line> lines;
        private Grid refGrid;
        private int squareSize;

        public HugeGrid(int cellSize, Grid grid)
        {
            refGrid = grid;
            squareSize = cellSize;
            lines = new List<Line>();
            Board = new BigGrid[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Board[i, j] = new BigGrid(i, j, GameType.Huge, squareSize, refGrid); //Create big grids inside the huge grid
                }
            }
            DrawSelf(); //Draw huge grid
        }

        private void DrawSelf()
        {
            //i * 9 * squareSize (9 * sqaureSpace is the width of the big grid)
            //27 * sqaureSize - width of the huge grid
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(i * 9 * squareSize, 0, i * 9 * squareSize, 27 * squareSize, 10)); //Save the vertical lines in the list
                refGrid.Children.Add(lines[i - 1]); //Draw the lines
            }
            for (int i = 1; i < 3; i++)
            {
                lines.Add(Drawer.Line(0, i * 9 * squareSize, 27 * squareSize, i * 9 * squareSize, 10)); //Save the horizontal lines in the list
                refGrid.Children.Add(lines[i + 1]); //Draw the lines
            }
        }

        public void Color(int x1, int y1, int x2, int y2, Brush color)
        {
            if (x1 == -1)
            {
                foreach (BigGrid b in Board)
                {
                    b.Color(x1, y1, color); //Color all the big grids
                }
                foreach (Line l in lines)
                {
                    refGrid.Children.Remove(l); //Remove the old lines
                    l.Stroke = color; //Color all the lines
                    refGrid.Children.Add(l); //Add the new lines
                }
            }
            else
            {
                Board[x1, y1].Color(x2, y2, color); //Color specified big grid
            }
        }

        private void DrawFigure(int x, int y, Figure figure)
        {
            //x * 9 * squareSize (x - position of the big grid inside the huge grid) - huge grid offset
            //sqaureSize / 4 - Small offset from the left
            //35 * sqareSize / 4 - Big offset from the left to leave space from the right
            //17 * squareSize / 2 - Diameter
            if (figure == Figure.Cross) //Draw to lines forming a cross
            {
                refGrid.Children.Add(Drawer.Line(x * 9 * squareSize + squareSize / 4, y * 9 * squareSize + squareSize / 4, x * 9 * squareSize + 35 * squareSize / 4, y * 9 * squareSize + 35 * squareSize / 4, 5));
                refGrid.Children.Add(Drawer.Line(x * 9 * squareSize + squareSize / 4, y * 9 * squareSize + 35 * squareSize / 4, x * 9 * squareSize + 35 * squareSize / 4, y * 9 * squareSize + squareSize / 4, 5));
            }
            else
            {
                refGrid.Children.Add(Drawer.Circle(x * 9 * squareSize + squareSize / 4, y * 9 * squareSize + squareSize / 4, 17 * squareSize / 2)); //Draw a circle
            }
        }

        public void AddFigure(int x1, int y1, int x2, int y2, int x3, int y3, Figure figure)
        {
            Board[x1, y1].AddFigure(x2, y2, x3, y3, figure); //Call AddFigure in a specified big grid
            if (Board[x1, y1].Status == Status.CrossWin)
            {
                DrawFigure(x1, y1, Figure.Cross); //Draw a huge cross
            }
            else if (Board[x1, y1].Status == Status.CircleWin)
            {
                DrawFigure(x1, y1, Figure.Circle); //Draw a huge circle
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
            //If no one won check if there is a big grid which is not complete
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
            //If all the big grids are complete it is a tie
            return Status.Tie;
        }
    }
}
