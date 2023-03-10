using System;
using System.IO;

namespace MazeMap
{
    public class Maze
    {
        private int[,] _grid;   // 2D array to store maze cells
        private int _rows;      // Number of rows in the maze
        private int _cols;      // Number of columns in the maze
        private int _startRow;  // Row index of the start point
        private int _startCol;  // Column index of the start point
        private int _endRow;    // Row index of the end point
        private int _endCol;    // Column index of the end point

        // Constructor to create an empty maze
        public Maze(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            _grid = new int[rows, cols];
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
            _rows = lines.Length;
            _cols = lines[0].Length;
            _grid = new int[_rows, _cols];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        // Set start point
                        _startRow = i;
                        _startCol = j;
                    }
                    else if (lines[i][j] == 'E')
                    {
                        // Set end point
                        _endRow = i;
                        _endCol = j;
                    }
                    else if (lines[i][j] == '#')
                    {
                        // Set wall
                        _grid[i, j] = 1;
                    }
                    else
                    {
                        // Set empty cell
                        _grid[i, j] = 0;
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
                    else if (i == _endRow && j == _endCol)
                    {
                        Console.Write("E");
                    }
                    else if (_grid[i, j] == 1)
                    {
                        Console.Write("#");
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

        public int EndRow
        {
            get { return _endRow; }
            set { _endRow = value; }
        }

        public int EndCol
        {
            get { return _endCol; }
            set { _endCol = value; }
        }
    }
}