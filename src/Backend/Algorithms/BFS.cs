﻿using BingChilling.Environment;

namespace BingChilling.Algorithms
{
    public class BFS : Solver
    {

        public BFS(Maze maze) : base(maze) { }

        public override List<Node> SearchTreasures(int startX, int startY, bool tsp=false, bool reset=false)
        {
            Queue<Node> queue = new Queue<Node>();
            Node startNode = new Node(startX, startY, null);
            List<Node> fullPath = new List<Node>();
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

                        if (tsp)
                        {
                            fullPath = fullPath.Concat(SearchPathBack(currentNode)).ToList();
                        }
                        else
                        {
                            fullPath = fullPath.Concat(currentNode.ListPath()).ToList();
                        }

                        Console.WriteLine("Path taken: ");
                        Console.WriteLine(fullPath.Last().GetDirections(""));
                        return fullPath;
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

                if (tsp)
                {
                    fullPath = fullPath.Concat(SearchPathBack(treasureNodes.Last())).ToList();
                }
                else
                {
                    fullPath = fullPath.Concat(treasureNodes.Last().ListPath()).ToList();
                }

                Console.WriteLine("Path taken: ");
                Console.WriteLine(fullPath.Last().GetDirections(""));
            }

            return fullPath;
        }

        public override List<Node> SearchPathBack(Node startNode)
        {
            Queue<Node> queue = new Queue<Node>();
            visited = new int[maze.Rows, maze.Cols];
            Node currentNode = startNode;
            queue.Enqueue(startNode);

            Console.WriteLine();
            Console.WriteLine("BFS traces way back...");
            Console.WriteLine();

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();

                if (currentNode.X == maze.StartRow && currentNode.Y == maze.StartCol)
                {
                    return currentNode.ListPath();
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

            return currentNode.ListPath();
        }

    }
}

