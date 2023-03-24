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

        // Method to search every reachable treasure in the maze
        public abstract List<Node> SearchTreasures(int startX, int startY, bool tsp=false, bool reset=false);

        // Method to trace back a path to the starting point
        public abstract List<Node> SearchPathBack(Node startNode);

    }
}
