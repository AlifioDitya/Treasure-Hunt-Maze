using BingChilling.Environment;
using Microsoft.Maui.Controls;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading;
//using Microsoft.Maui.Controls.Compatibility;
//using static Android.InputMethodServices.Keyboard;


namespace BingChillingGUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //startlabel.Clicked += mazeSlider;
            
        }

        private string filePath;
        private int[,] maze;
        private int sliderValue;
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
                        fileName.Text = $"\"{result.FileName}\"";
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
                        this.maze = matrix;
                        await DisplayMaze(matrix, row, col);
                    }
                    else {
                        throw new ArgumentException("Invalid file extension. Only .txt files are allowed");
                    }
                }

            }
            catch (ArgumentException ex) //
            {
                // The user canceled or something went wrong
                await DisplayAlert("Error", ex.Message, "OK");
                return;
            }
        }

        private async Task DisplayMaze(int[,] matrix, int numberOfRows, int numberOfColumns)
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
                        Text = matrix[i, j] == 2 || matrix[i, j] == 5 || matrix[i, j] == 8 ? "Treasure" : matrix[i, j] == 0 || matrix[i, j] == 3 || matrix[i, j] == 6 ? "Start" : "",
                        TextColor = matrix[i, j] == 0 || matrix[i, j] == 2 || matrix[i, j] == 3 || matrix[i, j] == 5 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        BackgroundColor = matrix[i, j] == -1 ? Color.FromRgb(0, 0, 0)
                                         : matrix[i, j] == 3 || matrix[i, j] == 4 || matrix[i, j] == 5 ? Color.FromRgb(0, 255, 0)
                                         : matrix[i, j] == 6 || matrix[i, j] == 7 || matrix[i, j] == 8 ? Color.FromRgb(255, 0, 0)
                                         : Color.FromRgb(255, 255, 255),
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
            await Task.Delay(this.sliderValue);
        }



        private async void visu_Clicked(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            int steps = 0;
            Maze maze = new Maze(0, 0);
            try {
                maze.Load(this.filePath);
            }
            catch (FileNotFoundException) {
                await DisplayAlert("Error", "Please select a file.", "OK");
                return;
            }
            
            int[,] matrix = new int[maze.Rows, maze.Cols];
            if (bfsCheckBox.IsChecked && dfsCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please select only the BFS or DFS checkbox to run the algorithm.", "OK");
                stopwatch.Start();
                stopwatch.Stop();
            }
            else if (bfsCheckBox.IsChecked)
            {
                //await DisplayAlert("BFS", "", "OK");
                //Run BFS algorithm
                BingChilling.Algorithms.BFS bfs = new BingChilling.Algorithms.BFS(maze);
                stopwatch.Start();
                List<Node> bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol);
                stopwatch.Stop();
                int index = 0;
                int count = 0;
                while(index < bfsPath.Count) {
                    if(count > 0) {
                        if (this.maze[bfsPath[index-1].X, bfsPath[index-1].Y] == 6)
                        {
                            this.maze[bfsPath[index-1].X, bfsPath[index-1].Y] = 3;
                        }
                        else if (this.maze[bfsPath[index-1].X, bfsPath[index-1].Y] == 7)
                        {
                            this.maze[bfsPath[index-1].X, bfsPath[index-1].Y] = 4;
                        }
                        else if (this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] == 8)
                        {
                            this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] = 5;
                        }
                        
                        await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                    }
                    //searching this index
                    if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 0)
                    {
                        this.maze[bfsPath[index].X, bfsPath[index].Y] = 6;
                    }
                    else if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 1)
                    {
                        this.maze[bfsPath[index].X, bfsPath[index].Y] = 7;
                    }
                    else if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 2)
                    {
                        this.maze[bfsPath[index].X, bfsPath[index].Y] = 8;
                    }
                    index++;
                    await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                    if (count == 0) {
                        count++;
                    }
                        
                    
                      
                }
                steps = bfsPath.Count;
                //DisplayMaze(this.maze, maze.Rows, maze.Cols);
                


                if (bfsPath.Count() > 0)
                {
                    routeInfo.Text = $"Route : {bfsPath.Last().GetDirections("")}";
                    stepsInfo.Text = $"Steps : {bfsPath.Count() - 1}";
                }
                else
                {
                    routeInfo.Text = "Route : -";
                    stepsInfo.Text = "Steps : 0";
                }
            }
            else if (dfsCheckBox.IsChecked)
            {
                //await DisplayAlert("DFS", "", "OK");
                // Run DFS algorithm
                BingChilling.Algorithms.DFS dfs = new BingChilling.Algorithms.DFS(maze);
                stopwatch.Start();
                List<Node> dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol);
                stopwatch.Stop();
                Console.WriteLine();

                int index = 0;
                while (index < dfsPath.Count)
                {
                    if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 0)
                    {
                        this.maze[dfsPath[index].X, dfsPath[index].Y] = 3;
                    }
                    else if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 1)
                    {
                        this.maze[dfsPath[index].X, dfsPath[index].Y] = 4;
                    }
                    else if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 2)
                    {
                        this.maze[dfsPath[index].X, dfsPath[index].Y] = 5;
                    }
                    index++;
                }

                await DisplayMaze(this.maze, maze.Rows, maze.Cols);

                steps = dfsPath.Count;

                if (dfsPath.Count() > 0)
                {
                    routeInfo.Text = $"Route : {dfsPath.Last().GetDirections("")}";
                    stepsInfo.Text = $"Steps : {dfsPath.Count() - 1}";
                }
                else
                {
                    routeInfo.Text = "Route : -";
                    stepsInfo.Text = "Steps : 0";
                }
            }
            else
            {
                stopwatch.Start();
                stopwatch.Stop();
                // No checkbox selected, show error message
                await DisplayAlert("Error", "Please select a BFS or DFS checkbox to run the algorithm.", "OK");
                return;
            }

            
            executionTime.Text = $"Execution Time : {stopwatch.ElapsedMilliseconds} ms";
            nodesCounter.Text = $"Nodes : {steps}";
            SemanticScreenReader.Announce(nodesCounter.Text);
            SemanticScreenReader.Announce(executionTime.Text);
            SemanticScreenReader.Announce(routeInfo.Text);
            SemanticScreenReader.Announce(stepsInfo.Text);


        }

        void mazeSlider(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            this.sliderValue = value;
            
        }
        private void ResetButton(object sender, EventArgs e)
        {
            mazeView.Content = null;
            executionTime.Text = $"Execution Time : ";
            nodesCounter.Text = $"Nodes : ";
            routeInfo.Text = "Route : ";
            stepsInfo.Text = "Steps : ";
        }
    }
}