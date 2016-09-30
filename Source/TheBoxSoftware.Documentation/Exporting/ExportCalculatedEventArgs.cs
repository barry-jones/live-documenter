using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting
{
    /// <summary>
    /// Event arguments for the <see cref="ExportCalculatedEventHandler"/>.
    /// </summary>
    public sealed class ExportCalculatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCalculatedEventArgs"/> class.
        /// </summary>
        public ExportCalculatedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCalculatedEventArgs"/> class.
        /// </summary>
        /// <param name="numberOfSteps">The number of steps.</param>
        public ExportCalculatedEventArgs(int numberOfSteps)
        {
            this.NumberOfSteps = numberOfSteps;
        }

        /// <summary>
        /// The number of steps to perform in this export
        /// </summary>
        /// <value>The number of steps.</value>
        public int NumberOfSteps
        {
            get; set;
        }
    }
}