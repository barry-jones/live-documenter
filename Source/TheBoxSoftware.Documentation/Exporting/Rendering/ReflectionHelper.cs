
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using Reflection;
    using Reflection.Signatures;

    public static class ReflectionHelper
    {
        public static string GetType(ReflectedMember member)
        {
            // NOTE: This code is duplicated in LiveDocumenter.Model.ElementIconConstants.GetIconFor

            string name = string.Empty;

            if (member is AssemblyDef)
            {
                name = "assembly";
            }
            else if (member is TypeDef)
            {
                name = "class";

                TypeDef typeDef = (TypeDef)member;
                if (typeDef != null)
                {

                    if (typeDef.IsInterface)
                    {
                        name = "interface";
                    }
                    else if (typeDef.IsStructure)
                    {
                        name = "structure";
                    }
                    else if (typeDef.IsDelegate)
                    {
                        name = "delegate";
                    }
                    else if (typeDef.IsEnumeration)
                    {
                        name = "enum";
                    }
                }
            }
            else if (member is EventDef)
            {
                name = "event";
            }
            else if (member is FieldDef)
            {
                name = "field";
                FieldDef fieldDef = (FieldDef)member;
                if (fieldDef.IsConstant)
                {
                    name = "constant";
                }
                else if (fieldDef.IsOperator)
                {
                    name = "operator";
                }
            }
            else if (member is MethodDef)
            {
                name = "method";
                MethodDef method = (MethodDef)member;
                if (method.IsOperator)
                {
                    name = "operator";
                }
                else if (method.IsConstructor)
                {
                    name = "constructor";
                }
            }
            else if (member is PropertyDef)
            {
                name = "properties";
            }

            return name;
        }

        /// <summary>
        /// Obtains a name to display for the reflected member.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>A name for the ReflectedMember</returns>
        public static string GetDisplayName(ReflectedMember entry)
        {
            string displayName = string.Empty;

            if (entry is FieldDef)
            {
                displayName = ((FieldDef)entry).Name;
            }
            else if (entry is PropertyDef)
            {
                PropertyDef property = entry as PropertyDef;
                displayName = new DisplayNameSignitureConvertor(property, false, true).Convert();
            }
            else if (entry is MethodDef)
            {
                displayName = ((MethodDef)entry).GetDisplayName(false, true);
            }
            else if (entry is EventDef)
            {
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
        public static string GetVisibility(object o)
        {
            string path = string.Empty;
            if (o is ReflectedMember)
                path = ReflectionHelper.GetVisibility((ReflectedMember)o);
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
        private static string GetVisibility(ReflectedMember member)
        {
            string name = "public";

            if (member is AssemblyDef)
            {
            }
            else if (member is TypeDef || member is EventDef || member is FieldDef || member is MethodDef || member is PropertyDef)
            {
                switch (member.MemberAccess)
                {
                    case Visibility.Protected:
                        name = "protected";
                        break;
                    case Visibility.Internal:
                        name = "sealed";
                        break;
                    case Visibility.InternalProtected:
                        name = "friend";
                        break;
                    case Visibility.Private:
                        name = "private";
                        break;
                    case Visibility.Public:
                        name = "public";
                        break;
                }
            }

            // TODO: Handle namespaces and assemblies

            return name;
        }
    }
}