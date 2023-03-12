using BingChilling.Environment;

namespace BingChilling.Algorithms
{
    public abstract class Solver
    {
        protected Maze maze;
        protected int[,] visited;
        protected List<Node> treasureNodes;

        public Solver(Maze maze)
        {
            this.maze = maze;
            visited = new int[maze.Rows, maze.Cols];
            treasureNodes = new List<Node>();
        }

        public abstract void Search(int startX, int startY);

    }
}
