using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	/// <summary>
	/// Exports content in a defined medium <paramref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The stream writer to render to.</typeparam>
	interface IRenderer<T> {
		/// <summary>
		/// The Exporter rendering the content.
		/// </summary>
		Exporter Exporter { get; set; }

		/// <summary>
		/// The method to render the content to <paramref name="T"/>.
		/// </summary>
		/// <param name="writer">The rendering medium.</param>
		void Render(T writer);
	}
}
