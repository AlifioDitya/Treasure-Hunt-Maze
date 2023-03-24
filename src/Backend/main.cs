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
            string path = @"Set path to .txt file here";

            // Load maze
            try
            {
                maze.Load(path);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

            //Search with BFS
            BFS bfs = new BFS(maze);
            List<Node> bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol, true);
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
            List<Node> dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol, true);
            DFS cleanDFS = new DFS(maze);
            List<Node> cleanDFSPath = cleanDFS.SearchTreasures(maze.StartRow, maze.StartCol, true, true);
            Console.WriteLine();

            if (cleanDFSPath.Count > 0)
            {
                Console.WriteLine("DFS Path with Backtrack: ");
                for (int i = 0; i < dfsPath.Count; i++)
                {
                    Console.WriteLine("({0}, {1})", dfsPath[i].X, dfsPath[i].Y);
                }
                Console.WriteLine();
                Console.WriteLine("Clean DFS Path: ");
                for (int i = 0; i < cleanDFSPath.Count; i++)
                {
                    Console.WriteLine("({0}, {1})", cleanDFSPath[i].X, cleanDFSPath[i].Y);
                }
            }

            Console.ReadLine();
        }
    }
}
