using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.Reflection.Core.COFF;
	using TheBoxSoftware.Reflection.Core.PE;

	/// <summary>
	/// Represents an entry in the PE Map
	/// </summary>
	internal class Entry {
		#region Constructors
		public Entry(string displayName) {
			this.Children = new List<Entry>();
			this.DisplayName = displayName;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name to be displayed for this entry in the map
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the child entries for this Entry
		/// </summary>
		public List<Entry> Children { get; set; }

		public object Data { get; set; }
		#endregion

		#region Factory Methods
		public static Entry Create(object forItem) {
			if (forItem is string) {
				return new Entry(forItem as string);
			}
			else if (forItem is MetadataStream) {
				return new MetadataStreamEntry(forItem as MetadataStream);
			}
			else if (forItem is CLRDirectory) {
				CLRDirectory directory = forItem as CLRDirectory;
				return new CLRDirectoryEntry(directory);
			}
			else if (forItem is Directory) {
				Directory directory = forItem as Directory;
				return new Entry(directory.Name);
			}
			else if (forItem is StringStream) {
				StringStream stringStream = forItem as StringStream;
				return new StringStreamEntry(stringStream);
			}
			else if (forItem is GuidStream) {
				GuidStream stringStream = forItem as GuidStream;
				return new GuidStreamEntry(stringStream);
			}
			else if (forItem is Stream) {
				Stream stream = forItem as Stream;
				return new Entry(stream.Name);
			}
			else {
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}
