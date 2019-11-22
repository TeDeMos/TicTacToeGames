using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class AI
    {
        private GameType gameType;
        private GameManager game;
        private HugeGrid hugeGrid;
        private BigGrid bigGrid;
        private SmallGrid smallGrid;
        private Random r;

        public AI(GameType type)
        {
            gameType = type;
            r = new Random();
        }

        public void AddManager(GameManager gameManager)
        {
            game = gameManager;
        }

        public void AddGrid(HugeGrid hg)
        {
            hugeGrid = hg;
        }
        public void AddGrid(BigGrid bg)
        {
            bigGrid = bg;
        }

        public void AddGrid(SmallGrid sg)
        {
            smallGrid = sg;
        }

        public void MakeChoice()
        {
            IntPoint choice;
            if (gameType == GameType.Small)
            {
                choice = MakeSmallGridChoice(smallGrid);
                game.ClickHandler(choice.X, choice.Y); //Make choice on a small grid
            }
            else if (gameType == GameType.Big)
            {
                choice = MakeBigGridChoice(bigGrid);
                game.ClickHandler(choice.X, choice.Y); //Make choice on a big grid
            }
            else
            {
                choice = MakeHugeGridChoice();
                game.ClickHandler(choice.X, choice.Y); //Make choice on a huge grid
            }
        }

        private IntPoint MakeSmallGridChoice(SmallGrid sg)
        {
            List<IntPoint> choices = new List<IntPoint>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!sg.Board[i, j].HasValue)
                    {
                        choices.Add(new IntPoint(i, j)); //Get all the empty cells
                    }
                }
            }
            return choices[r.Next(choices.Count)]; //Select a random one
        }

        private IntPoint MakeBigGridChoice(BigGrid bg)
        {
            List<IntPoint> choices = new List<IntPoint>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (bg.Board[i, j].Status == Status.Nothing)
                    {
                        choices.Add(new IntPoint(i, j)); //Get all the not complete small grids
                    }
                }
            }
            IntPoint firstChoice = choices[r.Next(choices.Count)]; //Select a random one
            IntPoint secondChoice = MakeSmallGridChoice(bg.Board[firstChoice.X, firstChoice.Y]); //Select a random cell from the chosen small grid
            return new IntPoint(3 * firstChoice.X + secondChoice.X, 3 * firstChoice.Y + secondChoice.Y);
        }

        private IntPoint MakeHugeGridChoice()
        {
            List<IntPoint> choices = new List<IntPoint>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (hugeGrid.Board[i, j].Status == Status.Nothing)
                    {
                        choices.Add(new IntPoint(i, j)); //Get all the not complete big grids
                    }
                }
            }
            IntPoint firstChoice = choices[r.Next(choices.Count)]; //Select a randon one
            IntPoint secondChoice = MakeBigGridChoice(hugeGrid.Board[firstChoice.X, firstChoice.Y]); //Select a random cell from the chosen big grid
            return new IntPoint(9 * firstChoice.X + secondChoice.X, 9 * firstChoice.Y + secondChoice.Y);
        }
    }
}
