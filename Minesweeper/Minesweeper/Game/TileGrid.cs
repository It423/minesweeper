// TileGrid.cs
// <copyright file="TileGrid.cs"> This code is protected under the MIT License. </copyright>
using System;
using Minesweeper.Game.Events;

namespace Minesweeper.Game
{
    /// <summary>
    /// A class that represents the grid of tiles.
    /// </summary>
    public class TileGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileGrid" /> class.
        /// </summary>
        /// <param name="rows"> How many rows to generate. </param>
        /// <param name="columns"> How many columns to generate. </param>
        /// <param name="mines"> How many mines to generate. </param>
        /// <param name="startingX"> The first clicked x value. </param>
        /// <param name="startingY"> The first clicked y value. </param>
        public TileGrid(int rows, int columns, int mines, int startingX, int startingY)
        {
            // Generate tiles
            this.Tiles = new Tile[columns][];
            for (int x = 0; x < columns; x++)
            {
                this.Tiles[x] = new Tile[rows];
                for (int y = 0; y < rows; y++)
                {
                    this.Tiles[x][y] = new Tile();
                }
            }

            // Generate mines
            Random r = new Random();
            for (int i = 0; i < mines; i++)
            {
                int x = r.Next(columns);
                int y = r.Next(rows);
                if (x != startingX && y != startingY)
                {
                    if (this.Tiles[x][y].Mine)
                    {
                        i--;
                    }
                    else
                    {
                        this.Tiles[x][y].Mine = true;
                    }
                }
                else
                {
                    i--;
                }
            }

            // Give each mine its number value
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int total = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (x + i >= 0 && x + i < columns && y + j >= 0 && y + j < rows)
                            {
                                if (this.Tiles[x + i][y + j].Mine)
                                {
                                    total++;
                                }
                            }
                        }
                    }

                    this.Tiles[x][y].Nearby = total;
                }
            }
        }

        /// <summary>
        /// Fires when a mine is uncovered.
        /// </summary>
        public event EventHandler<MineUncoveredEventArgs> MineUncovered;

        /// <summary>
        /// Fires when a tile is uncovered.
        /// </summary>
        public event EventHandler<TileUncoveredEventArgs> TileUncovered;

        /// <summary>
        /// Gets or sets the grid of tiles.
        /// </summary>
        public Tile[][] Tiles { get; set; }

        /// <summary>
        /// Uncovers squares in the grid.
        /// </summary>
        /// <param name="x"> The x coordinate to uncover. </param>
        /// <param name="y"> The y coordinate to uncover. </param>
        public void UncoverSquares(int x, int y)
        {
            // Only run if in range and not uncovered
            if (x >= 0 && x < this.Tiles.Length && y >= 0 && y < this.Tiles[0].Length && !this.Tiles[x][y].Uncovered)
            {
                this.Tiles[x][y].Uncovered = true;

                if (this.Tiles[x][y].Mine)
                {
                    this.OnMineUncover(this, new MineUncoveredEventArgs(x, y));
                }
                else
                {
                    this.OnTileUncover(this, new TileUncoveredEventArgs(x, y));

                    // Find next tiles if can
                    if (this.Tiles[x][y].Nearby == 0)
                    {
                        this.UncoverSquares(x - 1, y);
                        this.UncoverSquares(x + 1, y);
                        this.UncoverSquares(x, y - 1);
                        this.UncoverSquares(x, y + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Fires the mine uncovered event.
        /// </summary>
        /// <param name="origin"> The origin on the event. </param>
        /// <param name="e"> The event arguments. </param>
        protected void OnMineUncover(object origin, MineUncoveredEventArgs e)
        {
            EventHandler<MineUncoveredEventArgs> handler = this.MineUncovered;

            if (handler != null)
            {
                handler(origin, e);
            }
        }

        /// <summary>
        /// Fires the tile uncovered event.
        /// </summary>
        /// <param name="origin"> The origin on the event. </param>
        /// <param name="e"> The event arguments. </param>
        protected void OnTileUncover(object origin, TileUncoveredEventArgs e)
        {
            EventHandler<TileUncoveredEventArgs> handler = this.TileUncovered;

            if (handler != null)
            {
                handler(origin, e);
            }
        }
    }
}
