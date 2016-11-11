
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;
    using System.Text;

    /// <summary>
    /// A <see cref="SignatureConvertor"/> implementation that creates user
    /// displayable names for types, methods and properties.
    /// </summary>
    public sealed class DisplayNameSignitureConvertor : SignatureConvertor
    {
        private TypeDef _type;
        private MethodDef _method;
        private PropertyDef _property;
        private bool _includeNamespace;
        private bool _includeParameters = false;
        private bool _includeTypeName = true;

        /// <summary>
        /// Initialises a new instance of the DisplayNameSignitureConvertor.
        /// </summary>
        /// <param name="method">The method to obtain a display name for.</param>
        /// <param name="includeNamespace">Should the details of the namespace be included.</param>
        /// <param name="includeParamaters">Should the methods parameters be included.</param>
        public DisplayNameSignitureConvertor(MethodDef method, bool includeNamespace, bool includeParamaters)
            : this()
        {
            _type = (TypeDef)method.Type;
            _method = method;
            _includeNamespace = includeNamespace;
            _includeParameters = includeParamaters;
            _includeTypeName = false;
        }

        /// <summary>
        /// Initialises a new instance of the DisplayNameSignitureConvertor.
        /// </summary>
        /// <param name="method">The method to obtain a display name for.</param>
        /// <param name="includeNamespace">Should the details of the namespace be included.</param>
        /// <param name="includeParameters">Should the methods parameters be included.</param>
        /// <param name="isFromExtendedType">Indicates this is an extension method from the type it is extending.</param>
        public DisplayNameSignitureConvertor(MethodDef method, bool includeNamespace, bool includeParameters, bool isFromExtendedType)
            : this(method, includeNamespace, includeParameters)
        {
            IncludeFirstParameter = false;
        }

        /// <summary>
        /// Initialises a new instance of the DisplayNameSignitureConvertor
        /// </summary>
        /// <param name="property">The property to obtain the display name for.</param>
        /// <param name="includeNamespace">Should the details of the namespace be included?</param>
        /// <param name="includeParamaters">Should the parameters for the property be included?</param>
        public DisplayNameSignitureConvertor(PropertyDef property, bool includeNamespace, bool includeParamaters)
        {
            _type = property.OwningType;
            _property = property;
            _method = property.Getter != null ? property.Getter : property.Setter;
            _includeNamespace = includeNamespace;
            _includeParameters = includeParamaters;
            _includeTypeName = false;
        }

        /// <summary>
        /// Initialises a new instance of the DisplayNameSignitureConvertor.
        /// </summary>
        /// <param name="type">The type being converted.</param>
        /// <param name="includeNamespace">Should the details of the namespace be included in the display name.</param>
        public DisplayNameSignitureConvertor(TypeDef type, bool includeNamespace) : this()
        {
            _type = type;
            _includeNamespace = includeNamespace;
        }

        /// <summary>
        /// Initialises a new instance of the DisplayNameSignitureConvertor.
        /// </summary>
        private DisplayNameSignitureConvertor()
        {
            // Set the generic element tags
            GenericEnd = ">";
            GenericStart = "<";
            ByRef = "ref ";
            ByRefAtFront = true;
            ParameterSeperater = ", ";
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
        public string Convert()
        {
            try
            {
                return ConvertMember();
            }
            catch(Exception ex)
            {
                if(_property != null)
                {
                    throw new ReflectionException(_property, "Error processing display name signiture for a property", ex);
                }
                else if(_method != null)
                {
                    throw new ReflectionException(_method, "Error processing display name signiture for a method", ex);
                }
                else
                {
                    throw new ReflectionException(_type, "Error processing display name signiture for a type", ex);
                }
            }
        }

        /// <summary>
        /// Obtains the type name for the specified <see cref="TypeRef"/>.
        /// </summary>
        /// <param name="sb">The current display name for the signiture to append the details to.</param>
        /// <param name="type">The type to obtain the display name for.</param>
        protected override void GetTypeName(StringBuilder sb, TypeRef type)
        {
            string typeName = type.GetFullyQualifiedName();
            if(!_includeNamespace)
            {
                typeName = type.Name;
            }
            else
            {
                typeName = type.GetFullyQualifiedName();
            }

            if(type.IsGeneric)
            {
                int genIdIndex = typeName.IndexOf('`');
                if(genIdIndex != -1)
                {
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
        protected override void ConvertMVar(StringBuilder sb, uint sequence, ParamDef parameter)
        {
            // Type Generic Parameter
            GenericTypeRef foundGenericType = null;
            foreach(GenericTypeRef current in _method.GenericTypes)
            {
                if(current.Sequence == sequence)
                {
                    foundGenericType = current;
                    break;
                }
            }
            if(foundGenericType != null)
            {
                sb.Append(foundGenericType.Name);
            }
        }

        /// <summary>
        /// Converts a generic variable for display.
        /// </summary>
        /// <param name="sb">The current display name for the signiture to append details to.</param>
        /// <param name="sequence">The sequence number of the current generic variable</param>
        /// <param name="parameter">The parameter definition information.</param>
        protected override void ConvertVar(StringBuilder sb, uint sequence, ParamDef parameter)
        {
            // Type Generic Parameter
            GenericTypeRef foundGenericType = null;
            foreach(GenericTypeRef current in _type.GenericTypes)
            {
                if(current.Sequence == sequence)
                {
                    foundGenericType = current;
                    break;
                }
            }
            if(foundGenericType != null)
            {
                sb.Append(foundGenericType.Name);
            }
        }

        /// <summary>
        /// Overridden convertor for arrays. Converts the <see cref="ArrayShapeSignatureToken"/>
        /// to its correct display name equivelant.
        /// </summary>
        /// <param name="sb">The string being constructed containing the display name.</param>
        /// <param name="resolvedType">The type the parameter has been resolved to</param>
        /// <param name="shape">The signiture token detailing the shape of the array.</param>
        internal override void ConvertArray(StringBuilder sb, TypeRef resolvedType, ArrayShapeSignatureToken shape)
        {
            GetTypeName(sb, resolvedType);
            sb.Append("[");
            for(int i = 0; i < shape.Rank; i++)
            {
                if(i != 0 && i != shape.Rank)
                {
                    sb.Append(",");
                }
                bool hasLoBound = i < shape.LoBounds.Length;
                bool hasSize = i < shape.Sizes.Length;
                if(hasLoBound)
                {
                    sb.Append(shape.LoBounds[i]);
                }
                if(hasSize)
                {
                    sb.Append(shape.Sizes[i]);
                }
            }
            sb.Append("]");
        }

        private string ConvertMember()
        {
            StringBuilder converted = new StringBuilder();

            ConvertAndIncludeTypeName(converted);
            ConvertAndIncludeParameters(converted);

            return converted.ToString();
        }

        private void ConvertAndIncludeParameters(StringBuilder converted)
        {
            if(_method != null)
            {
                if(_method.IsConstructor && _property == null)
                {
                    GetTypeName(converted, _type);
                }
                else if(_method.IsOperator && _property == null)
                {
                    if(_method.IsConversionOperator)
                    {
                        Signature sig = _method.Signiture;
                        TypeRef convertToRef = sig.GetReturnTypeToken().ResolveType(_method.Assembly, _method);
                        TypeRef convertFromRef = sig.GetParameterTokens()[0].ResolveParameter(_method.Assembly, _method.Parameters[0]);

                        converted.Append(_method.Name.Substring(3));
                        converted.Append("(");
                        converted.Append(convertToRef.GetDisplayName(false));
                        converted.Append(" to ");
                        converted.Append(convertFromRef.GetDisplayName(false));
                        converted.Append(")");
                    }
                    else
                    {
                        converted.Append(_method.Name.Substring(3));
                    }
                }
                else if(_property == null)
                {
                    converted.Append(_method.Name);
                }
                else
                {
                    converted.Append(_property.Name);
                }

                if(_method.IsGeneric)
                {
                    converted.Append(GenericStart);
                    bool first = true;
                    foreach(GenericTypeRef type in _method.GetGenericTypes())
                    {
                        if(first)
                        {
                            first = false;
                        }
                        else
                        {
                            converted.Append(", ");
                        }
                        converted.Append(type.Name);
                    }
                    converted.Append(GenericEnd);
                }

                if(ShouldIncludeParameters())
                {
                    string parameters = "()";
                    if(ShouldConvertParameters())
                    {
                        parameters = Convert(_method);
                    }
                    converted.Append(parameters);
                }
            }
        }

        private bool ShouldIncludeParameters()
        {
            return _includeParameters
                && !_method.IsConversionOperator
                && (_property == null || _property.IsIndexer()); // only display parameters for indexer properties
        }

        private bool ShouldConvertParameters()
        {
            return _method.Parameters.Count > 0 && _property == null;
        }

        private void ConvertAndIncludeTypeName(StringBuilder converted)
        {
            if(_includeTypeName)
            {
                GetTypeName(converted, _type);
                if(_type.IsGeneric)
                {
                    converted.Append(GenericStart);
                    bool first = true;
                    foreach(GenericTypeRef type in _type.GetGenericTypes())
                    {
                        if(first)
                        {
                            first = false;
                        }
                        else
                        {
                            converted.Append(", ");
                        }
                        converted.Append(type.Name);
                    }
                    converted.Append(GenericEnd);
                }
            }
        }
    }
}