
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System;
    using System.Collections.Generic;
    using Signatures;

    /// <summary>
    /// Base class with methods that are useful for all VB formatting
    /// classes.
    /// </summary>
    internal class VBFormatter
    {
        /// <summary>
        /// Collection of all the types defined in this language with a language
        /// specific short form.
        /// </summary>
        private Dictionary<string, SyntaxToken> defaultTypes = new Dictionary<string, SyntaxToken>() {
            {"System.Object", Constants.TypeObject},
            {"System.Boolean", Constants.TypeBoolean},
            {"System.SByte", Constants.TypeSByte},
            {"System.Byte", Constants.TypeByte},
            {"System.Char", Constants.TypeChar},
            {"System.Double", Constants.TypeDouble},
            {"System.Int16", Constants.TypeShort},
            {"System.Int32", Constants.TypeInt},
            {"System.Int64", Constants.TypeLong},
            {"System.Single", Constants.TypeFloat},
            {"System.String", Constants.TypeString},
            {"System.UInt16", Constants.TypeUShort},
            {"System.UInt32", Constants.TypeUInt},
            {"System.UInt64", Constants.TypeULong},
            {"System.Void", Constants.TypeVoid}
            };

        protected List<SyntaxToken> FormatVisibility(Visibility visibility)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            switch(visibility)
            {
                case Visibility.Internal:
                    tokens.Add(Constants.KeywordInternal);
                    break;
                case Visibility.InternalProtected:
                    tokens.Add(Constants.KeywordInternal);
                    tokens.Add(Constants.Space);
                    tokens.Add(Constants.KeywordProtected);
                    break;
                case Visibility.Protected:
                    tokens.Add(Constants.KeywordProtected);
                    break;
                case Visibility.Private:
                    tokens.Add(Constants.KeywordPrivate);
                    break;
                case Visibility.Public:
                    tokens.Add(Constants.KeywordPublic);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return tokens;
        }

        protected SyntaxToken FormatInheritance(Inheritance inheritance)
        {
            switch(inheritance)
            {
                case Inheritance.Abstract: return Constants.KeywordAbstract;
                case Inheritance.Sealed: return Constants.KeywordSealed;
                case Inheritance.Static: return Constants.KeywordStatic;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns the language specific type name for the specified <paramref name="type"/>. For
        /// example in C# System.Boolean is referenced via the bool keyword.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <returns>The SyntaxToken reprsenting the name</returns>
        protected SyntaxToken FormatTypeName(TypeRef type)
        {
            string name = type.GetFullyQualifiedName();
            if(this.defaultTypes.ContainsKey(name))
            {
                return defaultTypes[name];
            }
            else
            {
                string typeName = type.Name;
                if(type.IsGeneric)
                {
                    int count = int.Parse(typeName.Substring(typeName.IndexOf('`') + 1));
                    typeName = typeName.Substring(0, typeName.IndexOf('`'));
                }
                return new SyntaxToken(typeName, SyntaxTokens.Text);
            }
        }

        protected List<SyntaxToken> FormatTypeName(TypeRef[] types)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            for(int i = 0; i < types.Length; i++)
            {
                tokens.Add(this.FormatTypeName(types[i]));
            }
            return tokens;
        }

        protected List<SyntaxToken> FormatTypeDetails(TypeDetails details)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            // If the type is an array the type ref details are no longer valid
            if(details.IsArray || details.IsMultidemensionalArray)
            {
                if(details.IsArray)
                {
                    tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
                    tokens.Add(Constants.ArrayEmpty);
                }
                if(details.IsMultidemensionalArray)
                {
                    tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
                    tokens.Add(Constants.ArrayStart);
                    tokens.Add(new SyntaxToken(new String(',', (int)details.ArrayShape.Rank - 1), SyntaxTokens.Text));
                    tokens.Add(Constants.ArrayEnd);
                }
            }
            else
            {
                tokens.Add(this.FormatTypeName(details.Type));
                if(details.IsGenericInstance)
                {
                    tokens.Add(Constants.GenericStart);
                    tokens.Add(Constants.Space);
                    for(int i = 0; i < details.GenericParameters.Count; i++)
                    {
                        if(i != 0)
                        {
                            tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                        }
                        tokens.AddRange(this.FormatTypeDetails(details.GenericParameters[i]));
                    }
                    tokens.Add(Constants.GenericEnd);
                }
            }

            // Pointers are not supported in visual basic .net. This would mean that
            // this element would not be available to a vb.net application. For now
            // we will just not show it.
            //if (details.IsPointer) {
            //    tokens.Add(new SyntaxToken("*", SyntaxTokens.Text));
            //}
            return tokens;
        }

        protected List<SyntaxToken> FormatParameterModifiers(ParameterDetails details)
        {
            ParamDef parameterDefinition = details.Parameter;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            if(details.TypeDetails.IsByRef)
            {
                tokens.Add(new SyntaxToken("ByRef", SyntaxTokens.Keyword));
                tokens.Add(Constants.Space);
            }

            return tokens;
        }

        protected bool IsMethodFunction(TypeDetails details)
        {
            return details.IsArray || details.Type.GetFullyQualifiedName() != "System.Void";
        }

        /// <summary>
        /// Formats the generic types for a the specified <paramref name="genericTypes"/>.
        /// </summary>
        /// <param name="genericTypes">The types to format.</param>
        /// <returns>The tokens for the generic types.</returns>
        protected List<SyntaxToken> FormatGenericParameters(List<GenericTypeRef> genericTypes)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            tokens.Add(Constants.GenericStart);
            tokens.Add(Constants.Space);

            for(int i = 0; i < genericTypes.Count; i++)
            {
                if(i != 0)
                {
                    tokens.Add(new SyntaxToken(",", SyntaxTokens.Text));
                }
                tokens.Add(FormatTypeName(genericTypes[i]));
            }

            tokens.Add(Constants.GenericEnd);
            return tokens;
        }
    }
}