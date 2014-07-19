using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	/// <summary>
	/// A base implementation of a class that converts a signiture
	/// to another medium.
	/// </summary>
	/// <seealso cref="TheBoxSoftware.Reflection.Comments.CRefPath"/>
	/// <seealso cref="TheBoxSoftware.Reflection.DisplayNameSignitureConvertor"/>
	public abstract class SignitureConvertor {
		protected string GenericStart = "{";
		protected string GenericEnd = "}";
		protected string ParameterSeperater = ",";
		protected string ByRef = "@";
		protected bool ByRefAtFront = false;
		protected bool IncludeFirstParameter = true;

		#region Method
		/// <summary>
		/// Converts the method parameters to a cref path implementation, this in itself
		/// is not enough. This is used by the cref parse methods.
		/// </summary>
		/// <param name="method">The method to convert the parameters for.</param>
		/// <returns>The converted string.</returns>
		protected string Convert(MethodDef method) {
			if (method == null) {
				throw new ArgumentNullException("method");
			}
			Signiture loadedSigniture = method.Signiture;
			StringBuilder convertedSigniture = new StringBuilder();

			List<ParamSignitureToken> parametersToConvert = loadedSigniture.GetParameterTokens();
			if (parametersToConvert != null && parametersToConvert.Count > 0) {
				bool hadReturnParameter = false;
				bool isFirstParameter = true;
				convertedSigniture.Append("(");
				for (int i = 0; i < method.Parameters.Count; i++) {
					ParamDef currentParameter = method.Parameters[i];
					if (currentParameter.Sequence == 0) {
						hadReturnParameter = true;
						continue;
					}
					if (!this.IncludeFirstParameter && (i == 0 || hadReturnParameter && i == 1)) continue;

					ParamSignitureToken currentToken = parametersToConvert[hadReturnParameter ? i - 1 : i];
					TypeRef typeRef = currentToken.ResolveParameter(method.Assembly, currentParameter);

					if (isFirstParameter) {
						isFirstParameter = false;
					}
					else {
						convertedSigniture.Append(ParameterSeperater);
					}

					if (this.ByRefAtFront) {
						if (currentToken.IsByRef) {
							convertedSigniture.Append(this.ByRef);
						}
					}

					this.Convert(
						convertedSigniture,
						method.Assembly,
						currentParameter,
						currentToken,
						currentToken.ElementType.ElementType,
						typeRef);

					if (!this.ByRefAtFront) {
						if (currentToken.IsByRef) {
							convertedSigniture.Append(this.ByRef);
						}
					}
				}

				convertedSigniture.Append(")");
			}

			return convertedSigniture.ToString();
		}

		/// <summary>
		/// Method which performs the actual conversion of a signiture to a cref string.
		/// </summary>
		/// <param name="sb">The string builder to hold the converted text</param>
		/// <param name="assembly">The assembly the current parameter is defined in</param>
		/// <param name="param">The parameter definition, required for token resolution</param>
		/// <param name="currentToken">The current token to converty</param>
		/// <param name="elementType">The type of element the token represents</param>
		/// <param name="resolvedType">The resolved type for this token.</param>
		private void Convert(StringBuilder sb, AssemblyDef assembly, ParamDef param, SignitureToken currentToken, ElementTypes elementType, TypeRef resolvedType) {
			StringBuilder convertedSigniture = sb;
			if (currentToken.TokenType == SignitureTokens.Param) {
				SignitureToken paramToken = ((ParamSignitureToken)currentToken).Tokens[
					((ParamSignitureToken)currentToken).Tokens.Count - 1
					];
				if (paramToken.TokenType == SignitureTokens.Type) {
					currentToken = (TypeSignitureToken)paramToken;
					elementType = ((TypeSignitureToken)paramToken).ElementType.ElementType;
				}
				else {
					currentToken = (ElementTypeSignitureToken)paramToken;
					elementType = ((ElementTypeSignitureToken)paramToken).ElementType;
				}
			}
			TypeSignitureToken typeToken = currentToken as TypeSignitureToken;
			ElementTypeSignitureToken elementToken = currentToken as ElementTypeSignitureToken;
			switch (elementType) {
				case ElementTypes.Var: this.ConvertVar(convertedSigniture, typeToken.ElementType.Token, param); break;
				case ElementTypes.MVar: this.ConvertMVar(convertedSigniture, typeToken.ElementType.Token, param); break;
				case ElementTypes.SZArray:
					// TODO: Fix the custom modifier section
					if (typeToken.Tokens[0] is CustomModifierToken) {
						NotImplementedException ex = new NotImplementedException("Custom modifiers are not implemented on SZArray");
						ex.Data["token"] = currentToken;
						throw ex;
					}
					ElementTypes szArrayElementType;
					if (typeToken.Tokens[1].TokenType == SignitureTokens.Type) {
						szArrayElementType = ((TypeSignitureToken)typeToken.Tokens[1]).ElementType.ElementType;
					}
					else {
						szArrayElementType = ((ElementTypeSignitureToken)typeToken.Tokens[1]).ElementType;
					}
					this.Convert(
						sb,
						assembly,
						param,
						typeToken.Tokens[1],
						szArrayElementType,
						resolvedType);
					convertedSigniture.Append("[]");
					break;
				case ElementTypes.Array:
					ArrayShapeSignitureToken shape = typeToken.Tokens.Last() as ArrayShapeSignitureToken;
					this.ConvertArray(convertedSigniture, resolvedType, shape);
					break;
				case ElementTypes.GenericInstance:
					TypeRef genericType = ((ElementTypeSignitureToken)typeToken.Tokens[1]).ResolveToken(assembly);
					//string fullName = genericType.GetFullyQualifiedName();
					//convertedSigniture.Append(fullName.Substring(0, fullName.IndexOf('`')));
					this.GetTypeName(convertedSigniture, genericType);
					GenericArgumentCountSignitureToken argsCount = typeToken.GetGenericArgumentCount();
					bool isFirstArgument = true;
					if (argsCount.Count > 0) {
						sb.Append(this.GenericStart);
						for (int j = 0; j < argsCount.Count; j++) {
							if (isFirstArgument) {
								isFirstArgument = false;
							}
							else {
								sb.Append(",");
							}

							TypeRef argResolvedType;
							ElementTypes elType;
							if (typeToken.Tokens[j + 3].TokenType == SignitureTokens.ElementType) {
								ElementTypeSignitureToken gESig = (ElementTypeSignitureToken)typeToken.Tokens[j + 3];
								argResolvedType = gESig.ResolveToken(assembly);
								elType = gESig.ElementType;
							}
							else {
								TypeSignitureToken gTSig = (TypeSignitureToken)typeToken.Tokens[j + 3];
								argResolvedType = gTSig.ResolveType(assembly, param);
								elType = gTSig.ElementType.ElementType;
							}

							this.Convert(
								sb,
								assembly,
								param,
								typeToken.Tokens[j + 3],
								elType,
								argResolvedType);
						}
						sb.Append(this.GenericEnd);
					}
					break;
				case ElementTypes.Class:
				case ElementTypes.ValueType:
				case ElementTypes.Char:
				case ElementTypes.I:
				case ElementTypes.I1:
				case ElementTypes.I2:
				case ElementTypes.I4:
				case ElementTypes.I8:
				case ElementTypes.Object:
				case ElementTypes.R4:
				case ElementTypes.R4_HFA:
				case ElementTypes.R8:
				case ElementTypes.R8_HFA:
				case ElementTypes.String:
				case ElementTypes.U:
				case ElementTypes.U1:
				case ElementTypes.U2:
				case ElementTypes.U4:
				case ElementTypes.U8:
				case ElementTypes.TypedByRef:
				case ElementTypes.Boolean:
					this.GetTypeName(convertedSigniture, resolvedType);
					break;
				case ElementTypes.Ptr:
					this.GetTypeName(convertedSigniture, resolvedType);
					//convertedSigniture.Append(resolvedType.GetFullyQualifiedName());
					convertedSigniture.Append("*");
					break;
				case ElementTypes.FunctionPointer:
					NotImplementedException fnPtrEx = new NotImplementedException("Function Pointer is not implemented yet.");
					fnPtrEx.Data["token"] = currentToken;
					throw fnPtrEx;
					break;
			}
		}

		/// <summary>
		/// Obtains the type name for the specified <see cref="TypeRef"/>.
		/// </summary>
		/// <param name="sb">The current cref name for the signiture to append the details to.</param>
		/// <param name="type">The type to obtain the display name for.</param>
		protected virtual void GetTypeName(StringBuilder sb, TypeRef type) {
			if (type.IsGeneric) {
				string fullName = type.GetFullyQualifiedName();
				sb.Append(fullName.Substring(0, fullName.IndexOf('`')));
			}
			else {
				sb.Append(type.GetFullyQualifiedName());
			}
		}

		/// <summary>
		/// Converts a generic variable for cref.
		/// </summary>
		/// <param name="sb">The current cref name for the signiture to append details to.</param>
		/// <param name="sequence">The sequence number of the current generic variable</param>
		/// <param name="parameter">The parameter definition information.</param>
		protected virtual void ConvertVar(StringBuilder sb, int sequence, ParamDef parameter) {
			sb.Append("`");
			sb.Append(sequence);
		}

		/// <summary>
		/// Converts a generic variable for cref.
		/// </summary>
		/// <param name="sb">The current cref name for the signiture to append details to.</param>
		/// <param name="sequence">The sequence number of the current generic variable</param>
		/// <param name="parameter">The parameter definition information.</param>
		protected virtual void ConvertMVar(StringBuilder sb, int sequence, ParamDef parameter) {
			sb.Append("``");
			sb.Append(sequence);
		}

		/// <summary>
		/// Overridden convertor for arrays. Converts the <see cref="ArrayShapeSignitureToken"/>
		/// to its correct cref name equivelant.
		/// </summary>
		/// <param name="sb">The string being constructed containing the cref name.</param>
		/// <param name="resolvedType">The type the parameter has been resolved to</param>
		/// <param name="shape">The signiture token detailing the shape of the array.</param>
		internal virtual void ConvertArray(StringBuilder sb, TypeRef resolvedType, ArrayShapeSignitureToken shape) {
			this.GetTypeName(sb, resolvedType);
			//convertedSigniture.Append(resolvedType.GetFullyQualifiedName());
			sb.Append("[");
			//ArrayShapeSignitureToken shape = typeToken.Tokens.Last() as ArrayShapeSignitureToken;
			for (int i = 0; i < shape.Rank; i++) {
				if (i != 0 && i != shape.Rank) {
					sb.Append(",");
				}
				bool hasLoBound = i < shape.NumLoBounds;
				bool hasSize = i < shape.NumSizes;
				if (hasLoBound) {
					sb.Append(shape.LoBounds[i]);
				}
				if (hasLoBound || hasSize) {
					sb.Append(":");
				}
				if (hasSize) {
					sb.Append(shape.Sizes[i]);
				}
			}
			sb.Append("]");
		}
		#endregion

		#region Properties
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
