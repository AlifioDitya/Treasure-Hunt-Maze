using System;
using MazeMap;

namespace BingChilling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new 5x5 maze
            Maze maze = new Maze(5, 5);

            // Load maze data from file
            maze.Load("maze.txt");

            // Display the maze
            maze.Display();

            // Create start and end nodes
            //Node start = new Node(maze.StartRow, maze.StartCol);
            //Node end = new Node(maze.EndRow, maze.EndCol);
        }
    }
}
