namespace mouseHover
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }

        public Position(int x, int y, string direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}