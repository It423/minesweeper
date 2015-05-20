// MainWindow.xaml.cs
// <copyright file="MainWindow.xaml.cs"> This code is protected under the MIT License. </copyright>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Minesweeper.Game;
using Minesweeper.Game.Events;

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
        /// Gets or sets the TilesButtons for minesweeper.
        /// </summary>
        public Button[][] TilesButtons { get; set; }

        /// <summary>
        /// Gets or sets the grid of tiles.
        /// </summary>
        public TileGrid GameGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is over.
        /// </summary>
        public bool GameOver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a tile has been clicked.
        /// </summary>
        public bool TileClicked { get; set; }

        /// <summary>
        /// Gets or sets how many mines there are.
        /// </summary>
        public int Mines { get; set; }

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
            this.TilesButtons = new Button[columns][];
            for (int x = 0; x < columns; x++)
            {
                this.TilesButtons[x] = new Button[rows];
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
            this.TilesButtons[x][y] = new Button();

            // Add event handlers
            this.TilesButtons[x][y].PreviewMouseLeftButtonUp += this.Tile_MouseLeftButtonUp;
            this.TilesButtons[x][y].PreviewMouseRightButtonUp += this.Tile_MouseRightButtonUp;

            // Set the image
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("Images/Covered.png", UriKind.Relative));
            this.TilesButtons[x][y].Background = brush;

            // Set the size
            this.TilesButtons[x][y].Width = 16;
            this.TilesButtons[x][y].Height = 16;

            // Set the grid position and add to grid
            Grid.SetColumn(this.TilesButtons[x][y], x);
            Grid.SetRow(this.TilesButtons[x][y], y);
            this.MineGrid.Children.Add(this.TilesButtons[x][y]);
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
            this.Mines = MainWindow.DifficultSettings[(int)this.DifficultySlider.Value][2];

            this.InitializeGrid(xSize, ySize);

            this.GameOver = false;
            this.TileClicked = false;
        }

        /// <summary>
        /// Handles a tile being left clicked.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Tile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Get the tile
            int x;
            int y;
            this.GetXYFromTile(sender, out x, out y);

            // Generate grid if need be
            if (!this.TileClicked)
            {
                this.GameGrid = new TileGrid(this.TilesButtons[0].Length, this.TilesButtons.Length, this.Mines, x, y);
                this.MinesLeftText.Text = string.Format("Mines Left: {0}", this.Mines.ToString());
                this.GameGrid.TileUncovered += this.Tile_Uncovered;
                this.GameGrid.MineUncovered += this.Mine_Uncovered;
                this.TileClicked = true;
            }

            if (!this.GameGrid.Tiles[x][y].Uncovered && !this.GameOver)
            {
                // Uncover the squares
                this.GameGrid.UncoverSquares(x, y);
            }
        }

        /// <summary>
        /// Handles a tile begin right clicked.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Tile_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Get the tile
            int x;
            int y;
            this.GetXYFromTile(sender, out x, out y);

            if (!this.GameOver && this.TileClicked && (!this.GameGrid.Tiles[x][y].Uncovered || this.GameGrid.Tiles[x][y].Flagged))
            {
                // Change the mines
                if (this.GameGrid.Tiles[x][y].Flagged)
                {
                    this.Mines++;
                }
                else
                {
                    this.Mines--;
                    
                    // Stop the player using all the flags
                    if (this.Mines < 0)
                    {
                        this.Mines++;
                        return;
                    }
                }

                this.MinesLeftText.Text = string.Format("Mines Left: {0}", this.Mines.ToString());

                // Get the correct image
                string image = this.GameGrid.Tiles[x][y].Flagged ? "Images/Covered.png" : "Images/Flag.png";

                // Set the image correctly
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(image, UriKind.Relative));
                this.TilesButtons[x][y].Background = brush;

                this.GameGrid.Tiles[x][y].Flagged = !this.GameGrid.Tiles[x][y].Flagged;
            }
        }

        /// <summary>
        /// Gets the x and y positions from a tile.
        /// </summary>
        /// <param name="tile"> The tile. </param>
        /// <param name="x"> The x position of the tile. </param>
        /// <param name="y"> The y position of the tile. </param>
        private void GetXYFromTile(object tile, out int x, out int y)
        {
            for (int xCheck = 0; xCheck < this.TilesButtons.Length; xCheck++)
            {
                for (int yCheck = 0; yCheck < this.TilesButtons[xCheck].Length; yCheck++)
                {
                    if (this.TilesButtons[xCheck][yCheck].Equals(tile))
                    {
                        x = xCheck;
                        y = yCheck;
                        return;
                    }
                }
            }

            x = 0;
            y = 0;
        }

        /// <summary>
        /// Handles a tile being uncovered.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Tile_Uncovered(object sender, TileUncoveredEventArgs e)
        {
            // Correct flag count if tile is flagged and clicked
            if (this.GameGrid.Tiles[e.X][e.Y].Flagged)
            {
                this.Tile_MouseRightButtonUp(this.TilesButtons[e.X][e.Y], null);
            }

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(string.Format("Images/Uncovered{0}.png", this.GameGrid.Tiles[e.X][e.Y].Nearby), UriKind.Relative));
            this.TilesButtons[e.X][e.Y].Background = brush;

            if (this.EveryTileFilledIn())
            {
                MessageBoxResult msg = MessageBox.Show(this, "You uncovered every square! You win!", "Victory!");
                this.GameOver = true;
            }
        }

        /// <summary>
        /// Handles a tile being uncovered.
        /// </summary>
        /// <param name="sender"> The origin of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void Mine_Uncovered(object sender, MineUncoveredEventArgs e)
        {
            // Uncover all mines
            for (int x = 0; x < this.GameGrid.Tiles.Length; x++)
            {
                for (int y = 0; y < this.GameGrid.Tiles[x].Length; y++)
                {
                    if (this.GameGrid.Tiles[x][y].Mine)
                    {
                        ImageBrush brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("Images/Mine.png", UriKind.Relative));
                        this.TilesButtons[x][y].Background = brush;
                    }
                }
            }

            // Tell the user they lost
            if (!this.GameOver)
            {
                MessageBoxResult msg = MessageBox.Show(this, "You hit a mine! Game over!", "Game Over");
                this.GameOver = true;
            }
        }

        /// <summary>
        /// Checks if every tile is clicked or flagged.
        /// </summary>
        /// <returns> Whether the game is complete. </returns>
        private bool EveryTileFilledIn()
        {
            foreach (Tile[] column in this.GameGrid.Tiles)
            {
                foreach (Tile t in column)
                {
                    if (!t.Uncovered && !t.Flagged)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
