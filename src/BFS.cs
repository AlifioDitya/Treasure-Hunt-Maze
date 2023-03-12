using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MazeEnv;

namespace Solver
{
    public class BFS
    {
        private Maze maze;
        private int[,] visited;
        private List<Node> treasureNodes;

        public BFS(Maze maze)
        {
            this.maze = maze;
            visited = new int[maze.Rows, maze.Cols];
            treasureNodes = new List<Node>();
        }

        public void Search(int startX, int startY)
        {
            Queue<Node> queue = new Queue<Node>();
            Node startNode = new Node(startX, startY, null);
            Node currentNode = startNode;
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();

                // If the current node is a treasure
                if (maze[currentNode.X, currentNode.Y] == 2 && !treasureNodes.Any(node => node.X == currentNode.X && node.Y == currentNode.Y))
                {
                    Console.WriteLine("Treasure found at ({0}, {1})!", currentNode.X, currentNode.Y);
                    treasureNodes.Add(currentNode);

                    // If all treasures have been found, stop the search
                    if (treasureNodes.Count == maze.Treasures)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Treasures Found: {0}", treasureNodes.Count);
                        Console.WriteLine("Path taken: ");
                        currentNode.PrintPath();
                        return;
                    }
                    else
                    {
                        visited = new int[maze.Rows, maze.Cols];
                        queue.Clear();
                    }
                }

                // Mark the current node as visited
                visited[currentNode.X, currentNode.Y] = 1;

                // Generate all possible next moves
                List<Node> nextMoves = currentNode.GenerateNextMoves(maze);

                // Add each next move to the queue if it hasn't been visited before
                foreach (Node nextMove in nextMoves)
                {
                    if (visited[nextMove.X, nextMove.Y] != 1)
                    {
                        queue.Enqueue(nextMove);
                    }
                }
            }

            if (treasureNodes.Count == 0)
            {
                Console.WriteLine("All treasures not found.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Treasures Found: {0}", treasureNodes.Count);
                Console.WriteLine("Path taken: ");
                treasureNodes.Last().PrintPath();
            }
        }
    }
}

