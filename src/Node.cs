using MazeEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeEnv
{
    internal class Node
    {
        private int x;
        private int y;
        private Node parent;

        public Node(int x, int y, Node parent)
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

        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public List<Node> GenerateNextMoves(Maze maze)
        {
            List<Node> nextMoves = new List<Node>();

            // Check up
            if (this.x > 0 && maze[this.x - 1, this.Y] != 1)
            {
                nextMoves.Add(new Node(this.x - 1, this.y, this));
            }

            // Check down
            if (this.x < maze.Rows - 1 && maze[this.x + 1, this.y] != 1)
            {
                nextMoves.Add(new Node(this.x + 1, this.y, this));
            }

            // Check left
            if (this.y > 0 && maze[this.x, this.y - 1] != 1)
            {
                nextMoves.Add(new Node(this.x, this.y - 1, this));
            }

            // Check right
            if (this.y < maze.Cols - 1 && maze[this.x, this.y + 1] != 1)
            {
                nextMoves.Add(new Node(this.x, this.y + 1, this));
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
                Console.WriteLine("({0}, {1})", this.x, this.y);
            }
            else
            {
                this.parent.PrintPath();
                Console.WriteLine("({0}, {1})", this.x, this.y);
            }
        }
    }
}
