// MineUncoveredEventArgs.cs
// <copyright file="MineUncoveredEventArgs.cs"> This code is protected under the MIT License. </copyright>
namespace Minesweeper.Game.Events
{
    /// <summary>
    /// The event arguments for when a mine is uncovered.
    /// </summary>
    public class MineUncoveredEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MineUncoveredEventArgs" /> class.
        /// </summary>
        /// <param name="x"> The x coordinate of the mine. </param>
        /// <param name="y"> The y coordinate of the mine. </param>
        public MineUncoveredEventArgs(int x, int y)
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
