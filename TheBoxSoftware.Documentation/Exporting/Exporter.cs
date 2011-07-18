using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class will not throw exceptions in the Export method. All exceptions will be driven
	/// through the <see cref="ExportException"/> event.
	/// </para>
	/// <para>
	/// Implementers of derived classes should make sure that this export exception mechanism
	/// is continued. As this method is likely to be called on seperate threads.
	/// </para>
	/// </remarks>
	public abstract class Exporter {
		public List<Entry> DocumentMap { get; set; }
		public List<DocumentedAssembly> CurrentFiles { get; set; }
		protected ExportSettings Settings { get; set; }
		private ExportCalculatedEventHandler exportCalculated;
		private ExportStepEventHandler exportStep;
		private ExportExceptionHandler exportException;

		/// <summary>
		/// Initializes a new instance of the <see cref="Exporter"/> class.
		/// </summary>
		/// <param name="currentFiles">The current files.</param>
		protected Exporter(List<DocumentedAssembly> currentFiles, ExportSettings settings) {
			this.CurrentFiles = currentFiles;
			this.Settings = settings;
		}

		public virtual void Export() {
		}

		protected abstract void GenerateDocumentMap();

		protected bool ShouldEntryBeAdded(Entry entryToTest) {
			bool shouldBeAdded = true;

			if (shouldBeAdded &&
				(entryToTest.Item is MethodDef ||
				entryToTest.Item is PropertyDef ||
				entryToTest.Item is FieldDef ||
				entryToTest.Item is TypeDef)) {
				ReflectedMember member = entryToTest.Item as ReflectedMember;
				bool publicVisibility = member.MemberAccess == Visibility.Public;
				if (!publicVisibility)
				{
					shouldBeAdded = false;
					foreach (Visibility current in this.Settings.Visibility)
					{
						if (member.MemberAccess == current)
						{
							shouldBeAdded = true;
							break;
						}
					}
				}
			}

			return shouldBeAdded;
		}

		/// <summary>
		/// Gets a unique id across this exported live document
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="memberUniqueId">The member unique id.</param>
		/// <param name="typeUniqueId">The type unique id.</param>
		internal void GetUniqueId(CRefPath path, out long memberUniqueId, out long typeUniqueId) {
			System.Diagnostics.Debug.WriteLine("GetUniqueId: " + path.ToString());
			System.Diagnostics.Debug.Indent();

			TypeDef type = null;
			ReflectedMember member = null;
			memberUniqueId = 0;
			typeUniqueId = 0;

			if (path.PathType != CRefTypes.Error) {
				foreach (DocumentedAssembly ass in this.CurrentFiles) {
					type = ass.LoadedAssembly.FindType(path.Namespace, path.TypeName);
					if (type != null)
						break;
				}

				if (type != null) {
					if (path.PathType == CRefTypes.Type) {
						member = type;
					}
					else if (path.PathType == CRefTypes.Property || path.PathType == CRefTypes.Method || path.PathType == CRefTypes.Field || path.PathType == CRefTypes.Event) {
						member = path.FindIn(type);
					}

					if (member != null) {
						memberUniqueId = this.GetUniqueKey(type.Assembly, member);
						typeUniqueId = this.GetUniqueKey(type.Assembly, type);
					}
				}
			}

			System.Diagnostics.Debug.WriteLine("tId: " + typeUniqueId.ToString() + " - mId: " + memberUniqueId.ToString());
			System.Diagnostics.Debug.Unindent();
		}

		/// <summary>
		/// Finds the entry in the document map with the specified key.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <param name="checkChildren">Wether or not to check the child entries</param>
		/// <returns>The entry that relates to the key or null if not found</returns>
		protected Entry FindByKey(long key, string subKey, bool checkChildren) {
			Entry found = null;
			for (int i = 0; i < this.DocumentMap.Count; i++) {
				found = this.DocumentMap[i].FindByKey(key, subKey, checkChildren);
				if (found != null) {
					break;
				}
			}
			return found;
		}

		/// <summary>
		/// Obtains a key that uniquely identifies the member in the library, for all libraries
		/// loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <param name="member">The member</param>
		/// <returns>A long that is unique in the application</returns>
		internal long GetUniqueKey(AssemblyDef assembly, ReflectedMember member) {
			long id = ((long)assembly.UniqueId) << 32;
			id += member.UniqueId;
			return id;
		}

		/// <summary>
		/// Obtains a key that uniquely identifies the assembly in the library, for all libraries
		/// and members loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly to get the unique identifier for</param>
		/// <returns>A long that is unique in the application</returns>
		internal long GetUniqueKey(AssemblyDef assembly) {
			return ((long)assembly.UniqueId) << 32;
		}

		#region Events
		/// <summary>
		/// Occurs when a step has been performed during the export operation.
		/// </summary>
		public event ExportStepEventHandler ExportStep {
			add { this.exportStep += value; }
			remove { this.exportStep -= value; }
		}

		/// <summary>
		/// Raises the <see cref="E:ExportStep"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportStepEventArgs"/> instance containing the event data.</param>
		protected void OnExportStep(ExportStepEventArgs e) {
			if (this.exportStep != null) {
				this.exportStep(this, e);
			}
		}

		/// <summary>
		/// Occurs when the number of steps in the export operation has been calculated.
		/// </summary>
		public event ExportCalculatedEventHandler ExportCalculated {
			add { this.exportCalculated += value; }
			remove { this.exportCalculated -= value; }
		}

		/// <summary>
		/// Raises the <see cref="E:ExportCalculated"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportCalculatedEventArgs"/> instance containing the event data.</param>
		protected void OnExportCalculated(ExportCalculatedEventArgs e) {
			if (this.exportCalculated != null) {
				this.exportCalculated(this, e);
			}
		}

		/// <summary>
		/// Occurs when an exception occurs in the export process.
		/// </summary>
		public event ExportExceptionHandler ExportException {
			add { this.exportException += value; }
			remove { this.exportException -= value; }
		}

		/// <summary>
		/// Raises the <see cref="E:ExportException"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportExceptionEventArgs"/> instance containing the event data.</param>
		protected void OnExportException(ExportExceptionEventArgs e) {
			if (this.exportException != null) {
				this.exportException(this, e);
			}
		}
		#endregion
	}
}
