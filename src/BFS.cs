using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MazeEnv;

namespace Solver
{
    public class BFS
    {
        private Maze maze;
        private int[,] visited = new int[Maze._rows, Maze._cols];

        public BFS(Maze maze)
        {
            this.maze = maze;
        }

        public void Search(int startX, int startY)
        {
            Queue<Node> queue = new Queue<Node>();
            Node startNode = new Node(startX, startY, null);
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                // If the current node is the goal
                if (maze[currentNode.X, currentNode.Y] == 2)
                {
                    Console.WriteLine("Goal found!");
                    currentNode.PrintPath();
                    return;
                }

                // Otherwise, mark the current node as visited
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

            Console.WriteLine("Goal not found.");
        }
    }
}

