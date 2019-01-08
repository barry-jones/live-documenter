
namespace TheBoxSoftware.API.LiveDocumenter
{
    using System;

    /// <summary>
    /// Class that handles and parses a CRef comment path. A CRef path can contain
    /// a fully qualified link to a type, property, method etc in an assembly.
    /// </summary>
    /// <include file='Documentation\contententry.xml' path='members/member[@name="CRefPath"]/*'/>
    [System.Diagnostics.DebuggerDisplay("cref={ToString()}")]
	public sealed class CRefPath 
    {
		private string _crefPath;
		private string _returnType;
		private bool _isOperator = false;

		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		public CRefPath() { }

		/// <summary>
		/// Parses the provided path and returns a populated cref path instance.
		/// </summary>
		/// <param name="crefPath">The cref path to control.</param>
		/// <exception cref="ArgumentNullException">The specified path was null or empty.</exception>
		public static CRefPath Parse(string crefPath) 
        {
			if (string.IsNullOrEmpty(crefPath))
				throw new ArgumentNullException("path");
			CRefPath parsedPath = new CRefPath();
			parsedPath._crefPath = crefPath;
			parsedPath.Parse();
			return parsedPath;
		}

		/// <summary>
		/// Parses the contents of the contained path and stores the details in the
		/// classes properties.
		/// </summary>
		private void Parse() 
        {
			ParseType();

			if (PathType != CRefTypes.Error)
            {
				string[] items;
				int startParams = _crefPath.IndexOf('(');
				if (startParams == -1) 
                {
					items = _crefPath.Substring(_crefPath.IndexOf(':') + 1).Split('.');
				}
				else 
                {
					items = _crefPath.Substring(_crefPath.IndexOf(':') + 1, _crefPath.IndexOf('(') - 2).Split('.');
					Parameters = _crefPath.Substring(_crefPath.IndexOf('('));
				}

				switch (PathType)
                {
					case CRefTypes.Namespace:
						Namespace = string.Join(".", items);
						break;
					case CRefTypes.Type:
						TypeName = items[items.Length - 1];
						Namespace = string.Join(".", items, 0, items.Length - 1);
						break;
					default:
						if (items.Length - 2 <= 0)
                        {
							PathType = CRefTypes.Error;
						}
						else
                        {
							// -2 because the last element is the element name
							TypeName = items[items.Length - 2];
							ElementName = items[items.Length - 1];
							Namespace = string.Join(".", items, 0, items.Length - 2);
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
		private void ParseType() 
        {
			if (_crefPath.IndexOf(':') < 0 || string.IsNullOrEmpty(_crefPath.Substring(0, _crefPath.IndexOf(':'))))
            {
				PathType = CRefTypes.Error;
				return;
			}

			string typePortion = _crefPath.Substring(0, _crefPath.IndexOf(':'));
			switch (typePortion) 
            {
				case CRefConstants.TypeIndicator: PathType = CRefTypes.Type; break;
				case CRefConstants.PropertyTypeIndicator: PathType = CRefTypes.Property; break;
				case CRefConstants.MethodTypeIndicator: PathType = CRefTypes.Method; break;
				case CRefConstants.FieldTypeIndicator: PathType = CRefTypes.Field; break;
				case CRefConstants.ErrorTypeIndicator: PathType = CRefTypes.Error; break;
				case CRefConstants.NamespaceTypeIndicator: PathType = CRefTypes.Namespace; break;
				case CRefConstants.EventTypeIndicator: PathType = CRefTypes.Event; break;
				default:
					break;
			}
		}

		/// <summary>
		/// Returns a string representation of this CRefPath.
		/// </summary>
		/// <returns>The cref path constructed from the elements.</returns>
		public override string ToString() 
        {
			string toString = string.Empty;
			switch (PathType) 
            {
				case CRefTypes.Namespace:
					toString = string.Format("{0}:{1}",
						CRefConstants.GetIndicatorFor(PathType),
						Namespace);
					break;
				case CRefTypes.Error: break;
				default:
					string typePortion = string.Format("{0}:{1}.{2}",
						CRefConstants.GetIndicatorFor(PathType),
						Namespace,
						TypeName);
					if (PathType != CRefTypes.Type)
                    {
						typePortion += "." + ElementName;
					}
					// [#32] - Added code to make cref paths add parameters for
					//	parameterised properties.
					if (PathType == CRefTypes.Method  || (PathType == CRefTypes.Property && Parameters != null)) 
                    {
						typePortion += Parameters;
					}

					// Operators provide the return types after a "~" character as specified in:
					//	http://msdn.microsoft.com/en-us/library/fsbx0t7x(VS.71).aspx
					if (_isOperator && !string.IsNullOrEmpty(_returnType)) 
                    {
						typePortion += "~";
						typePortion += _returnType;
					}

					toString = typePortion;
					break;
			}
			return toString;
		}

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
	}
}
