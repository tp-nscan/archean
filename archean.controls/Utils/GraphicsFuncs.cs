using System.Windows;

namespace archean.controls.Utils
{
    public static class GraphicsFuncs
    {
        public static Point Interpolate(this Point start, Point end, double position)
        {
            var dX = (end.X - start.X) * position;
            var dY = (end.Y - start.Y) * position;

            return new Point(x: start.X + dX, y: start.Y + dY);
        }
    }
}
