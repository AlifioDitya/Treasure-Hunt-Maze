using BingChilling.Environment;
using Microsoft.Maui.Controls;
//using static Android.InputMethodServices.Keyboard;


namespace BingChillingGUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private string filePath;
        private async void Button_Clicked(object sender, EventArgs e)
        {
            PickOptions options = new PickOptions();
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                    {
                        this.filePath = result.FullPath;
                        using var ctrstream = await result.OpenReadAsync();
                        using var ctrreader = new StreamReader(ctrstream);
                        int row = 0, col = 0;
                        //int[,] matrix = new int[6, 6]; // Fixed dimensions for the example maze

                        string ctrline;
                        while ((ctrline = ctrreader.ReadLine()) != null)
                        {
                            col = 0;
                            foreach (char c in ctrline)
                            {
                                col++;
                            }
                            row++;
                        }

                        int[,] matrix = new int[row, col];
                       

                        row = 0;

                        using var stream = await result.OpenReadAsync();
                        using var reader = new StreamReader(stream);
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            col = 0;
                            foreach (char c in line)
                            {
                                if (c == 'X')
                                    matrix[row, col] = -1; // Wall
                                else if (c == 'R')
                                    matrix[row, col] = 1; // Road
                                else if (c == 'T')
                                    matrix[row, col] = 2; // Treasure
                                else if (c == 'K')
                                    matrix[row, col] = 0;
                                col++;
                            }
                            row++;
                        }

                        for (int i = 0; i < matrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                Console.Write(matrix[i, j] + " ");
                            }
                            Console.WriteLine();
                        }
                        DisplayMaze(matrix, row, col);
                    }
                }

            }
            catch (Exception ex) //
            {
                // The user canceled or something went wrong
            }
        }

        private void DisplayMaze(int[,] matrix, int numberOfRows, int numberOfColumns)
        {

            // Create a new grid to hold the maze
            var rowDefinitions = new RowDefinitionCollection();
            var grid = new Grid
            {
                ColumnSpacing = 1,
                RowSpacing = 1,
                BackgroundColor = Color.FromRgb(255, 255, 255),
                //VerticalOptions = LayoutOptions.FillAndExpand,
               // HorizontalOptions = LayoutOptions.FillAndExpand,
                
            };
            for (int i = 0; i < numberOfRows; i++)
            {
                rowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            var columnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < numberOfColumns; i++)
            {
                columnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create the Grid and set the RowDefinitions and ColumnDefinitions

            grid.RowDefinitions = rowDefinitions;
            grid.ColumnDefinitions = columnDefinitions;

            // Loop through the matrix and create a label for each element
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Create a new label for this element
                    var label = new Label
                    {
                        Text = matrix[i, j] == 2 ? "Treasure" : matrix[i, j] == 0 ? "Start" : "",
                        TextColor = matrix[i, j] == 2 ? Color.FromRgb(0, 0, 0) : matrix[i, j] == 0 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        BackgroundColor = matrix[i, j] == -1 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    };

                    // Add the label to the grid
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    grid.Children.Add(label);
                }
            }

            // Set the content of the mazeView to the grid
            mazeView.Content = grid;
        }



        private async void visu_Clicked(object sender, EventArgs e)
        {
            BingChilling.Environment.Maze maze = new BingChilling.Environment.Maze(0, 0);
            maze.Load(this.filePath);
            if (bfsCheckBox.IsChecked && dfsCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please select only 1 checkbox to run the algorithm.", "OK");
            }
            else if (bfsCheckBox.IsChecked)
            {
                //await DisplayAlert("BFS", "", "OK");
                //Run BFS algorithm
                BingChilling.Algorithms.BFS bfs = new BingChilling.Algorithms.BFS(maze);
                List<Node> bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol);
                Console.WriteLine();

                if (bfsPath.Count > 0)
                {
                    Console.WriteLine("BFS Path: ");
                    for (int i = 0; i < bfsPath.Count; i++)
                    {
                        Console.WriteLine("({0}, {1})", bfsPath[i].X, bfsPath[i].Y);
                    }
                    Console.WriteLine();
                }


            }
            else if (dfsCheckBox.IsChecked)
            {
                //await DisplayAlert("DFS", "", "OK");
                // Run DFS algorithm
                BingChilling.Algorithms.DFS dfs = new BingChilling.Algorithms.DFS(maze);
                List<Node> dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol);
                Console.WriteLine();

                if (dfsPath.Count > 0)
                {
                    Console.WriteLine("DFS Path: ");
                    for (int i = 0; i < dfsPath.Count; i++)
                    {
                        Console.WriteLine("({0}, {1})", dfsPath[i].X, dfsPath[i].Y);
                    }
                }

            }
            else
            {
                // No checkbox selected, show error message
                await DisplayAlert("Error", "Please select a checkbox to run the algorithm.", "OK");
                return;
            }

        }

    }
}