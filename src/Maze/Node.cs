
namespace MazeMap
{

    public class Node
    {
        private int _row;               // Row index of the node
        private int _col;               // Column index of the node
        private List<Node> _neighbors;  // List of neighboring nodes
        private bool _visited;          // Status of node (visited or unvisited)

        // Constructor to create a node
        public Node(int row, int col)
        {
            _row = row;
            _col = col;
            _neighbors = new List<Node>();
            _visited = false;
        }

        // Getters and setters for node properties
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        public List<Node> Neighbors
        {
            get { return _neighbors; }
            set { _neighbors = value; }
        }

        public bool Visited
        {
            get { return _visited; }
            set { _visited = value; }
        }
    }
}