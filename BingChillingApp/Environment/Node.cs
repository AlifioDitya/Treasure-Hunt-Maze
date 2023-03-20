namespace BingChilling.Environment
{
    public class Node
    {
        private int x;
        private int y;
        private Node? parent;

        public Node(int x, int y, Node? parent)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public List<Node> GenerateNextMoves(Maze maze)
        {
            List<Node> nextMoves = new List<Node>();

            // Check up
            if (x > 0 && maze[x - 1, Y] != 1)
            {
                nextMoves.Add(new Node(x - 1, y, this));
            }

            // Check down
            if (x < maze.Rows - 1 && maze[x + 1, y] != 1)
            {
                nextMoves.Add(new Node(x + 1, y, this));
            }

            // Check left
            if (y > 0 && maze[x, y - 1] != 1)
            {
                nextMoves.Add(new Node(x, y - 1, this));
            }

            // Check right
            if (y < maze.Cols - 1 && maze[x, y + 1] != 1)
            {
                nextMoves.Add(new Node(x, y + 1, this));
            }

            return nextMoves;
        }

        public void PrintPath()
        {
            if (this == null)
            {
                return;
            }

            if (parent == null)
            { 
                Console.WriteLine("({0}, {1})", x, y);
            }
            else
            {
                parent.PrintPath();
                Console.WriteLine("({0}, {1})", x, y);
            }
        }

        public string GetDirections(string currPath)
        {
            if (parent == null)
            {
                return currPath;
            }

            if (x < parent.X)
            {
                return parent.GetDirections("U" + currPath);
            }
            else if (x > parent.X)
            {
                return parent.GetDirections("D" + currPath);
            }
            else if (y < parent.Y)
            {
                return parent.GetDirections("L" + currPath);
            }
            else
            {
                return parent.GetDirections("R" + currPath);
            }
        }
    }
}
