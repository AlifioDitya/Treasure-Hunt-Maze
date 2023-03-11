using Solver;
using MazeEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingChilling
{
    class Program
    {
        public static void Main()
        {
            // Maze grid
            int[,] grid = new int[,] {
                {0, 1, 0, 0, 2},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 1, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 0}
            };

            // Starting position of the maze
            int startX = 0;
            int startY = 0;

            // Create empty maze
            Maze maze = new Maze(0, 0, startX, startY);

            // Load maze
            maze.Load(@"C:\\Users\\Fio\\source\\repos\\BingChilling\\Tubes2_BingChilling\\src\\maze.txt");

            // Search with BFS
            BFS bfs = new BFS(maze);
            bfs.Search(startX, startY);
            Console.ReadLine();
        }
    }
}
