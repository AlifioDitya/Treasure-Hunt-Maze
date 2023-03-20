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
                        int[,] matrix = new int[5, 5]; // Fixed dimensions for the example maze

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
                                col++;
                            }
                            row++;
                        }

                        DisplayMaze(matrix);
                    }
                }

            }
            catch (Exception ex) //
            {
                // The user canceled or something went wrong
            }
        }

        private void DisplayMaze(int[,] matrix)
        {
            // Create a new grid to hold the maze
            var grid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0
            };

            // Add columns and rows to the grid
            for (int i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }

            // Loop through the matrix and create a label for each element
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Create a new label for this element
                    var label = new Label
                    {
                        Text = matrix[i, j] == 2 ? "Treasure" : "",
                        TextColor = matrix[i, j] == 2 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        BackgroundColor = matrix[i, j] == -1 ? Color.FromRgb(0, 0, 0) : Color.FromRgb(255, 255, 255),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    // Add the label to the grid
                    grid.Children.Add(label);
                }
            }

            // Add the grid to the main page
            Content = grid;
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