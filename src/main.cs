using BingChilling.Algorithms;
using BingChilling.Environment;

namespace BingChilling
{
    class Program
    {
        public static void Main()
        {
            // Create empty maze
            Maze maze = new Maze(0, 0);

            // Load maze
            maze.Load(@"/Users/irsyadnb/Downloads/ITB Jaya/SMT 4/STIMA/Tubes2_BingChilling/src/maze.txt");

            // Search with BFS
            BFS bfs = new BFS(maze);
            List<Node> bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol);
            Console.WriteLine();

            if (bfsPath.Count > 0)
            {
                Console.WriteLine("BFS Path: ");
                for (int i = 0; i < bfsPath.Count; i++)
                {
                    Console.WriteLine("({0}, {1})", bfsPath[i].X, bfsPath[i].Y);
                }
                Console.WriteLine();
            }

            // Search with DFS
            DFS dfs = new DFS(maze);
            List<Node> dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol);
            Console.WriteLine();

            if (dfsPath.Count > 0)
            {
                Console.WriteLine("DFS Path: ");
                for (int i = 0; i < dfsPath.Count; i++)
                {
                    Console.WriteLine("({0}, {1})", dfsPath[i].X, dfsPath[i].Y);
                }
            }


            Console.ReadLine();
        }
    }
}
