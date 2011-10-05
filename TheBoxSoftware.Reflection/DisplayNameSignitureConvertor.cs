using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// A <see cref="SignitureConvertor"/> implementation that creates user
	/// displayable names for types, methods and properties.
	/// </summary>
	public sealed class DisplayNameSignitureConvertor : Signitures.SignitureConvertor {
		private TypeDef type;
		private MethodDef method;
		private PropertyDef property;
		private bool includeNamespace;
		private bool includeParameters = false;
		private bool includeTypeName = true;

		/// <summary>
		/// Initialises a new instance of the DisplayNameSignitureConvertor.
		/// </summary>
		/// <param name="method">The method to obtain a display name for.</param>
		/// <param name="includeNamespace">Should the details of the namespace be included.</param>
		/// <param name="includeParamaters">Should the methods parameters be included.</param>
		public DisplayNameSignitureConvertor(MethodDef method, bool includeNamespace, bool includeParamaters)
			: this() {
			this.type = (TypeDef)method.Type;
			this.method = method;
			this.includeNamespace = includeNamespace;
			this.includeParameters = includeParamaters;
			this.includeTypeName = false;
		}

		/// <summary>
		/// Initialises a new instance of the DisplayNameSignitureConvertor.
		/// </summary>
		/// <param name="method">The method to obtain a display name for.</param>
		/// <param name="includeNamespace">Should the details of the namespace be included.</param>
		/// <param name="includeParamaters">Should the methods parameters be included.</param>
		/// <param name="isFromExtendedType">Indicates this is an extension method from the type it is extending.</param>
		public DisplayNameSignitureConvertor(MethodDef method, bool includeNamespace, bool includeParameters, bool isFromExtendedType)
			: this(method, includeNamespace, includeParameters) {
				this.IncludeFirstParameter = false;
		}

		/// <summary>
		/// Initialises a new instance of the DisplayNameSignitureConvertor
		/// </summary>
		/// <param name="property">The property to obtain the display name for.</param>
		/// <param name="includeNamespace">Should the details of the namespace be included?</param>
		/// <param name="includeParamaters">Should the parameters for the property be included?</param>
		public DisplayNameSignitureConvertor(PropertyDef property, bool includeNamespace, bool includeParamaters) {
			this.type = (TypeDef)property.Type;
			this.property = property;
			this.method = property.GetMethod != null ? property.GetMethod : property.SetMethod;
			this.includeNamespace = includeNamespace;
			this.includeParameters = includeParamaters;
			this.includeTypeName = false;
		}

		/// <summary>
		/// Initialises a new instance of the DisplayNameSignitureConvertor.
		/// </summary>
		/// <param name="type">The type being converted.</param>
		/// <param name="includeNamespace">Should the details of the namespace be included in the display name.</param>
		public DisplayNameSignitureConvertor(TypeDef type, bool includeNamespace) : this() {
			this.type = type;
			this.includeNamespace = includeNamespace;
		}

		/// <summary>
		/// Initialises a new instance of the DisplayNameSignitureConvertor.
		/// </summary>
		private DisplayNameSignitureConvertor() {
			// Set the generic element tags
			this.GenericEnd = ">";
			this.GenericStart = "<";
			this.ByRef = "ref ";
			this.ByRefAtFront = true;
			this.ParameterSeperater = ", ";
		}

		/// <summary>
		/// Implementation of the convert method, that is used to create a display
		/// version of the signiture this convertor has been instantiated with.
		/// </summary>
		/// <returns>The fully converted signiture as a string.</returns>
		/// <exception cref="ReflectionException">
		/// Thrown when an error occurs while processing the display name, see the exception
		/// details for more information.
		/// </exception>
		public string Convert() {
			try {
				StringBuilder converted = new StringBuilder();
				bool isIndexer = false;

				// Convert the type portion
				if (this.includeTypeName) {
					this.GetTypeName(converted, this.type);
					if (this.type.IsGeneric) {
						converted.Append(this.GenericStart);
						bool first = true;
						foreach (GenericTypeRef type in this.type.GetGenericTypes()) {
							if (first) {
								first = false;
							}
							else {
								converted.Append(", ");
							}
							converted.Append(type.Name);
						}
						converted.Append(this.GenericEnd);
					}
				}

				// Convert the method portion
				if (method != null) {
					if (method.IsConstructor && property == null) {
						this.GetTypeName(converted, this.type);
					}
					else if (method.IsOperator && property == null) {
						if (method.IsConversionOperator) {
							Signiture sig = method.Signiture;
							TypeRef convertToRef = sig.GetReturnTypeToken().ResolveType(method.Assembly, method);
							TypeRef convertFromRef = sig.GetParameterTokens()[0].ResolveParameter(method.Assembly, method.Parameters[0]);

							converted.Append(method.Name.Substring(3));
							converted.Append("(");
							converted.Append(convertToRef.GetDisplayName(false));
							converted.Append(" to ");
							converted.Append(convertFromRef.GetDisplayName(false));
							converted.Append(")");
						}
						else {
							converted.Append(method.Name.Substring(3));
						}
					}
					else if (property == null) {
						converted.Append(method.Name);
					}
					else {
						converted.Append(property.Name);
					}

					if (method.IsGeneric) {
						converted.Append(this.GenericStart);
						bool first = true;
						foreach (GenericTypeRef type in method.GetGenericTypes()) {
							if (first) {
								first = false;
							}
							else {
								converted.Append(", ");
							}
							converted.Append(type.Name);
						}
						converted.Append(this.GenericEnd);
					}

					if (this.includeParameters && !method.IsConversionOperator && 
						(property == null || property.IsIndexer) // only display parameters for indexer properties
						) {
						string parameters = this.Convert(method);
						if (string.IsNullOrEmpty(parameters) && property == null) {
							parameters = "()";
						}
						converted.Append(parameters);
					}
				}
				return converted.ToString();
			}
			catch (Exception ex) {
				if (this.property != null) {
					throw new ReflectionException(this.property, "Error processing display name signiture for a property", ex);
				}
				else if (this.method != null) {
					throw new ReflectionException(this.method, "Error processing display name signiture for a method", ex);
				}
				else {
					throw new ReflectionException(this.type, "Error processing display name signiture for a type", ex);
				}
			}
		}

		/// <summary>
		/// Obtains the type name for the specified <see cref="TypeRef"/>.
		/// </summary>
		/// <param name="sb">The current display name for the signiture to append the details to.</param>
		/// <param name="type">The type to obtain the display name for.</param>
		protected override void GetTypeName(StringBuilder sb, TypeRef type) {
			string typeName = type.GetFullyQualifiedName();
			if (!this.includeNamespace) {
				typeName = type.Name;
			}
			else {
				typeName = type.GetFullyQualifiedName();
			}

			if (type.IsGeneric) {
				int genIdIndex = typeName.IndexOf('`');
				if (genIdIndex != -1) {
					typeName = typeName.Substring(0, genIdIndex);
				}
			}

			sb.Append(typeName);
		}

		/// <summary>
		/// Converts a generic variable for display.
		/// </summary>
		/// <param name="sb">The current display name for the signiture to append details to.</param>
		/// <param name="sequence">The sequence number of the current generic variable.</param>
		/// <param name="parameter">The parameter definition information.</param>
		protected override void ConvertMVar(StringBuilder sb, int sequence, ParamDef parameter) {
			// Type Generic Parameter
			GenericTypeRef foundGenericType = null;
			foreach (GenericTypeRef current in this.method.GenericTypes) {
				if (current.Sequence == sequence) {
					foundGenericType = current;
					break;
				}
			}
			if (foundGenericType != null) {
				sb.Append(foundGenericType.Name);
			}
		}

		/// <summary>
		/// Converts a generic variable for display.
		/// </summary>
		/// <param name="sb">The current display name for the signiture to append details to.</param>
		/// <param name="sequence">The sequence number of the current generic variable</param>
		/// <param name="parameter">The parameter definition information.</param>
		protected override void ConvertVar(StringBuilder sb, int sequence, ParamDef parameter) {
			// Type Generic Parameter
			GenericTypeRef foundGenericType = null;
			foreach (GenericTypeRef current in this.type.GenericTypes) {
				if (current.Sequence == sequence) {
					foundGenericType = current;
					break;
				}
			}
			if (foundGenericType != null) {
				sb.Append(foundGenericType.Name);
			}
		}

		/// <summary>
		/// Overridden convertor for arrays. Converts the <see cref="ArrayShapeSignitureToken"/>
		/// to its correct display name equivelant.
		/// </summary>
		/// <param name="sb">The string being constructed containing the display name.</param>
		/// <param name="resolvedType">The type the parameter has been resolved to</param>
		/// <param name="shape">The signiture token detailing the shape of the array.</param>
		protected override void ConvertArray(StringBuilder sb, TypeRef resolvedType, ArrayShapeSignitureToken shape) {
			this.GetTypeName(sb, resolvedType);
			sb.Append("[");
			for (int i = 0; i < shape.Rank; i++) {
				if (i != 0 && i != shape.Rank) {
					sb.Append(",");
				}
				bool hasLoBound = i < shape.NumLoBounds;
				bool hasSize = i < shape.NumSizes;
				if (hasLoBound) {
					sb.Append(shape.LoBounds[i]);
				}
				if (hasSize) {
					sb.Append(shape.Sizes[i]);
				}
			}
			sb.Append("]");
		}
	}
}
