using System;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Event arguments for the ExportStepsEventHandler
	/// </summary>
	public class ExportStepEventArgs : EventArgs {
		/// <summary>
		/// Initializes a new instance of the <see cref="ExportStepEventArgs"/> class.
		/// </summary>
		public ExportStepEventArgs() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportStepEventArgs"/> class.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <param name="step">The step.</param>
		public ExportStepEventArgs(string description, int step) {
			this.Description = description;
			this.Step = step;
		}

		/// <summary>
		/// A description of the step that is being performed.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// The step number.
		/// </summary>
		/// <value>The step.</value>
		public int Step { get; set; }
	}
}
