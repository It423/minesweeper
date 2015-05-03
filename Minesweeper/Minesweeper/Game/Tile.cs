// Tile.cs
// <copyright file="Tile.cs"> This code is protected under the MIT License. </copyright>
namespace Minesweeper.Game
{
    /// <summary>
    /// A class that represents a tile in the game of minesweeper.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tile" /> class.
        /// </summary>
        public Tile()
        {
            this.Mine = false;
            this.Flagged = false;
            this.Uncovered = false;
            this.Nearby = 0;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is a mine.
        /// </summary>
        public bool Mine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is flagged.
        /// </summary>
        public bool Flagged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is uncovered.
        /// </summary>
        public bool Uncovered { get; set; }

        /// <summary>
        /// Gets or sets how many tiles are nearby.
        /// </summary>
        public int Nearby { get; set; }
    }
}
