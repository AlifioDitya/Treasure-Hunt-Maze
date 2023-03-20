using BingChilling.Environment;

namespace BingChilling.Algorithms
{
    public class DFS : Solver
    {
        public DFS(Maze maze) : base(maze) { }

        public override List<Node> SearchTreasures(int startX, int startY)
        {
            Stack<Node> stack = new Stack<Node>();
            Node startNode = new Node(startX, startY, null);
            List<Node> fullPath = new List<Node>();
            stack.Push(startNode);

            Console.WriteLine("DFS begins search...");

            while (stack.Count > 0)
            {
                Node currentNode = stack.Pop();

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
                        fullPath = fullPath.Concat(currentNode.ListPath()).ToList();
                        fullPath.RemoveAt(fullPath.Count - 1);
                        fullPath = fullPath.Concat(SearchPathBack(currentNode.X, currentNode.Y)).ToList();
                        return fullPath;
                    }
                    else
                    {
                        visited = new int[maze.Rows, maze.Cols];
                        stack.Clear();
                    }
                }

                // Mark the current node as visited
                visited[currentNode.X, currentNode.Y] = 1;

                // Generate all possible next moves
                List<Node> nextMoves = currentNode.GenerateNextMoves(maze);

                // Add each next move to the stack if it hasn't been visited before
                foreach (Node nextMove in nextMoves)
                {
                    if (visited[nextMove.X, nextMove.Y] != 1)
                    {
                        stack.Push(nextMove);
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
                fullPath = fullPath.Concat(treasureNodes.Last().ListPath()).ToList();
                fullPath.RemoveAt(fullPath.Count - 1);
                fullPath = fullPath.Concat(SearchPathBack(treasureNodes.Last().X, treasureNodes.Last().Y)).ToList();
            }

            return fullPath;
        }

        public override List<Node> SearchPathBack(int startX, int startY)
        {
            Stack<Node> stack = new Stack<Node>();
            Node startNode = new Node(startX, startY, null);
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
                    Console.WriteLine("Path taken: ");
                    Console.WriteLine(currentNode.GetDirections(""));
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
    }
}
