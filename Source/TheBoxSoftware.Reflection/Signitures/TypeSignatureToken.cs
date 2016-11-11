
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core;

    /// <summary>
    /// <para>Represents an element of a signiture that can be used to resolve back to a type,
    /// however the type it resolves back to can be one of many things; see ECMA 335
    /// 23.2.12.
    /// </para>
    /// <para>
    /// A type can be represented by one or more tokens, hence this token derives from
    /// <see cref="SignatureTokenContainer"/>. However it will always have an <see cref="ElementTypeSignatureToken"/>
    /// which should allow any resolving to be perfomed more easily.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Type: {ElementType}, ")]
    internal sealed class TypeSignatureToken : SignatureTokenContainer
    {
        /// <summary>
        /// Initialises a new TypeSigniture from the <paramref name="signiture"/> starting at the
        /// specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture to parse the type from.</param>
        /// <param name="offset">The offset to start reading from.</param>
        public TypeSignatureToken(byte[] signiture, Offset offset) : base(SignatureTokens.Type)
        {
            ElementTypeSignatureToken type = new ElementTypeSignatureToken(signiture, offset);
            TypeSignatureToken childType;
            Tokens.Add(type);
            ElementType = type;

            switch(type.ElementType)
            {
                case ElementTypes.SZArray:
                    while(CustomModifierToken.IsToken(signiture, offset))
                    {
                        CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                        Tokens.Add(modifier);
                    }
                    childType = new TypeSignatureToken(signiture, offset);
                    Tokens.Add(childType);
                    break;
                case ElementTypes.Ptr:
                    while(CustomModifierToken.IsToken(signiture, offset))
                    {
                        CustomModifierToken modifier = new CustomModifierToken(signiture, offset);
                        Tokens.Add(modifier);
                    }
                    childType = new TypeSignatureToken(signiture, offset);
                    Tokens.Add(childType);
                    break;
                case ElementTypes.GenericInstance:
                    ElementTypeSignatureToken genericType = new ElementTypeSignatureToken(signiture, offset);
                    Tokens.Add(genericType);
                    GenericArgumentCountSignatureToken argCount = new GenericArgumentCountSignatureToken(signiture, offset);
                    Tokens.Add(argCount);
                    for(int i = 0; i < argCount.Count; i++)
                    {
                        TypeSignatureToken genArgType = new TypeSignatureToken(signiture, offset);
                        Tokens.Add(genArgType);
                    }
                    break;
                case ElementTypes.Array:
                    childType = new TypeSignatureToken(signiture, offset);
                    Tokens.Add(childType);
                    Tokens.Add(new ArrayShapeSignatureToken(signiture, offset));
                    break;
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();

            if(ElementType.ElementType == ElementTypes.SZArray)
            {
                TypeSignatureToken childType = Tokens.Last() as TypeSignatureToken;
                details.ArrayOf = childType.GetTypeDetails(member);
                details.IsArray = true;
            }
            else if(
                ElementType.ElementType == ElementTypes.Class ||
                ElementType.ElementType == ElementTypes.ValueType
                )
            {
                details.Type = ElementType.ResolveToken(member.Assembly);
            }
            else if(ElementType.ElementType == ElementTypes.GenericInstance)
            {
                ElementTypeSignatureToken childType = Tokens[1] as ElementTypeSignatureToken;
                details.Type = childType.ResolveToken(member.Assembly);
                details.IsGenericInstance = true;
                details.GenericParameters = new List<TypeDetails>();
                for(int i = 3; i < GetGenericArgumentCount().Count + 3; i++)
                {
                    if(Tokens[i].TokenType == SignatureTokens.Type)
                    {
                        details.GenericParameters.Add(
                            ((TypeSignatureToken)Tokens[i]).GetTypeDetails(member)
                            );
                    }
                    else
                    {
                        TypeDetails genericParameter = new TypeDetails();
                        genericParameter.Type = ((ElementTypeSignatureToken)Tokens[i]).ResolveToken(member.Assembly);
                        details.GenericParameters.Add(genericParameter);
                    }
                }
            }
            else if(ElementType.ElementType == ElementTypes.Ptr)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = Tokens[1] as TypeSignatureToken;
                    details = childType.GetTypeDetails(member);
                }
                else
                {
                    ElementTypeSignatureToken childType = Tokens[1] as ElementTypeSignatureToken;
                    details.Type = childType.ResolveToken(member.Assembly);
                }
                details.IsPointer = true;
            }
            else if(ElementType.ElementType == ElementTypes.Array)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = Tokens[1] as TypeSignatureToken;
                    details.ArrayOf = childType.GetTypeDetails(member);
                }
                else
                {
                    ElementTypeSignatureToken childType = Tokens[1] as ElementTypeSignatureToken;
                    details.ArrayOf = new TypeDetails();
                    details.ArrayOf.Type = childType.ResolveToken(member.Assembly);
                }
                details.IsMultidemensionalArray = true;
                details.ArrayShape = (ArrayShapeSignatureToken)Tokens.Find(t => t.TokenType == SignatureTokens.ArrayShape);
            }
            else if(ElementType.ElementType == ElementTypes.MVar)
            {
                details.Type = ResolveType(member.Assembly, member);
            }
            else if(ElementType.ElementType == ElementTypes.Var)
            {
                details.Type = ResolveType(member.Assembly, member);
            }
            else if(ElementType.Definition != null)
            {
                details.Type = ElementType.Definition as TypeRef;
            }

            return details;
        }

        public GenericArgumentCountSignatureToken GetGenericArgumentCount()
        {
            foreach(SignatureToken current in Tokens)
            {
                if(current.TokenType == SignatureTokens.GenericArgumentCount)
                {
                    return current as GenericArgumentCountSignatureToken;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempts to resolve the type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        internal TypeRef ResolveType(AssemblyDef assembly, ReflectedMember member)
        {
            TypeRef type = null;
            if(ElementType.ElementType == ElementTypes.SZArray)
            {
                TypeSignatureToken childType = (TypeSignatureToken)Tokens.Last();
                type = childType.ResolveType(assembly, member);
            }
            else if(
                ElementType.ElementType == ElementTypes.Class ||
                ElementType.ElementType == ElementTypes.ValueType
                )
            {
                type = (TypeRef)assembly.ResolveMetadataToken(ElementType.Token);
            }
            else if(ElementType.ElementType == ElementTypes.GenericInstance)
            {
                ElementTypeSignatureToken childType = (ElementTypeSignatureToken)Tokens[1];
                type = childType.ResolveToken(assembly);
            }
            else if(ElementType.ElementType == ElementTypes.Ptr)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = (TypeSignatureToken)Tokens[1];
                    type = childType.ResolveType(assembly, member);
                }
                else
                {
                    ElementTypeSignatureToken childType = (ElementTypeSignatureToken)Tokens[1];
                    type = childType.ResolveToken(assembly);
                }
            }
            else if(ElementType.ElementType == ElementTypes.Array)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = (TypeSignatureToken)Tokens[1];
                    type = childType.ResolveType(assembly, member);
                }
                else
                {
                    ElementTypeSignatureToken childType = (ElementTypeSignatureToken)Tokens[1];
                    type = childType.ResolveToken(assembly);
                }
            }
            else if(ElementType.ElementType == ElementTypes.MVar)
            {
                MethodDef method = member as MethodDef;
                if(method == null)
                {
                    InvalidOperationException ex = new InvalidOperationException(
                        string.Format(
                            "A MethodDef was expected for type resolution in the signiture but a {0} was provided.",
                            member.GetType().ToString()
                        ));
                    throw ex;
                }
                type = method.GenericTypes[(int)ElementType.Token];
            }
            else if(ElementType.ElementType == ElementTypes.Var)
            {
                // Anything that contains a Type property will do here
                TypeDef theType;
                if(member is MethodDef)
                {
                    // a method may define its own parameters
                    theType = (TypeDef)((MethodDef)member).Type;
                }
                else if(member is PropertyDef)
                {
                    theType = ((PropertyDef)member).OwningType;
                }
                else if(member is FieldDef)
                {
                    theType = (TypeDef)((FieldDef)member).Type;
                }
                else if(member is EventDef)
                {
                    theType = ((EventDef)member).Type;
                }
                else if(member is TypeSpec)
                {
                    theType = ((TypeSpec)member).ImplementingType;
                }
                else
                {
                    InvalidOperationException ex = new InvalidOperationException(
                        string.Format(
                            "A ReflectedMember that is contained by a TypeDef was expected for type resolution in the signiture but a {0} was provided.",
                            member.GetType().ToString()
                        ));
                    throw ex;
                }

                List<GenericTypeRef> genericParameters = theType.GenericTypes;

                if(genericParameters.Count < ElementType.Token)
                {
                    throw new InvalidOperationException("The generic token refers to a parameter that is not available.");
                }
                type = genericParameters[(int)ElementType.Token];
            }
            else if(ElementType.Definition != null)
            {
                type = (TypeRef)ElementType.Definition;
            }
            return type;
        }

        /// <summary>
        /// Attempts to resolve the type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal TypeRef ResolveType(AssemblyDef assembly, ParamDef parameter)
        {
            TypeRef type = null;
            if(ElementType.ElementType == ElementTypes.SZArray)
            {
                TypeSignatureToken childType = Tokens.Last() as TypeSignatureToken;
                type = childType.ResolveType(assembly, parameter);
            }
            else if(
                ElementType.ElementType == ElementTypes.Class ||
                ElementType.ElementType == ElementTypes.ValueType
                )
            {
                type = (TypeRef)assembly.ResolveMetadataToken(ElementType.Token);
            }
            else if(ElementType.ElementType == ElementTypes.GenericInstance)
            {
                ElementTypeSignatureToken childType = Tokens[1] as ElementTypeSignatureToken;
                type = childType.ResolveToken(assembly);
            }
            else if(ElementType.ElementType == ElementTypes.Ptr)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = Tokens[1] as TypeSignatureToken;
                    type = childType.ResolveType(assembly, parameter);
                }
                else
                {
                    ElementTypeSignatureToken childType = Tokens[1] as ElementTypeSignatureToken;
                    type = childType.ResolveToken(assembly);
                }
            }
            else if(ElementType.ElementType == ElementTypes.Array)
            {
                if(Tokens[1].TokenType == SignatureTokens.Type)
                {
                    TypeSignatureToken childType = Tokens[1] as TypeSignatureToken;
                    type = childType.ResolveType(assembly, parameter);
                }
                else
                {
                    ElementTypeSignatureToken childType = (ElementTypeSignatureToken)this.Tokens[1];
                    type = childType.ResolveToken(assembly);
                }
            }
            else if(ElementType.ElementType == ElementTypes.MVar)
            {
                type = parameter.Method.GenericTypes[(int)ElementType.Token];
            }
            else if(ElementType.ElementType == ElementTypes.Var)
            {
                List<GenericTypeRef> genericParameters = ((TypeDef)parameter.Method.Type).GenericTypes;
                if(genericParameters.Count < ElementType.Token)
                {
                    throw new InvalidOperationException("The generic token refers to a parameter that is not available.");
                }
                type = genericParameters[(int)ElementType.Token];
            }
            else if(ElementType.Definition != null)
            {
                type = ElementType.Definition as TypeRef;
            }
            return type;
        }

        /// <summary>
        /// Produces a string representation of the Type token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[Type: ");
            foreach(SignatureToken current in Tokens)
                sb.Append(current.ToString());
            sb.Append("] ");

            return sb.ToString();
        }

        /// <summary>
        /// The ElementType details for this TypeSignitureToken
        /// </summary>
        public ElementTypeSignatureToken ElementType { get; set; }
    }
}
