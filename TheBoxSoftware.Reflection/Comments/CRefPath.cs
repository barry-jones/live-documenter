using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Comments {
	using TheBoxSoftware.Diagnostics;

	/// <summary>
	/// Class that handles and parses a CRef comment path. A CRef path can contain
	/// a fully qualified link to a type, property, method etc in an assembly.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("cref={ToString()}")]
	public sealed class CRefPath : Signitures.SignitureConvertor {
		private string crefPath;
		private string returnType;
		private bool isOperator = false;

		#region Constructors
		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		public CRefPath() { }

		/// <summary>
		/// Constructs a cref path for the provided <paramref name="field"/>.
		/// </summary>
		/// <param name="field">The field to create the path for.</param>
		public CRefPath(FieldDef field)
			: this(CRefTypes.Field, field.Type.Namespace, field.Type.Name, field.Name) {
		}

		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		/// <param name="type">The TypRef to initialise the path with.</param>
		public CRefPath(TypeRef type)
			: this(CRefTypes.Type, type.Namespace, type.Name, string.Empty) {
		}

		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		/// <param name="property">The property to initialise the path with.</param>
		public CRefPath(PropertyDef property)
			: this(CRefTypes.Property, property.Type.Namespace, property.Type.Name, property.Name) {
			MethodDef method = property.GetMethod ?? property.SetMethod;
			this.Parameters = property.IsIndexer ? this.Convert(method) : string.Empty;
		}

		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		/// <param name="cEvent">The event to create the path to.</param>
		public CRefPath(EventDef cEvent) 
			: this(CRefTypes.Event, cEvent.Type.Namespace, cEvent.Type.Name, cEvent.Name) {
		}

		/// <summary>
		/// Initialises a new instance of the CRefPath class.
		/// </summary>
		/// <param name="method">The method to initialise the path with.</param>
		public CRefPath(MethodDef method)
			: this(CRefTypes.Method, method.Type.Namespace, method.Type.Name, method.Name) {
			// We need to adjust the method name if is a constructor
			if (method.IsConstructor) {
				this.ElementName = method.Name.Replace('.', '#');
			}
			if (method.IsGeneric) {
				this.ElementName += "``" + method.GenericTypes.Count;
			}
			this.Parameters = this.Convert(method);
			if (method.IsOperator && method.IsConversionOperator) {
				this.isOperator = true;
				this.returnType = method.Signiture.GetReturnTypeToken().ResolveType(method.Assembly, method).GetFullyQualifiedName();
			}
		}

		// TODO: Implement Events, Delegates

		/// <summary>
		/// Private constructor initialises a new instance of the CRefPath class.
		/// </summary>
		/// <param name="type">The cref path type.</param>
		/// <param name="namesp">The namespace for the path.</param>
		/// <param name="name">The type name for the path.</param>
		/// <param name="element">The element name for the path.</param>
		private CRefPath(CRefTypes type, string namesp, string name, string element) {
			this.PathType = type;
			this.Namespace = namesp;
			this.TypeName = name;
			this.ElementName = element;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Factory method for creating CRefPath entries when the type
		/// is unknown.
		/// </summary>
		/// <param name="member">The member to create a path for</param>
		/// <returns>The CRefPath</returns>
		/// <exception cref="NotImplementedException">
		/// Thrown when the type of the <see cref="ReflectedMember"/> is not
		/// implemented.
		/// </exception>
		public static CRefPath Create(ReflectedMember member) {
			CRefPath path = null;

			if (member is FieldDef) {
				path = new CRefPath((FieldDef)member);
			}
			else if (member is TypeRef) {
				path = new CRefPath((TypeRef)member);
			}
			else if (member is PropertyDef) {
				path = new CRefPath((PropertyDef)member);
			}
			else if (member is EventDef) {
				path = new CRefPath((EventDef)member);
			}
			else if (member is MethodDef) {
				path = new CRefPath((MethodDef)member);
			}
			else {
				throw new NotImplementedException(string.Format("The '{0}' type is not supported.",
					member.GetType().ToString()
					));
			}

			return path;
		}

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
					TraceHelper.WriteLine("Parsing for cref paths of type " + typePortion + " has not been implemented");
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

		/// <summary>
		/// Finds the member this <see cref="CRefPath"/> refers to in the provided type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The found member ref.</returns>
		/// <exception cref="ArgumentNullException">When <paramref name="type"/> is null.</exception>
		public ReflectedMember FindIn(TypeDef type) {
			if (type == null) throw new ArgumentNullException("type");

			if (this.PathType == CRefTypes.Namespace || this.PathType == CRefTypes.Type || this.PathType == CRefTypes.Error) {
				throw new InvalidOperationException(string.Format("Can not find member in a type when the path type is '{0}'", this.PathType.ToString()));
			}

			ReflectedMember member = null;
			List<ReflectedMember> foundMembers = new List<ReflectedMember>();

			// find all potential members
			switch (this.PathType) {
				case CRefTypes.Event:
					foundMembers.AddRange(type.Events.FindAll(e => e.Name == this.ElementName).ToArray());
					break;
				case CRefTypes.Field:
					foundMembers.AddRange(type.Fields.FindAll(e => e.Name == this.ElementName).ToArray());
					break;
				case CRefTypes.Method:
					string elementName = this.ElementName.Replace('#', '.');
					int genParameters = 0;
					if (elementName.Contains('`')) {
						genParameters = int.Parse(elementName.Substring(elementName.Length - 1, 1));
						elementName = elementName.Substring(0, elementName.IndexOf('`'));
					}
					MethodDef[] foundMethods = type.Methods.FindAll(e => e.Name == elementName).ToArray();
					if (foundMethods.Length > 1 && genParameters > 0) {
						for (int i = 0; i < foundMethods.Length; i++) {
							if (foundMethods[i].GenericTypes != null && foundMethods[i].GenericTypes.Count == genParameters) {
								foundMembers.Add(foundMethods[i]);
							}
						}
					}
					else {
						foundMembers.AddRange(foundMethods);
					}
					break;
				case CRefTypes.Property:
					foundMembers.AddRange(type.Properties.FindAll(e => e.Name == this.ElementName).ToArray());
					break;
			}

			if (foundMembers.Count == 1) {
				member = foundMembers[0];
			}
			else if (foundMembers.Count > 1) {
				// the elements will differ by the parameters, this is slow!
				foreach (ReflectedMember current in foundMembers) {
					string found = CRefPath.Create(current).ToString();
					if (found == this.ToString()) {
						member = current;
						break;
					}
				}
			}

			return member;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Indicates the type of element that is referenced by the CRef path.
		/// </summary>
		public CRefTypes PathType { get; set; }
		#endregion
	}
}
