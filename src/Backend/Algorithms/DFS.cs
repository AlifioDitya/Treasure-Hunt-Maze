﻿using BingChilling.Environment;

namespace BingChilling.Algorithms
{
    public class DFS : Solver
    {
        public DFS(Maze maze) : base(maze) { }

        public override List<Node> SearchTreasures(int startX, int startY, bool tsp=false, bool reset=false)
        {
            Stack<Node> stack = new Stack<Node>();
            Node startNode = new Node(startX, startY, null);
            List<Node> fullPath = new List<Node>();
            stack.Push(startNode);

            Console.WriteLine("DFS begins search...");

            while (stack.Count > 0)
            {
                Node currentNode = stack.Pop();

                // If reset mode is off, nodes are added sequentially
                if (!reset)
                {
                    fullPath.Add(currentNode);
                }

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
                            if (reset)
                            {
                                fullPath = fullPath.Concat(SearchPathBack(currentNode)).ToList();
                            }
                            else
                            {
                                fullPath = fullPath.Concat(TSPWithBacktrack(currentNode)).ToList();
                            }
                        }
                        else if (reset)
                        {
                            fullPath = fullPath.Concat(currentNode.ListPath()).ToList();
                        }

                        Console.WriteLine("Path taken: ");
                        Console.WriteLine(fullPath.Last().GetDirections(""));
                        return fullPath;
                    }
                    else if (reset)
                    {
                        visited = new int[maze.Rows, maze.Cols];
                        stack.Clear();
                    }
                }

                // Mark the current node as visited
                visited[currentNode.X, currentNode.Y] = 1;

                // Generate all possible next moves
                List<Node> nextMoves = currentNode.GenerateNextMoves(maze);

                int neighbours = 0;

                // Add each next move to the stack if it hasn't been visited before
                foreach (Node nextMove in nextMoves)
                {
                    if (visited[nextMove.X, nextMove.Y] != 1)
                    {
                        stack.Push(nextMove);
                        neighbours++;
                    }
                }

                // Backtracking case if reset mode is off
                if (!reset && neighbours == 0 && currentNode.Parent != null)
                {
                    stack.Push(currentNode.Parent);
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
                    if (reset)
                    {
                        fullPath = fullPath.Concat(SearchPathBack(treasureNodes.Last())).ToList();
                    }
                    else
                    {
                        fullPath = fullPath.Concat(TSPWithBacktrack(treasureNodes.Last())).ToList();
                    }
                }
                else if (reset)
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
            Stack<Node> stack = new Stack<Node>();
            visited = new int[maze.Rows, maze.Cols];
            Node currentNode = startNode;
            stack.Push(startNode);

            Console.WriteLine();
            Console.WriteLine("DFS traces way back...");
            Console.WriteLine();

            while (stack.Count > 0)
            {
                currentNode = stack.Pop();

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
                            stack.Push(nextMove);
                        }
                    }
                }
            }

            return currentNode.ListPath();
        }

        public List<Node> TSPWithBacktrack(Node startNode)
        {
            Stack<Node> stack = new Stack<Node>();
            List<Node> fullPath = new List<Node>();
            Node currentNode;
            stack.Push(startNode);

            Console.WriteLine();
            Console.WriteLine("DFS traces way back...");
            Console.WriteLine();

            bool first = true;

            while (stack.Count > 0)
            {
                currentNode = stack.Pop();

                if (first)
                {
                    first = false;
                }
                else
                {
                    fullPath.Add(currentNode);
                }

                if (currentNode.X == maze.StartRow && currentNode.Y == maze.StartCol)
                {
                    return fullPath;
                }
                else
                {
                    // Mark the current node as visited
                    visited[currentNode.X, currentNode.Y] = 1;

                    // Generate all possible next moves
                    List<Node> nextMoves = currentNode.GenerateNextMoves(maze);

                    int neighbours = 0;

                    // Add each next move to the queue if it hasn't been visited before
                    foreach (Node nextMove in nextMoves)
                    {
                        if (visited[nextMove.X, nextMove.Y] != 1)
                        {
                            stack.Push(nextMove);
                            neighbours++;
                        }
                    }

                    // Backtracking case
                    if (neighbours == 0 && currentNode.Parent != null)
                    {
                        stack.Push(currentNode.Parent);
                    }
                }
            }

            return fullPath;
        }
    }
}
