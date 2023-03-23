namespace BingChilling.Environment
{
    public class Maze
    {
        private int[,] _grid;   // 2D array to store maze cells
        private int _rows;      // Number of rows in the maze
        private int _cols;      // Number of columns in the maze
        private int _startRow;  // Row index of the start point
        private int _startCol;  // Column index of the start point
        private int _treasures; // Number of treasures inside the maze

        // Constructor to create an empty maze
        public Maze(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            _grid = new int[rows, cols];
        }

        // Indexer
        public int this[int row, int col]
        {
            get { return _grid[row, col]; }
            set { _grid[row, col] = value; }
        }

        // Method to load a maze from a file
        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Maze file not found.", filePath);
            }

            // Read maze data from file
            string[] lines = File.ReadAllLines(filePath);
            lines = lines.Select(line => line.Replace(" ", "")).ToArray();

            _rows = lines.Length;
            _cols = lines[0].Length;
            _grid = new int[_rows, _cols];

            int startCount = 0;

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (lines[i][j] == 'K')
                    {
                        if (startCount == 0)
                        {
                            // Set start point
                            _startRow = i;
                            _startCol = j;
                            _grid[i, j] = 0;
                            startCount++;
                        }
                        else
                        {
                            throw new InvalidMazeFormatException("Error parsing maze file.");
                        }
                    }
                    else if (lines[i][j] == 'T')
                    {
                        // Set end point
                        _grid[i, j] = 2;
                        _treasures++;
                    }
                    else if (lines[i][j] == 'X')
                    {
                        // Set wall
                        _grid[i, j] = 1;
                    }
                    else if (lines[i][j] == 'R')
                    {
                        // Set empty cell
                        _grid[i, j] = 0;
                    }
                    else
                    {
                        throw new InvalidMazeFormatException("Error parsing maze file.");
                    }
                }
            }
        }

        // Method to display the maze on the console
        public void Display()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (i == _startRow && j == _startCol)
                    {
                        Console.Write("S");
                    }
                    else if (_grid[i, j] == 2)
                    {
                        Console.Write("T");
                    }
                    else if (_grid[i, j] == 1)
                    {
                        Console.Write("X");
                    }
                    else if (_grid[i, j] == -1)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.WriteLine();
            }
        }

        // Getters and setters for maze properties
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public int Cols
        {
            get { return _cols; }
            set { _cols = value; }
        }

        public int StartRow
        {
            get { return _startRow; }
            set { _startRow = value; }
        }

        public int StartCol
        {
            get { return _startCol; }
            set { _startCol = value; }
        }

        public int Treasures
        {
            get { return _treasures; }
            set { _treasures = value; }
        }
    }

    public class InvalidMazeFormatException : Exception
    {
        public InvalidMazeFormatException(string message) : base(message)
        {
        }

        public InvalidMazeFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}