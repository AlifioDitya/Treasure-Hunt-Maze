using Microsoft.Maui.Controls;

namespace BingChillingGUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

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
                        using var stream = await result.OpenReadAsync();
                        using var reader = new StreamReader(stream);
                        int row = 0, col = 0;
                        int[,] matrix = new int[6, 6]; // Fixed dimensions for the example maze

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
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                
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



        // Helper method to get the background color for each element
        private Color GetColorForElement(int element)
        {
            Color gray = Color.FromRgb(128, 128, 128);
            switch (element)
            {
                case -1: // Wall
                    return Color.FromRgb(128, 128, 128);
                case 1: // Road
                    return Color.FromRgb(255, 255, 255);
                case 2: // Treasure
                    return Color.FromRgb(255, 255, 0);
                default: // Unknown element
                    return Color.FromRgb(0, 0, 0);
            }
        }

    }
}