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
            maze.Load(@"C:\\Users\\Fio\\source\\repos\\BingChilling\\Tubes2_BingChilling\\src\\maze.txt");

            // Search with BFS
            BFS bfs = new BFS(maze);
            bfs.Search(maze.StartRow, maze.StartCol);
            Console.WriteLine();

            // Search with DFS
            DFS dfs = new DFS(maze);
            dfs.Search(maze.StartRow, maze.StartCol);
            Console.ReadLine();
        }
    }
}
