using BingChilling.Environment;
using Microsoft.Maui.Controls;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading;
using BingChilling.Algorithms;
//using Microsoft.Maui.Controls.Compatibility;
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
        private int[,] maze;
        private int sliderValue;
        private Grid grid;
        private FileResult result;

        private async void baseMaze( FileResult result )
        {
            try
            {
                BingChilling.Environment.Maze maze = new BingChilling.Environment.Maze(0, 0);
                maze.Load(result.FullPath);


                fileName.Text = $"\"{result.FileName}\"";
                this.filePath = result.FullPath;

                int[,] matrix = new int[maze.Rows, maze.Cols];
                int startcount = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (maze[i, j] == 1)
                        {
                            matrix[i, j] = -1;
                        }
                        else if (maze[i, j] == 0 && i == maze.StartRow && j == maze.StartCol && startcount == 0)
                        {
                            matrix[i, j] = 0;
                            startcount++;
                        }
                        else if (maze[i, j] == 2)
                        {
                            matrix[i, j] = 2;
                        }
                        else if (maze[i, j] == 0)
                        {
                            matrix[i, j] = 1;
                        }
                    }
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
                await DisplayMaze(matrix, maze.Rows, maze.Cols);
            }
            catch (InvalidMazeFormatException ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
                return;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            PickOptions options = new PickOptions();
            try
            {
                result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                    {
                        baseMaze(result);
                    }
                    else
                    {
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
                        Text = matrix[i, j] == 2 || matrix[i, j] == 5 ? "Treasure" : matrix[i, j] == 0 || matrix[i, j] == 3 ? "Start" : "",
                        TextColor = matrix[i, j] == 0 || matrix[i, j] == 2 || matrix[i, j] == 3 || matrix[i, j] == 5 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        BackgroundColor = matrix[i, j] == -1 ? Color.FromRgb(0, 0, 0)
                                         : matrix[i, j] == 3 || matrix[i, j] == 4 || matrix[i, j] == 5 ? Color.FromRgb(0, 255, 0)
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
            this.grid = grid;
            await Task.Delay(this.sliderValue);
        }



        private async void visu_Clicked(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            int steps = 0;
            BingChilling.Environment.Maze maze = new BingChilling.Environment.Maze(0, 0);
            try
            {
                maze.Load(this.filePath);
            }
            catch (FileNotFoundException)
            {
                await DisplayAlert("Error", "Please select a file.", "OK");
                return;
            } catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
                return;
            }

            if (bfsCheckBox.IsChecked && dfsCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please select only 1 checkbox to run the algorithm.", "OK");
                stopwatch.Start();
                stopwatch.Stop();
            }
            else if (bfsCheckBox.IsChecked)
            {
                //await DisplayAlert("BFS", "", "OK");
                //Run BFS algorithm
                BingChilling.Algorithms.BFS bfs = new BingChilling.Algorithms.BFS(maze);
                stopwatch.Start();

                List<Node> bfsPath; //= bfs.SearchTreasures(maze.StartRow, maze.StartCol);
                if (tspCheckBox.IsChecked)
                {
                    bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol, true);
                    //cleanDFSPath = dfsPath.Last().ListPath();
                    stopwatch.Stop();
                }
                else
                {
                    bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol, false);
                    //cleanDFSPath = dfsPath.Last().ListPath();
                }
                stopwatch.Stop();
                visualization(bfsPath, bfsPath);
                steps = bfsPath.Count;
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
                stopwatch.Start();
                // Run DFS algorithm
                List<Node> dfsPath;
                List<Node> cleanDFSPath;
                BingChilling.Algorithms.DFS dfs = new BingChilling.Algorithms.DFS(maze);
                if (tspCheckBox.IsChecked) {
                    dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol, true);
                    cleanDFSPath = dfsPath.Last().ListPath();
                    stopwatch.Stop();
                }
                else {
                    dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol, false);
                    cleanDFSPath = dfsPath.Last().ListPath();
                }
                visualization(dfsPath, cleanDFSPath);        
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
                await DisplayAlert("Error", "Please select a checkbox to run the algorithm.", "OK");
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
            this.sliderValue = 1000 - value;

        }
        private void ResetButton(object sender, EventArgs e)
        {
            mazeView.Content = null;
            executionTime.Text = $"Execution Time : ";
            nodesCounter.Text = $"Nodes : ";
            routeInfo.Text = "Route : ";
            stepsInfo.Text = "Steps : ";
            dfsCheckBox.IsChecked = false;
            bfsCheckBox.IsChecked = false;
            tspCheckBox.IsChecked = false;
            if(this.filePath != null)
                baseMaze(result);
        }

        private async Task ChangeTileColor(int number, int row, int column)
        {
            if (this.grid != null && row >= 0 && row < this.grid.RowDefinitions.Count && column >= 0 && column < this.grid.ColumnDefinitions.Count)
            {
                var label = this.grid.Children.Cast<Label>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                if (number != 0)
                {
                    if (label != null)
                    {
                        number--;
                        label.BackgroundColor = Color.FromRgb(255, 255 - number * 20, 0);
                    }
                }
                else
                {
                    if (label != null)
                    {
                        label.BackgroundColor = Color.FromRgb(0, 0, 255);
                    }
                }

            }

            mazeView.Content = grid;

            await Task.Delay(this.sliderValue);
        }

        private async void visualization(List<Node> list, List<Node> cleanedList) {
            int[,] matrix = new int[this.maze.GetLength(0), this.maze.GetLength(1)];
            int prevVal = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = -1;
                }
            }

            int index = 0;
            int count = 0;
            while (index <= list.Count)
            {
                if (count > 0)
                {

                    if (prevVal > 0)
                    {
                        prevVal++;
                        matrix[list[index - 1].X, list[index - 1].Y] = prevVal;

                    }
                    else
                    {
                        matrix[list[index - 1].X, list[index - 1].Y]++;

                    }
                    await ChangeTileColor(matrix[list[index - 1].X, list[index - 1].Y], list[index - 1].X, list[index - 1].Y);
                    if (index == list.Count)
                    {

                        index++;
                    } 
                }
                //searching this index
                if (index < list.Count)
                {

                    if (matrix[list[index].X, list[index].Y] == -1)
                    {
                        prevVal = 0;
                        matrix[list[index].X, list[index].Y]++;
                    }
                    else
                    {
                        prevVal = matrix[list[index].X, list[index].Y];
                        matrix[list[index].X, list[index].Y] = 0;

                    }
                    await ChangeTileColor(0, list[index].X, list[index].Y);
                    index++;;
                    if (count == 0)
                    {
                        count++;
                    }
                }
            }
            index = 0;
            while (index < cleanedList.Count)
            {
                if (this.maze[cleanedList[index].X, cleanedList[index].Y] == 0)
                {
                    this.maze[cleanedList[index].X, cleanedList[index].Y] = 3;
                }
                else if (this.maze[cleanedList[index].X, cleanedList[index].Y] == 1)
                {
                    this.maze[cleanedList[index].X, cleanedList[index].Y] = 4;
                }
                else if (this.maze[cleanedList[index].X, cleanedList[index].Y] == 2)
                {
                    this.maze[cleanedList[index].X, cleanedList[index].Y] = 5;
                }
                index++;
            }

          
            await DisplayMaze(this.maze, this.maze.GetLength(0), this.maze.GetLength(1));

        }
    }
}