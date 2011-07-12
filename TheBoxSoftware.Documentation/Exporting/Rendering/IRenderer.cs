using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	interface IRenderer<T> {
		Exporter Exporter { get; set; }
		void Render(T writer);
	}
}
