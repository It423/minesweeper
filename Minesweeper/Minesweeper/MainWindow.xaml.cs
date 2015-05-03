// MainWindow.xaml.cs
// <copyright file="MainWindow.cs"> This code is protected under the MIT License. </copyright>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The values used for each difficulty,
        /// </summary>
        public static readonly int[][] DifficultSettings = new int[3][] { 
            new int[3] { 9, 9, 10 },
            new int[3] { 16, 16, 40 },
            new int[3] { 16, 30, 99 }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.StartButton_Click(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Gets or sets the tiles for minesweeper.
        /// </summary>
        public Button[][] Tiles { get; set; }

        /// <summary>
        /// Initializes a minesweeper grid.
        /// </summary>
        /// <param name="rows"> The amount of rows in the grid. </param>
        /// <param name="columns"> The amount of columns in the grid. </param>
        private void InitializeGrid(int rows, int columns)
        {
            // Set window size
            this.Width = (16 * columns) + 20;
            this.Height = (16 * rows) + 130;

            // Set the rows and columns on the display grid
            this.SetGridDefinitions(rows, columns);

            // Generate the grid
            this.Tiles = new Button[columns][];
            for (int x = 0; x < columns; x++)
            {
                this.Tiles[x] = new Button[rows];
                for (int y = 0; y < rows; y++)
                {
                    this.CreateTileButton(x, y);
                }
            }

            this.MineGrid.UpdateLayout();
        }

        /// <summary>
        /// Creates a tile button. 
        /// </summary>
        /// <param name="x"> The x position of the tile. </param>
        /// <param name="y"> The y position of the tile. </param>
        private void CreateTileButton(int x, int y)
        {
            this.Tiles[x][y] = new Button();

            // Add event handlers
            this.Tiles[x][y].MouseLeftButtonUp += this.Tile_MouseLeftButtonUp;
            this.Tiles[x][y].MouseRightButtonDown += this.Tile_MouseRightButtonUp;

            // Set the image
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("Images/Covered.png", UriKind.Relative));
            this.Tiles[x][y].Background = brush;

            // Set the size
            this.Tiles[x][y].Width = 16;
            this.Tiles[x][y].Height = 16;

            // Set the grid position and add to grid
            Grid.SetColumn(this.Tiles[x][y], x);
            Grid.SetRow(this.Tiles[x][y], y);
            this.MineGrid.Children.Add(this.Tiles[x][y]);
        }

        /// <summary>
        /// Sets the mine grid to the right size.
        /// </summary>
        /// <param name="rows"> The amount of rows. </param>
        /// <param name="columns"> The amount of columns. </param>
        private void SetGridDefinitions(int rows, int columns)
        {
            this.MineGrid.RowDefinitions.Clear();
            this.MineGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                this.MineGrid.RowDefinitions.Add(new RowDefinition());
                this.MineGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
            }

            for (int i = 0; i < columns; i++)
            {
                this.MineGrid.ColumnDefinitions.Add(new ColumnDefinition());
                this.MineGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        /// <summary>
        /// Handles the click of the start button.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Get difficulty settings
            int xSize = MainWindow.DifficultSettings[(int)this.DifficultySlider.Value][0];
            int ySize = MainWindow.DifficultSettings[(int)this.DifficultySlider.Value][1];
            int mines = MainWindow.DifficultSettings[(int)this.DifficultySlider.Value][2];

            this.InitializeGrid(xSize, ySize);
        }

        /// <summary>
        /// Handles a tile being left clicked.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Tile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles a tile beign right clicked.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Tile_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
