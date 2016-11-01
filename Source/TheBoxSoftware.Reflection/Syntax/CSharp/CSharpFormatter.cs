
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System;
    using System.Collections.Generic;
    using Signitures;

    /// <summary>
    /// Base class with methods that are useful for all C# formatting
    /// classes.
    /// </summary>
    internal abstract class CSharpFormatter
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

        /// <summary>
        /// Foramts the visibility modifier.
        /// </summary>
        /// <param name="visibility">The visibility to format.</param>
        /// <returns>A formatted string representing the syntaxs.</returns>
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

        /// <summary>
        /// Formats the inheritance modifier for C#.
        /// </summary>
        /// <param name="inheritance">The modifier to format.</param>
        /// <returns>The formatted syntax token.</returns>
        public SyntaxToken FormatInheritance(Inheritance inheritance)
        {
            switch(inheritance)
            {
                case Inheritance.Abstract:
                    return Constants.KeywordAbstract;
                case Inheritance.Sealed:
                    return Constants.KeywordSealed;
                case Inheritance.Static:
                    return Constants.KeywordStatic;
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
            if(defaultTypes.ContainsKey(name))
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
                tokens.Add(FormatTypeName(types[i]));
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
                    tokens.AddRange(FormatTypeDetails(details.ArrayOf));
                    tokens.Add(Constants.ArrayEmpty);
                }
                if(details.IsMultidemensionalArray)
                {
                    tokens.AddRange(this.FormatTypeDetails(details.ArrayOf));
                    tokens.Add(Constants.ArrayStart);
                    tokens.Add(new SyntaxToken(new String(',', details.ArrayShape.Rank - 1), SyntaxTokens.Text));
                    tokens.Add(Constants.ArrayEnd);
                }
            }
            else
            {
                tokens.Add(FormatTypeName(details.Type));
                if(details.IsGenericInstance)
                {
                    tokens.Add(Constants.GenericStart);
                    for(int i = 0; i < details.GenericParameters.Count; i++)
                    {
                        if(i != 0)
                        {
                            tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                        }
                        tokens.AddRange(FormatTypeDetails(details.GenericParameters[i]));
                    }
                    tokens.Add(Constants.GenericEnd);
                }
            }

            if(details.IsPointer)
            {
                tokens.Add(Constants.KeywordPointer);
            }
            return tokens;
        }

        protected List<SyntaxToken> FormatParameterModifiers(ParameterDetails details)
        {
            ParamDef parameterDefinition = details.Parameter;
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            if(parameterDefinition.IsOut)
            {
                tokens.Add(new SyntaxToken("out ", SyntaxTokens.Keyword));
            }
            else if(details.TypeDetails.IsByRef)
            {
                tokens.Add(new SyntaxToken("ref ", SyntaxTokens.Keyword));
            }

            return tokens;
        }

        protected List<SyntaxToken> FormatGenericParameters(List<GenericTypeRef> genericTypes)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            tokens.Add(Constants.GenericStart);

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