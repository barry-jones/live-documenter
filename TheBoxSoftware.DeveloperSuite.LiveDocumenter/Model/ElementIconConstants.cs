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
    public sealed class ElementIconConstants {
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
		public static string GetIconPathFor(object o) {
			string path = string.Empty;
			if (o is ReflectedMember)
				path = ElementIconConstants.GetIconPathFor((ReflectedMember)o);
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
        private static string GetIconPathFor(ReflectedMember member) {
			string name = string.Empty;

			if (member is AssemblyDef) {
				name = "assembly";
			}
            else if(member is TypeDef) {
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

                    if ((typeDef.Flags & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic) {
                    }
                    else if ((typeDef.Flags & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily) {
                        name += "_protected";
                    }
                    else if ((typeDef.Flags & TypeAttributes.NestedAssembly) == TypeAttributes.NestedAssembly) {
                        name += "_sealed";
                    }
                    else if ((typeDef.Flags & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate) {
                        name += "_private";
                    }
                    else if ((typeDef.Flags & TypeAttributes.Public) == TypeAttributes.Public) {
                    }
                    else if ((typeDef.Flags & TypeAttributes.NotPublic) == TypeAttributes.NotPublic) {
                        name += "_sealed";
                    }
                }
			}
			else if(member is EventDef) {
				name = "event";
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
						name += "_protected";
					}
					else if ((visibility & MethodAttributes.Assem) == MethodAttributes.Assem) {
						name += "_sealed";
					}
					else if ((visibility & MethodAttributes.Private) == MethodAttributes.Private) {
						name += "_private";
					}
				}
			}
			else if(member is FieldDef) {
				name = "field";
				FieldDef fieldDef = (FieldDef)member;
				if (fieldDef.IsConstant) {
					name = "constant";
				}
				else if (fieldDef.IsOperator) {
					name = "operator";
				}

                if ((fieldDef.Flags & FieldAttributes.Public) == FieldAttributes.Public) {
                }
                else if ((fieldDef.Flags & FieldAttributes.Family) == FieldAttributes.Family) {
                    name += "_protected";
                }
                else if ((fieldDef.Flags & FieldAttributes.Assembly) == FieldAttributes.Assembly) {
                    name += "_sealed";
                }
                else if ((fieldDef.Flags & FieldAttributes.Private) == FieldAttributes.Private) {
                    name += "_private";
                }
			}
			else if(member is MethodDef) {
				name = "method";
				MethodDef methodDef = (MethodDef)member;

                if ((methodDef.Attributes & MethodAttributes.Public) == MethodAttributes.Public) {
                    // stay at method
                }
                else if ((methodDef.Attributes & MethodAttributes.Family) == MethodAttributes.Family) {
                    name = "method_protected";
                }
                else if ((methodDef.Attributes & MethodAttributes.Assem) == MethodAttributes.Assem) {
                    name = "method_sealed";
                }
				else if ((methodDef.Attributes & MethodAttributes.Private) == MethodAttributes.Private) {
					name = "method_private";
				}
			}
            else if (member is PropertyDef) {
                name = "properties";
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
                        name += "_protected";
                    }
                    else if ((visibility & MethodAttributes.Assem) == MethodAttributes.Assem) {
                        name += "_sealed";
                    }
                    else if ((visibility & MethodAttributes.Private) == MethodAttributes.Private) {
                        name += "_private";
                    }
                }
            }
			// TODO: Handle namespaces and assemblies

            return string.IsNullOrEmpty(name) ? string.Empty : string.Format(basePath, name);
        }
    }
}
