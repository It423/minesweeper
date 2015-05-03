// TileUncoveredEventArgs.cs
// <copyright file="TileUncoveredEventArgs.cs"> This code is protected under the MIT License. </copyright>
namespace Minesweeper.Game.Events
{
    public class TileUncoveredEventArgs : System.EventArgs
    {
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
