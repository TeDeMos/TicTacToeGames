using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe
{
    public class Drawer
    {
        //Class for drawing stuff
        public static Line Line(int x1, int y1, int x2, int y2, int strokeThickness)
        {
            Line l = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = strokeThickness
            };
            return l;
        }

        public static Ellipse Circle(int x, int y, int diameter)
        {
            Ellipse e = new Ellipse
            {
                Margin = new Thickness(x, y, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = diameter,
                Height = diameter,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };
            return e;
        }

        public static Rectangle Square(int x, int y, int sideLenght, Brush fillColor)
        {
            Rectangle r = new Rectangle
            {
                Width = sideLenght,
                Height = sideLenght,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0),
                Fill = fillColor,
                Stroke = fillColor
            };
            return r;
        }
    }
}
