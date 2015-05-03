// TileUncoveredEventArgs.cs
// <copyright file="TileUncoveredEventArgs.cs"> This code is protected under the MIT License. </copyright>
namespace Minesweeper.Game.Events
{
    /// <summary>
    /// The event arguments for when a tile is uncovered.
    /// </summary>
    public class TileUncoveredEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileUncoveredEventArgs" /> class.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile. </param>
        /// <param name="y"> The y coordinate of the tile. </param>
        public TileUncoveredEventArgs(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the x coordinate of the uncovered square.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate of the uncovered square.
        /// </summary>
        public int Y { get; set; }
    }
}
