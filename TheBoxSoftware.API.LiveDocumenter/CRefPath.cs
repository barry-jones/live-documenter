using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumenter {

	/// <summary>
	/// Class that handles and parses a CRef comment path. A CRef path can contain
	/// a fully qualified link to a type, property, method etc in an assembly.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("cref={ToString()}")]
	public sealed class CRefPath {
		private string crefPath;
		private string returnType;
		private bool isOperator = false;

		#region Constructors
		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		public CRefPath() { }
		#endregion

		#region Methods
		/// <summary>
		/// Parses the provided path and returns a populated cref path instance.
		/// </summary>
		/// <param name="crefPath">The cref path to control.</param>
		/// <exception cref="ArgumentNullException">The specified path was null or empty.</exception>
		public static CRefPath Parse(string crefPath) {
			if (string.IsNullOrEmpty(crefPath))
				throw new ArgumentNullException("path");
			CRefPath parsedPath = new CRefPath();
			parsedPath.crefPath = crefPath;
			parsedPath.Parse();
			return parsedPath;
		}

		/// <summary>
		/// Parses the contents of the contained path and stores the details in the
		/// classes properties.
		/// </summary>
		private void Parse() {
			this.ParseType();

			if (this.PathType != CRefTypes.Error) {
				string[] items;
				if (this.PathType != CRefTypes.Method) {
					items = this.crefPath.Substring(this.crefPath.IndexOf(':') + 1).Split('.');
				}
				else {
					int startParams = this.crefPath.IndexOf('(');
					if (startParams == -1) {
						items = this.crefPath.Substring(this.crefPath.IndexOf(':') + 1).Split('.');
					}
					else {
						items = this.crefPath.Substring(this.crefPath.IndexOf(':') + 1, this.crefPath.IndexOf('(') - 2).Split('.');
						this.Parameters = this.crefPath.Substring(this.crefPath.IndexOf('('));
					}
				}

				switch (this.PathType) {
					case CRefTypes.Namespace:
						this.Namespace = string.Join(".", items);
						break;
					case CRefTypes.Type:
						this.TypeName = items[items.Length - 1];
						this.Namespace = string.Join(".", items, 0, items.Length - 1);
						break;
					default:
						if (items.Length - 2 <= 0) {
							this.PathType = CRefTypes.Error;
						}
						else {
							// -2 because the last element is the element name
							this.TypeName = items[items.Length - 2];
							this.ElementName = items[items.Length - 1];
							this.Namespace = string.Join(".", items, 0, items.Length - 2);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Extracts the cref type portion of the string and sets the <see cref="PathType"/>.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// Thrown when the parser finds a type that it can not handle.
		/// </exception>
		private void ParseType() {
			if (this.crefPath.IndexOf(':') < 0 || string.IsNullOrEmpty(this.crefPath.Substring(0, this.crefPath.IndexOf(':')))) {
				this.PathType = CRefTypes.Error;
				return;
			}

			string typePortion = this.crefPath.Substring(0, this.crefPath.IndexOf(':'));
			switch (typePortion) {
				case CRefConstants.TypeIndicator: this.PathType = CRefTypes.Type; break;
				case CRefConstants.PropertyTypeIndicator: this.PathType = CRefTypes.Property; break;
				case CRefConstants.MethodTypeIndicator: this.PathType = CRefTypes.Method; break;
				case CRefConstants.FieldTypeIndicator: this.PathType = CRefTypes.Field; break;
				case CRefConstants.ErrorTypeIndicator: this.PathType = CRefTypes.Error; break;
				case CRefConstants.NamespaceTypeIndicator: this.PathType = CRefTypes.Namespace; break;
				case CRefConstants.EventTypeIndicator: this.PathType = CRefTypes.Event; break;
				default:
					break;
			}
		}

		/// <summary>
		/// Returns a string representation of this CRefPath.
		/// </summary>
		/// <returns>The cref path constructed from the elements.</returns>
		public override string ToString() {
			string toString = string.Empty;
			switch (this.PathType) {
				case CRefTypes.Namespace:
					toString = string.Format("{0}:{1}",
						CRefConstants.GetIndicatorFor(this.PathType),
						this.Namespace);
					break;
				case CRefTypes.Error: break;
				default:
					string typePortion = string.Format("{0}:{1}.{2}",
						CRefConstants.GetIndicatorFor(this.PathType),
						this.Namespace,
						this.TypeName);
					if (this.PathType != CRefTypes.Type) {
						typePortion += "." + this.ElementName;
					}
					// [#32] - Added code to make cref paths add parameters for
					//	parameterised properties.
					if (this.PathType == CRefTypes.Method  || (this.PathType == CRefTypes.Property && this.Parameters != null)) {
						typePortion += this.Parameters;
					}

					// Operators provide the return types after a "~" character as specified in:
					//	http://msdn.microsoft.com/en-us/library/fsbx0t7x(VS.71).aspx
					if (this.isOperator && !string.IsNullOrEmpty(this.returnType)) {
						typePortion += "~";
						typePortion += this.returnType;
					}

					toString = typePortion;
					break;
			}
			return toString;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Indicates the type of element that is referenced by the CRef path.
		/// </summary>
		public CRefTypes PathType { get; set; }

        /// <summary>
        /// Gets or sets a string that indicates the namespace the type parsed from the cref
        /// path resides in.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets a string that indicates the name of the type from the CRef path.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets a string that is the value of the element name from the cref
        /// path.
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// A string representing the parameter section of the CRefPath.
        /// </summary>
        public string Parameters { get; set; }
		#endregion
	}
}
