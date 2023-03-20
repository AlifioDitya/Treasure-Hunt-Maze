﻿using BingChilling.Environment;

namespace BingChilling.Algorithms
{
    public class BFS : Solver
    {

        public BFS(Maze maze) : base(maze) { }

        public override void Search(int startX, int startY)
        {
            Queue<Node> queue = new Queue<Node>();
            Node startNode = new Node(startX, startY, null);
            queue.Enqueue(startNode);

            Console.WriteLine("BFS begins search...");

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

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
                        Console.WriteLine(currentNode.GetDirections(""));
                        SearchBack(currentNode.X, currentNode.Y);
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
                Console.WriteLine(treasureNodes.Last().GetDirections(""));
                SearchBack(treasureNodes.Last().X, treasureNodes.Last().Y);
            }
        }

        public void SearchBack(int startX, int startY)
        {
            Queue<Node> queue = new Queue<Node>();
            Node startNode = new Node(startX, startY, null);
            int[,] visited = new int[maze.Rows, maze.Cols];
            queue.Enqueue(startNode);

            Console.WriteLine();
            Console.WriteLine("BFS traces way back...");
            Console.WriteLine();

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode.X == maze.StartRow && currentNode.Y == maze.StartCol)
                {
                    Console.WriteLine("Path taken: ");
                    Console.WriteLine(currentNode.GetDirections(""));
                    return;
                } 
                else
                {
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
            }
        }
    }
}

