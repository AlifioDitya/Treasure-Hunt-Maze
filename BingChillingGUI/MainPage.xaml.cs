﻿using BingChilling.Environment;
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
        private int shade;
        private int shade2;
        private Grid grid;
        private FileResult result;

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
                        BingChilling.Environment.Maze maze = new BingChilling.Environment.Maze(0, 0);
                        maze.Load(result.FullPath);
                        fileName.Text = $"\"{result.FileName}\"";
                        this.filePath = result.FullPath;

                        int[,] matrix = new int[maze.Rows, maze.Cols];
                        int startcount = 0;
                        for(int i = 0; i < matrix.GetLength(0); i++) {
                            for(int j = 0; j < matrix.GetLength(1); j++) {
                                if (maze[i, j] == 1)
                                {
                                    matrix[i, j] = -1;
                                }
                                else if (maze[i, j] == 0 && i == maze.StartRow && i == maze.StartRow && startcount == 0)
                                {
                                    matrix[i, j] = 0;
                                    startcount++;
                                }
                                else if (maze[i, j] == 2)
                                {
                                    matrix[i, j] = 2;
                                }
                                else if (maze[i,j] == 0)
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
                        Text = matrix[i, j] == 2 || matrix[i, j] == 5 || matrix[i, j] == 8 ? "Treasure" : matrix[i, j] == 0 || matrix[i, j] == 3 || matrix[i, j] == 6 ? "Start" : "",
                        TextColor = matrix[i, j] == 0 || matrix[i, j] == 2 || matrix[i, j] == 3 || matrix[i, j] == 5 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        BackgroundColor = matrix[i, j] == -1 ? Color.FromRgb(0, 0, 0)
                                         : matrix[i, j] == 3 || matrix[i, j] == 4 || matrix[i, j] == 5 ? Color.FromRgb(this.shade, this.shade2, 0)
                                         : matrix[i, j] == 6 || matrix[i, j] == 7 || matrix[i, j] == 8 ? Color.FromRgb(0, 0, 255)
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

            int[,] matrix = new int[maze.Rows, maze.Cols];
            int prevVal = 0;
            //int[,] matrix2 = new int[maze.Rows, maze.Cols];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = -1;
                }
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
                List<Node> bfsPath = bfs.SearchTreasures(maze.StartRow, maze.StartCol);
                stopwatch.Stop();

                int index = 0;
                int count = 0;
                this.shade2 = 255;
                this.shade = 255;
                while (index <= bfsPath.Count)
                {
                    if (count > 0)
                    {
                        if (this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] == 6)
                        {
                            this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] = 3;
                            //this.shade += 50;
                        }
                        else if (this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] == 7)
                        {
                            this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] = 4;
                            //this.shade += 50;
                        }
                        else if (this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] == 8)
                        {
                            this.maze[bfsPath[index - 1].X, bfsPath[index - 1].Y] = 5;
                            //this.shade += 50;
                        }

                        if (prevVal > 0)
                        {
                            prevVal++;
                            matrix[bfsPath[index - 1].X, bfsPath[index - 1].Y] = prevVal;

                        }
                        else
                        {
                            //prevVal = matrix[bfsPath[index].X, bfsPath[index].Y];
                            matrix[bfsPath[index - 1].X, bfsPath[index - 1].Y]++;

                        }
                        await ChangeTileColor(matrix[bfsPath[index - 1].X, bfsPath[index - 1].Y], bfsPath[index - 1].X, bfsPath[index - 1].Y);
                        if (index == bfsPath.Count)
                        {
                            index++;
                        }

                        //await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                    }
                    //searching this index
                    if (index < bfsPath.Count)
                    {
                        if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 0 || this.maze[bfsPath[index].X, bfsPath[index].Y] == 3)
                        {

                            this.maze[bfsPath[index].X, bfsPath[index].Y] = 6;

                        }
                        else if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 1 || this.maze[bfsPath[index].X, bfsPath[index].Y] == 4)
                        {

                            this.maze[bfsPath[index].X, bfsPath[index].Y] = 7;
                        }
                        else if (this.maze[bfsPath[index].X, bfsPath[index].Y] == 2 || this.maze[bfsPath[index].X, bfsPath[index].Y] == 5)
                        {

                            this.maze[bfsPath[index].X, bfsPath[index].Y] = 8;
                        }
                        if (matrix[bfsPath[index].X, bfsPath[index].Y] == -1)
                        {
                            prevVal = 0;
                            matrix[bfsPath[index].X, bfsPath[index].Y]++;
                        }
                        else
                        {
                            prevVal = matrix[bfsPath[index].X, bfsPath[index].Y];
                            matrix[bfsPath[index].X, bfsPath[index].Y] = 0;

                        }
                        await ChangeTileColor(0, bfsPath[index].X, bfsPath[index].Y);
                        index++;
                        //await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                        if (count == 0)
                        {
                            count++;
                        }
                    }
                }
                this.shade = 0;
                Console.WriteLine(this.shade);

                await DisplayMaze(this.maze, maze.Rows, maze.Cols);

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                    Console.WriteLine();

                }


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
                BingChilling.Algorithms.DFS dfs = new BingChilling.Algorithms.DFS(maze);
                List<Node> dfsPath = dfs.SearchTreasures(maze.StartRow, maze.StartCol, true);
                List<Node> cleanDFSPath = dfsPath.Last().ListPath();
                stopwatch.Stop();
                Console.WriteLine();

                int index = 0;
                int count = 0;
                this.shade2 = 255;
                Console.WriteLine(dfsPath.Count);
                while (index <= dfsPath.Count)
                {
                    if (count > 0)
                    {
                        /*if (this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] == 6)
                        {
                            this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] = 3;
                            //this.shade += 50;
                        }
                        else if (this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] == 7)
                        {
                            this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] = 4;
                            //this.shade += 50;
                        }
                        else if (this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] == 8)
                        {
                            this.maze[dfsPath[index - 1].X, dfsPath[index - 1].Y] = 5;
                            //this.shade += 50;
                        }*/

                        if (prevVal > 0)
                        {
                            prevVal++;
                            matrix[dfsPath[index - 1].X, dfsPath[index - 1].Y] = prevVal;

                        }
                        else
                        {
                            //prevVal = matrix[bfsPath[index].X, bfsPath[index].Y];
                            matrix[dfsPath[index - 1].X, dfsPath[index - 1].Y]++;

                        }
                        await ChangeTileColor(matrix[dfsPath[index - 1].X, dfsPath[index - 1].Y], dfsPath[index - 1].X, dfsPath[index - 1].Y);
                        Console.WriteLine("Index: ");
                        Console.WriteLine(index);
                        if (index == dfsPath.Count)
                        {
                           
                            index++;
                        }

                        //await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                    }
                    //searching this index
                    if (index < dfsPath.Count)
                    {
                        /*if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 0 || this.maze[dfsPath[index].X, dfsPath[index].Y] == 3)
                        {

                            this.maze[dfsPath[index].X, dfsPath[index].Y] = 6;

                        }
                        else if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 1 || this.maze[dfsPath[index].X, dfsPath[index].Y] == 4)
                        {

                            this.maze[dfsPath[index].X, dfsPath[index].Y] = 7;
                        }
                        else if (this.maze[dfsPath[index].X, dfsPath[index].Y] == 2 || this.maze[dfsPath[index].X, dfsPath[index].Y] == 5)
                        {

                            this.maze[dfsPath[index].X, dfsPath[index].Y] = 8;
                        }*/
                        if (matrix[dfsPath[index].X, dfsPath[index].Y] == -1)
                        {
                            prevVal = 0;
                            matrix[dfsPath[index].X, dfsPath[index].Y]++;
                        }
                        else
                        {
                            prevVal = matrix[dfsPath[index].X, dfsPath[index].Y];
                            matrix[dfsPath[index].X, dfsPath[index].Y] = 0;

                        }
                        await ChangeTileColor(0, dfsPath[index].X, dfsPath[index].Y);
                        index++;
                        //await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                        if (count == 0)
                        {
                            count++;
                        }
                    }
                }
                this.shade = 0;
                Console.WriteLine(this.shade);
                //await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                //int index = 0;
                index = 0;
                Console.WriteLine("Clean Path Count: ");
                Console.WriteLine(cleanDFSPath.Count);
                while (index < cleanDFSPath.Count)
                {
                    if (this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] == 0)
                    {
                        this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] = 3;
                    }
                    else if (this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] == 1)
                    {
                        this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] = 4;
                    }
                    else if (this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] == 2)
                    {
                        this.maze[cleanDFSPath[index].X, cleanDFSPath[index].Y] = 5;
                    }
                    index++;
                    //Console.WriteLine("CleanPath: ");
                    //Console.WriteLine(cleanDFSPath[index].X + cleanDFSPath[index].Y);
                }
                //Console.WriteLine(cleanDFSPath)
                await DisplayMaze(this.maze, maze.Rows, maze.Cols);
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        Console.Write(this.maze[i, j] + " ");
                    }
                    Console.WriteLine();

                }

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
            this.sliderValue = value;

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
        }

        private async Task ChangeTileColor(int number, int row, int column)
        {
            Console.WriteLine(number);
            if (this.grid != null && row >= 0 && row < this.grid.RowDefinitions.Count && column >= 0 && column < this.grid.ColumnDefinitions.Count)
            {
                var label = this.grid.Children.Cast<Label>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                if (number != 0)
                {
                    if (label != null)
                    {
                        number--;
                        label.BackgroundColor = Color.FromRgb(255, 255 - number * 50, 0);
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
    }
}