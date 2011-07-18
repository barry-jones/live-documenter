using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Class that handles the association of the VS icons to each of
	/// the different elements (properties, methods etc).
	/// </summary>
	public sealed class ElementIconConstants
	{
		private const string basePath = "Resources/ElementIcons/vsobject_{0}.png";

		/// <summary>
		/// Obtains the path to an icon image for the specified object.
		/// </summary>
		/// <param name="o">The object to find the icon for.</param>
		/// <returns>The path to the icon for the member or empty string if no icon found.</returns>
		/// <remarks>
		/// <paramref name="o"/> does need to a <see cref="ReflectedMember"/>
		/// at the moment. Other types will fail and return an empty string.
		/// </remarks>
		public static string GetIconPathFor(object o)
		{
			string path = string.Empty;
			if (o is ReflectedMember)
				path = ElementIconConstants.GetIconPathFor((ReflectedMember)o);
			else
			{
			}
			return path;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		/// <remarks>
		/// The order in which the visibility modifiers are checked is important as
		/// they use the first 3 bits of the flag, and they can use the first bit to
		/// represent private and the 3 and 1st bit to represent public.
		/// </remarks>
		private static string GetIconPathFor(ReflectedMember member)
		{
			string name = string.Empty;

			if (member is AssemblyDef) {
				name = "assembly";
			}
			else if (member is TypeDef) {
				name = "class";
				TypeDef typeDef = (TypeDef)member;
				if (typeDef != null) {
					if (typeDef.IsInterface) {
						name = "interface";
					}
					else if (typeDef.IsStructure) {
						name = "structure";
					}
					else if (typeDef.IsDelegate) {
						name = "delegate";
					}
					else if (typeDef.IsEnumeration) {
						name = "enum";
					}
				}
			}
			else if (member is EventDef) {
				name = "event";
			}
			else if (member is FieldDef) {
				name = "field";
				FieldDef fieldDef = (FieldDef)member;
				if (fieldDef.IsConstant) {
					name = "constant";
				}
				else if (fieldDef.IsOperator) {
					name = "operator";
				}
			}
			else if (member is MethodDef) {
				name = "method";
			}
			else if (member is PropertyDef) {
				name = "properties";
			}

			// add the visibility modifier
			if (member is TypeDef || member is EventDef || member is FieldDef || member is MethodDef || member is PropertyDef) {
				switch (member.MemberAccess) {
					case Visibility.Protected:
						name += "_protected";
						break;
					case Visibility.Internal:
						name += "_sealed";
						break;
					case Visibility.InternalProtected:
						name += "_friend";
						break;
					case Visibility.Private:
						name += "_private";
						break;
					case Visibility.Public:
						break;
				}
			}

			// TODO: Handle namespaces and assemblies

			return string.IsNullOrEmpty(name) ? string.Empty : string.Format(basePath, name);
		}
	}
}
