using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting
{
    /// <summary>
    /// Describes an issue at various points of the export process.
    /// </summary>
    public sealed class Issue
    {
        private string _description;

        /// <summary>
        /// Gets or sets a description of the issue.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
    }
}