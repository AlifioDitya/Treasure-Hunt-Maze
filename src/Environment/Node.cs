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

            // Check left
            if (y > 0 && maze[x, y - 1] != 1)
            {
                nextMoves.Add(new Node(x, y - 1, this));
            }

            // Check down
            if (x < maze.Rows - 1 && maze[x + 1, y] != 1)
            {
                nextMoves.Add(new Node(x + 1, y, this));
            }

            // Check right
            if (y < maze.Cols - 1 && maze[x, y + 1] != 1)
            {
                nextMoves.Add(new Node(x, y + 1, this));
            }

            // Check up
            if (x > 0 && maze[x - 1, Y] != 1)
            {
                nextMoves.Add(new Node(x - 1, y, this));
            }

            return nextMoves;
        }

        public string GetDirections(string currPath)
        {
            if (parent == null)
            {
                return currPath;
            }

            if (x < parent.X)
            {
                return parent.GetDirections("U " + currPath);
            }
            else if (x > parent.X)
            {
                return parent.GetDirections("D " + currPath);
            }
            else if (y < parent.Y)
            {
                return parent.GetDirections("L " + currPath);
            }
            else
            {
                return parent.GetDirections("R " + currPath);
            }
        }

        public List<Node> ListPath()
        {
            List<Node> path = new List<Node>();
            Node? currentNode = this;

            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }

        public Node? Parent
        {
            get { return parent; }
            set { parent = value; }
        }
    }
}
