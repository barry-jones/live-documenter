using System;
using System.Collections.Generic;
using System.Linq;
using TheBoxSoftware.Diagnostics;

namespace TheBoxSoftware.Reflection.Comments
{
    /// <summary>
    /// Class that handles and parses a CRef comment path. A CRef path can contain a fully qualified 
    /// link to a type, property, method etc in an assembly.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("cref={ToString()}")]
    public sealed class CRefPath : Signitures.SignitureConvertor
    {
        private const int SEPERATOR_INDEX = 1;

        private string _crefPath;
        private string _returnType;
        private bool _isOperator = false;

        /// <summary>
        /// Initialises a new instance of the CRefPath class.
        /// </summary>
        public CRefPath() { }

        /// <summary>
        /// Constructs a cref path for the provided <paramref name="field"/>.
        /// </summary>
        /// <param name="field">The field to create the path for.</param>
        public CRefPath(FieldDef field)
            : this(CRefTypes.Field, field.Type, field.Name)
        {
        }

        /// <summary>
        /// Initialises a new instance of the CRefPath class.
        /// </summary>
        /// <param name="type">The TypRef to initialise the path with.</param>
        public CRefPath(TypeRef type)
            : this(CRefTypes.Type, type, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the CRefPath class.
        /// </summary>
        /// <param name="property">The property to initialise the path with.</param>
        public CRefPath(PropertyDef property)
            : this(CRefTypes.Property, property.Type, property.Name)
        {
            MethodDef method = property.GetMethod ?? property.SetMethod;
            this.Parameters = property.IsIndexer ? this.Convert(method) : string.Empty;
        }

        /// <summary>
        /// Initialises a new instance of the CRefPath class.
        /// </summary>
        /// <param name="cEvent">The event to create the path to.</param>
        public CRefPath(EventDef cEvent)
            : this(CRefTypes.Event, cEvent.Type, cEvent.Name)
        {
        }

        /// <summary>
        /// Initialises a new instance of the CRefPath class.
        /// </summary>
        /// <param name="method">The method to initialise the path with.</param>
        public CRefPath(MethodDef method)
            : this(CRefTypes.Method, method.Type, method.Name)
        {
            // We need to adjust the method name if is a constructor
            if(method.IsConstructor)
            {
                this.ElementName = method.Name.Replace('.', '#');
            }
            if(method.IsGeneric)
            {
                this.ElementName += "``" + method.GenericTypes.Count.ToString();
            }
            this.Parameters = this.Convert(method);
            if(method.IsOperator && method.IsConversionOperator)
            {
                _isOperator = true;
                _returnType = method.Signiture.GetReturnTypeToken().ResolveType(method.Assembly, method).GetFullyQualifiedName();
            }
        }

        // TODO: Implement Events, Delegates

        /// <summary>
        /// Private constructor initialises a new instance of the CRefPath class.
        /// </summary>
        /// <param name="crefPathType">The cref path type.</param>
        /// <param name="type">The type information.</param>
        /// <param name="element">The element name for the path.</param>
        private CRefPath(CRefTypes crefPathType, TypeRef type, string element)
        {
            this.PathType = crefPathType;
            this.ElementName = element;

            this.TypeName = type.Name;

            // work out the namespace
            if(type is TypeDef)
            {
                List<string> elements = new List<string>();
                TypeDef container = null;
                if(((TypeDef)type).ContainingClass != null)
                {
                    container = (TypeDef)type;
                    do
                    {
                        container = container.ContainingClass;
                        elements.Add(container.Name);
                    } while(container.ContainingClass != null);
                }
                if(container != null)
                {
                    elements.Add(container.Namespace);
                }
                else
                {
                    elements.Add(type.Namespace);
                }
                elements.Reverse();
                this.Namespace = string.Join(".", elements.ToArray());
            }
            else
            {
                this.Namespace = type.Namespace;
            }
        }

        /// <summary>
        /// Factory method for creating CRefPath entries when the type
        /// is unknown.
        /// </summary>
        /// <param name="member">The member to create a path for</param>
        /// <returns>The CRefPath</returns>
        /// <exception cref="NotImplementedException">
        /// Thrown when the type of the <see cref="ReflectedMember"/> is not
        /// implemented.
        /// </exception>
        public static CRefPath Create(ReflectedMember member)
        {
            CRefPath path = null;

            if(member is FieldDef)
            {
                path = new CRefPath((FieldDef)member);
            }
            else if(member is TypeRef)
            {
                path = new CRefPath((TypeRef)member);
            }
            else if(member is PropertyDef)
            {
                path = new CRefPath((PropertyDef)member);
            }
            else if(member is EventDef)
            {
                path = new CRefPath((EventDef)member);
            }
            else if(member is MethodDef)
            {
                path = new CRefPath((MethodDef)member);
            }
            else
            {
                throw new NotImplementedException($"The '{member.GetType().ToString()}' type is not supported.");
            }

            return path;
        }

        /// <summary>
        /// Parses the provided path and returns a populated cref path instance.
        /// </summary>
        /// <param name="crefPath">The cref path to control.</param>
        /// <exception cref="ArgumentNullException">The specified path was null or empty.</exception>
        public static CRefPath Parse(string crefPath)
        {
            if(string.IsNullOrEmpty(crefPath))
                throw new ArgumentNullException("path");

            CRefPath parsedPath = new CRefPath();
            parsedPath._crefPath = crefPath;
            parsedPath.Parse();

            return parsedPath;
        }

        /// <summary>
        /// Returns a string representation of this CRefPath.
        /// </summary>
        /// <returns>The cref path constructed from the elements.</returns>
        public override string ToString()
        {
            string toString = string.Empty;
            string pathIndicator = CRefConstants.GetIndicatorFor(PathType);

            switch(this.PathType)
            {
                case CRefTypes.Namespace:
                    toString = pathIndicator + ":" + Namespace;
                    break;

                case CRefTypes.Error: break;

                default:
                    string typePortion = pathIndicator + ":" + Namespace + "." + TypeName;
                    if(PathType != CRefTypes.Type)
                    {
                        typePortion += "." + ElementName;
                    }
                    // [#32] - Added code to make cref paths add parameters for
                    //	parameterised properties.
                    if(PathType == CRefTypes.Method || (PathType == CRefTypes.Property && Parameters != null))
                    {
                        typePortion += Parameters;
                    }

                    // Operators provide the return types after a "~" character as specified in:
                    //	http://msdn.microsoft.com/en-us/library/fsbx0t7x(VS.71).aspx
                    if(_isOperator && !string.IsNullOrEmpty(_returnType))
                    {
                        typePortion += "~" + _returnType;
                    }

                    toString = typePortion;
                    break;
            }

            return toString;
        }

        /// <summary>
        /// Finds the member this <see cref="CRefPath"/> refers to in the provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The found member ref.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="type"/> is null.</exception>
        public ReflectedMember FindIn(TypeDef type)
        {
            // TODO: Move to the TypeDef class
            if(type == null) throw new ArgumentNullException("type");

            if(this.PathType == CRefTypes.Namespace || this.PathType == CRefTypes.Type || this.PathType == CRefTypes.Error)
            {
                throw new InvalidOperationException(string.Format("Can not find member in a type when the path type is '{0}'", this.PathType.ToString()));
            }

            ReflectedMember member = null;
            List<ReflectedMember> foundMembers = new List<ReflectedMember>();

            // find all potential members
            switch(this.PathType)
            {
                case CRefTypes.Event:
                    foundMembers.AddRange(type.Events.FindAll(e => string.Compare(e.Name, this.ElementName, true) == 0).ToArray());
                    break;
                case CRefTypes.Field:
                    foundMembers.AddRange(type.Fields.FindAll(e => string.Compare(e.Name, this.ElementName, true) == 0).ToArray());
                    break;
                case CRefTypes.Method:
                    string elementName = this.ElementName.Replace('#', '.');
                    int genParameters = 0;
                    if(elementName.Contains('`'))
                    {
                        genParameters = int.Parse(elementName.Substring(elementName.Length - 1, 1));
                        elementName = elementName.Substring(0, elementName.IndexOf('`'));
                    }
                    MethodDef[] foundMethods = type.Methods.FindAll(e => string.Compare(e.Name, elementName, true) == 0).ToArray();
                    if(foundMethods.Length > 1 && genParameters > 0)
                    {
                        for(int i = 0; i < foundMethods.Length; i++)
                        {
                            if(foundMethods[i].GenericTypes != null && foundMethods[i].GenericTypes.Count == genParameters)
                            {
                                foundMembers.Add(foundMethods[i]);
                            }
                        }
                    }
                    else
                    {
                        foundMembers.AddRange(foundMethods);
                    }
                    break;
                case CRefTypes.Property:
                    foundMembers.AddRange(type.Properties.FindAll(e => string.Compare(e.Name, this.ElementName, true) == 0).ToArray());
                    break;
            }

            if(foundMembers.Count == 1)
            {
                member = foundMembers[0];
            }
            else if(foundMembers.Count > 1)
            {
                // the elements will differ by the parameters, this is slow!
                foreach(ReflectedMember current in foundMembers)
                {
                    string found = CRefPath.Create(current).ToString();
                    if(string.Compare(found, this.ToString(), true) == 0)
                    {
                        member = current;
                        break;
                    }
                }
            }

            return member;
        }

        /// <summary>
        /// Parses the contents of the contained path and stores the details in the
        /// classes properties.
        /// </summary>
        private void Parse()
        {
            ParseType();

            if(PathType != CRefTypes.Error)
            {
                string[] items;
                int startParams = _crefPath.IndexOf('(');
                if(startParams == -1)
                {
                    items = _crefPath.Substring(SEPERATOR_INDEX + 1).Split('.');
                }
                else
                {
                    items = _crefPath.Substring(SEPERATOR_INDEX + 1, _crefPath.IndexOf('(') - 2).Split('.');
                    Parameters = _crefPath.Substring(_crefPath.IndexOf('('));
                }

                switch(PathType)
                {
                    case CRefTypes.Namespace:
                        Namespace = string.Join(".", items);
                        break;
                    case CRefTypes.Type:
                        TypeName = items[items.Length - 1];
                        Namespace = string.Join(".", items, 0, items.Length - 1);
                        break;
                    default:
                        if(items.Length - 2 <= 0)
                        {
                            PathType = CRefTypes.Error;
                        }
                        else
                        {
                            // -2 because the last element is the element name
                            TypeName = items[items.Length - 2];
                            ElementName = items[items.Length - 1];
                            Namespace = string.Join(".", items, 0, items.Length - 2);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Extracts the cref type portion of the string and sets the <see cref="PathType"/>.
        /// </summary>
        private void ParseType()
        {
            PathType = CRefTypes.Error;

            if(string.IsNullOrEmpty(_crefPath) || _crefPath[SEPERATOR_INDEX] != ':')
            {
                return;
            }

            string typePortion = _crefPath[0].ToString(); // we only need to get the first character
            switch(typePortion)
            {
                case CRefConstants.TypeIndicator: PathType = CRefTypes.Type; break;
                case CRefConstants.PropertyTypeIndicator: PathType = CRefTypes.Property; break;
                case CRefConstants.MethodTypeIndicator: PathType = CRefTypes.Method; break;
                case CRefConstants.FieldTypeIndicator: PathType = CRefTypes.Field; break;
                case CRefConstants.ErrorTypeIndicator: PathType = CRefTypes.Error; break;
                case CRefConstants.NamespaceTypeIndicator: PathType = CRefTypes.Namespace; break;
                case CRefConstants.EventTypeIndicator: PathType = CRefTypes.Event; break;
                default:
                    TraceHelper.WriteLine($"Parsing for cref paths of type {typePortion} has not been implemented");
                    break;
            }
        }

        /// <summary>
        /// Indicates the type of element that is referenced by the CRef path.
        /// </summary>
        public CRefTypes PathType { get; set; }
    }
}