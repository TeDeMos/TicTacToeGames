namespace TicTacToe
{
    public class IntPoint
    {
        //A Point but it is not double so I don't have to parse every time
        public int X { get; set; }
        public int Y { get; set; }

        public IntPoint(int xPos, int yPos)
        {
            X = xPos;
            Y = yPos;
        }
    }
}
