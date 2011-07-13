using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Documentation.Exporting.Rendering {
	public static class ReflectionHelper {
		/// <summary>
		/// Obtains a key that uniquely identifies the member in the library, for all libraries
		/// loaded in to the documenter.
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <param name="member">The member</param>
		/// <returns>A long that is unique in the application</returns>
		public static long GetUniqueKey(AssemblyDef assembly, ReflectedMember member) {
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
		public static long GetUniqueKey(AssemblyDef assembly) {
			return ((long)assembly.UniqueId) << 32;
		}

		public static string GetType(ReflectedMember member) {
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

			return name;
		}


		/// <summary>
		/// Obtains a name to display for the reflected member.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns>A name for the ReflectedMember</returns>
		public static string GetDisplayName(ReflectedMember entry) {
			string displayName = string.Empty;

			if (entry is FieldDef) {
				displayName = ((FieldDef)entry).Name;
			}
			else if (entry is PropertyDef) {
				displayName = ((PropertyDef)entry).GetDisplayName(false);
			}
			else if (entry is MethodDef) {
				displayName = ((MethodDef)entry).GetDisplayName(false, true);
			}
			else if (entry is EventDef) {
				displayName = ((EventDef)entry).Name;
			}

			return displayName;
		}

		/// <summary>
		/// Obtains the path to an icon image for the specified object.
		/// </summary>
		/// <param name="o">The object to find the icon for.</param>
		/// <returns>The path to the icon for the member or empty string if no icon found.</returns>
		/// <remarks>
		/// <paramref name="o"/> does need to a <see cref="ReflectedMember"/>
		/// at the moment. Other types will fail and return an empty string.
		/// </remarks>
		public static string GetVisibility(object o) {
			string path = string.Empty;
			if (o is ReflectedMember)
				path = ReflectionHelper.GetVisibility((ReflectedMember)o);
			else {
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
		private static string GetVisibility(ReflectedMember member) {
			string name = "public";

			if (member is AssemblyDef) {
			}
			else if (member is TypeDef) {
				TypeDef typeDef = (TypeDef)member;
				if (typeDef != null) {
					if ((typeDef.Flags & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic) {
					}
					else if ((typeDef.Flags & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily) {
						name = "protected";
					}
					else if ((typeDef.Flags & TypeAttributes.NestedAssembly) == TypeAttributes.NestedAssembly) {
						name = "sealed";
					}
					else if ((typeDef.Flags & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate) {
						name = "private";
					}
					else if ((typeDef.Flags & TypeAttributes.Public) == TypeAttributes.Public) {
					}
					else if ((typeDef.Flags & TypeAttributes.NotPublic) == TypeAttributes.NotPublic) {
						name = "sealed";
					}
				}
			}
			else if (member is EventDef) {
				EventDef eventDef = (EventDef)member;

				if (eventDef != null) {
					MethodDef addMethod = eventDef.GetAddEventMethod();
					MethodDef removeMethod = eventDef.GetRemoveEventMethod();
					MethodAttributes addVisibility = addMethod != null
						? addMethod.Attributes & MethodAttributes.MemberAccessMask
						: 0;
					MethodAttributes removeVisibility = removeMethod != null
						? removeMethod.Attributes & MethodAttributes.MemberAccessMask
						: 0;
					MethodAttributes visibility = (((int)addVisibility) > ((int)removeVisibility))
						? addVisibility
						: removeVisibility;
					if ((visibility & MethodAttributes.Public) == MethodAttributes.Public) {
					}
					else if ((visibility & MethodAttributes.Family) == MethodAttributes.Family) {
						name = "protected";
					}
					else if ((visibility & MethodAttributes.Assem) == MethodAttributes.Assem) {
						name = "sealed";
					}
					else if ((visibility & MethodAttributes.Private) == MethodAttributes.Private) {
						name = "private";
					}
				}
			}
			else if (member is FieldDef) {
				FieldDef fieldDef = (FieldDef)member;

				if ((fieldDef.Flags & FieldAttributes.Public) == FieldAttributes.Public) {
				}
				else if ((fieldDef.Flags & FieldAttributes.Family) == FieldAttributes.Family) {
					name = "protected";
				}
				else if ((fieldDef.Flags & FieldAttributes.Assembly) == FieldAttributes.Assembly) {
					name = "sealed";
				}
				else if ((fieldDef.Flags & FieldAttributes.Private) == FieldAttributes.Private) {
					name = "private";
				}
			}
			else if (member is MethodDef) {
				MethodDef methodDef = (MethodDef)member;

				if ((methodDef.Attributes & MethodAttributes.Public) == MethodAttributes.Public) {
					// stay at method
				}
				else if ((methodDef.Attributes & MethodAttributes.Family) == MethodAttributes.Family) {
					name = "protected";
				}
				else if ((methodDef.Attributes & MethodAttributes.Assem) == MethodAttributes.Assem) {
					name = "sealed";
				}
				else if ((methodDef.Attributes & MethodAttributes.Private) == MethodAttributes.Private) {
					name = "private";
				}
			}
			else if (member is PropertyDef) {
				PropertyDef propertyDef = (PropertyDef)member;

				if (propertyDef != null) {
					MethodAttributes getterVisibility = propertyDef.GetMethod != null
						? propertyDef.GetMethod.Attributes & MethodAttributes.MemberAccessMask
						: 0;
					MethodAttributes setterVisibility = propertyDef.SetMethod != null
						? propertyDef.SetMethod.Attributes & MethodAttributes.MemberAccessMask
						: 0;
					MethodAttributes visibility = (((int)getterVisibility) > ((int)setterVisibility))
						? getterVisibility
						: setterVisibility;
					if ((visibility & MethodAttributes.Public) == MethodAttributes.Public) {
					}
					else if ((visibility & MethodAttributes.Family) == MethodAttributes.Family) {
						name += "protected";
					}
					else if ((visibility & MethodAttributes.Assem) == MethodAttributes.Assem) {
						name = "sealed";
					}
					else if ((visibility & MethodAttributes.Private) == MethodAttributes.Private) {
						name = "private";
					}
				}
			}
			// TODO: Handle namespaces and assemblies

			return name;
		}
	}
}
