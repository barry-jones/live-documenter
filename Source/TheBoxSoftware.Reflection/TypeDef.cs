
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Core.COFF;

    /// <summary>
    /// Contains the information regarding the construction and elements of a type reflected 
    /// from the metadata information. A type definition is a metadata reflected type which 
    /// is defined in an assembly.
    /// </summary>
    [DebuggerDisplay("Type={ToString()}")]
    public class TypeDef : TypeRef
    {
        private CodedIndex _extends;
        private MetadataTables _table;
        private int _index;
        private List<MethodDef> _methods;
        private List<FieldDef> _fields;
        private List<EventDef> _events;
        private List<PropertyDef> _properties;
        private TypeDef _containingClass;
        private List<TypeRef> _implements;
        private TypeAttributes _flags;
        private List<GenericTypeRef> _genericTypes;

        public TypeDef()
        {
            Methods = new List<MethodDef>();
            Events = new List<EventDef>();
            GenericTypes = new List<GenericTypeRef>();
            Implements = new List<TypeRef>();
            ExtensionMethods = new List<MethodDef>();
            Attributes = new List<CustomAttribute>();
            Fields = new List<FieldDef>();
            Properties = new List<PropertyDef>();
        }

        /// <summary>
        /// Obtains all of the <see cref="TypeRef"/>s that extend this TypeDef.
        /// </summary>
        /// <returns>A collection of derived types.</returns>
        public List<TypeRef> GetExtendingTypes()
        {
            CodedIndex ciForThisType = new CodedIndex(_table, (uint)_index);

            return Assembly.GetExtendindTypes(this, ciForThisType);
        }

        /// <summary>
        /// Obtains the list of generic types that are defined and owned only by this member.
        /// </summary>
        /// <returns>A collection of generic types for this member</returns>
        public List<GenericTypeRef> GetGenericTypes()
        {
            List<GenericTypeRef> parameters = new List<GenericTypeRef>();
            string generic = Name.Substring(
                Name.Length - 1,
                1);
            int numberOfParams = 0;

            if(int.TryParse(generic, out numberOfParams))
            {
                int index = GenericTypes.Count - numberOfParams;
                for(int i = index; i < GenericTypes.Count; i++)
                {
                    parameters.Add(GenericTypes[i]);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Obtains the fields that are defined in this type. System generated fields will not be
        /// returned.
        /// </summary>
        /// <returns>The fields in the type.</returns>
        public List<FieldDef> GetFields()
        {
            return GetFields(false);
        }

        /// <summary>
        /// Obtains the fields that are defined in this TypeDef.
        /// </summary>
        /// <param name="includeSystemGenerated">Indicates if system generated fields should be returned.</param>
        /// <returns>The fields in the type.</returns>
        public List<FieldDef> GetFields(bool includeSystemGenerated)
        {
            List<FieldDef> fields = new List<FieldDef>();
            for(int i = 0; i < Fields.Count; i++)
            {
                FieldDef currentField = Fields[i];
                if(includeSystemGenerated || (!includeSystemGenerated && !currentField.IsSystemGenerated))
                {
                    fields.Add(currentField);
                }
            }
            return fields;
        }

        /// <summary>
        /// Obtains only the methods, not property accessors or operand overload methods for this type
        /// </summary>
        /// <returns>A collection of MethodDefs representing the methods for this type</returns>
        public List<MethodDef> GetMethods()
        {
            return GetMethods(false);
        }

        /// <summary>
        /// Obtains the methods defined in this type.
        /// </summary>
        /// <param name="includeSystemGenerated">Indicates if system generated methods should be returned.</param>
        /// <returns>The methods defined in this type.</returns>
        public List<MethodDef> GetMethods(bool includeSystemGenerated)
        {
            List<MethodDef> methods = new List<MethodDef>();
            for(int i = 0; i < Methods.Count; i++)
            {
                // IsSpecialName denotes (or appears to) that the method is a compiler
                // generated get|set for properties. Compiler generated code for linq
                // expressions are not 'special name' so need to be checked for seperately.
                if(!this.Methods[i].IsSpecialName)
                {
                    if(includeSystemGenerated || (!includeSystemGenerated && !Methods[i].IsCompilerGenerated))
                    {
                        methods.Add(Methods[i]);
                    }
                }
            }
            return methods;
        }

        /// <summary>
        /// Returns a collection of constructor methods defined for this type.
        /// </summary>
        /// <returns>The collection of constructors.</returns>
        public List<MethodDef> GetConstructors()
        {
            return GetConstructors(false);
        }

        /// <summary>
        /// Returns a collection of constructors defined for this type.
        /// </summary>
        /// <param name="includeSystemGenerated">Indicates if system generated methods should be included.</param>
        /// <returns>The collection of constructors.</returns>
        public List<MethodDef> GetConstructors(bool includeSystemGenerated)
        {
            List<MethodDef> methods = new List<MethodDef>();
            for(int i = 0; i < Methods.Count; i++)
            {
                if(Methods[i].IsConstructor && (includeSystemGenerated || (!includeSystemGenerated && !Methods[i].IsCompilerGenerated)))
                {
                    methods.Add(Methods[i]);
                }
            }
            return methods;
        }

        /// <summary>
        /// Returns a collection of operator methods defined for this type.
        /// </summary>
        /// <returns>A collection of zero or more operators defined in this TypeDef.</returns>
        public List<MethodDef> GetOperators()
        {
            return GetOperators(false);
        }

        /// <summary>
        /// Returns a collection of operator methods defined for this type.
        /// </summary>
        /// <param name="includeSystemGenerated">Indicates if system generated operators should be included.</param>
        /// <returns>A collection of zero or more operators defined in this TypeDef.</returns>
        public List<MethodDef> GetOperators(bool includeSystemGenerated)
        {
            List<MethodDef> methods = new List<MethodDef>();
            for(int i = 0; i < Methods.Count; i++)
            {
                if(Methods[i].IsOperator && (includeSystemGenerated || (!includeSystemGenerated && !Methods[i].IsCompilerGenerated)))
                {
                    methods.Add(Methods[i]);
                }
            }
            return methods;
        }

        /// <summary>
        /// Returns a collection of properties that have been defined for this type. Properties
        /// are stored as a series of MethodDef structures, representing the get_ and set_
        /// methods. The PropertyDef wraps these methods up and provides a nice wrapper to
        /// retrieve the names and other details of the property.
        /// </summary>
        /// <returns>A collection of properties defined for the type.</returns>
        public List<PropertyDef> GetProperties()
        {
            return Properties;
        }

        /// <summary>
        /// Obtains a collection of events that are defined in this type.
        /// </summary>
        /// <returns>The collection of events.</returns>
        public List<EventDef> GetEvents()
        {
            return Events;
        }

        internal static TypeDef CreateFromMetadata(BuildReferences references, TypeDefMetadataTableRow fromRow)
        {
            TypeDefBuilder builder = new TypeDefBuilder(references, fromRow);
            return builder.Build();
        }

        /// <summary>
        /// The methods this type contains
        /// </summary>
        public List<MethodDef> Methods
        {
            get { return _methods; }
            set { _methods = value; }
        }

        /// <summary>
        /// The fields this type contains
        /// </summary>
        public List<FieldDef> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// The events this type contains.
        /// </summary>
        public List<EventDef> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        /// <summary>
        /// The properties this type contains.
        /// </summary>
        public List<PropertyDef> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        /// <summary>
        /// When this class is a nested class this property will contain the class which
        /// owns this class.
        /// </summary>
        public TypeDef ContainingClass
        {
            get { return _containingClass; }
            set { _containingClass = value; }
        }

        /// <summary>
        /// Collection of <see cref="TypeRef"/> instances defining the interfaces this class implements.
        /// </summary>
        public List<TypeRef> Implements
        {
            get { return _implements; }
            set { _implements = value; }
        }

        /// <summary>
        /// Flags defining extra information about the type.
        /// </summary>
        public TypeAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// Collection of all the generic types that are relevant for this member, this
        /// includes the types defined in parent and containing classes.
        /// </summary>
        /// <seealso cref="GetGenericTypes"/>
        public List<GenericTypeRef> GenericTypes
        {
            get { return _genericTypes; }
            set { _genericTypes = value; }
        }

        /// <summary>
        /// Indicates if this class is an Interface.
        /// </summary>
        public bool IsInterface
        {
            get { return (_flags & TypeAttributes.ClassSemanticMask) == TypeAttributes.Interface; }
        }

        /// <summary>
        /// Returns a reference to the TypeDef or Ref which this type
        /// inherits from.
        /// </summary>
        public TypeRef InheritsFrom
        {
            get
            {
                try
                {
                    TypeRef inheritsFrom = null;

                    if(_extends.Index != 0)
                    {
                        inheritsFrom = Assembly.ResolveCodedIndex(_extends) as TypeRef;

                        // We have to handle type spec based classes, as if they use the parent generic types
                        // we need the type to have a reference to its container.
                        if(inheritsFrom is TypeSpec)
                        {
                            ((TypeSpec)inheritsFrom).ImplementingType = this;
                        }
                    }

                    return inheritsFrom;
                }
                catch(Exception ex)
                {
                    throw new ReflectionException(this, "Error caused determining the parent type.", ex);
                }
            }
        }

        /// <summary>
        /// Indicates if this class is an enumeration.
        /// </summary>
        public bool IsEnumeration
        {
            get { return InheritsFrom != null && InheritsFrom.GetFullyQualifiedName() == "System.Enum"; }
        }

        /// <summary>
        /// Indicates if this class is a delegate
        /// </summary>
        public bool IsDelegate
        {
            get
            {
                TypeRef parent = InheritsFrom;
                if(parent != null)
                {
                    return (parent.Name == "MulticastDelegate" || (parent.Name == "Delegate" && Name != "MulticastDelegate"));
                }
                else return false;
            }
        }

        /// <summary>
        /// Indicates if this type has any members defined
        /// </summary>
        public bool HasMembers
        {
            get
            {
                return Methods.Count > 0 || Fields.Count > 0;
            }
        }

        /// <summary>
        /// Returns the namespace for the current class. This is obtained from the
        /// Types metadata.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When the class <see cref="IsNested"/> the namespace returned is the namespace
        /// of the enclosing type. Where the class is nested multiple times each nested class
        /// is checked until one its containers defines a namespace and that is returned.
        /// </para>
        /// </remarks>
        public override string Namespace
        {
            get
            {
                if(IsNested)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ContainingClass.Namespace);

                    List<string> containingClassNames = new List<string>();
                    TypeDef currentType = this;
                    if(currentType.IsNested)
                    {
                        currentType = currentType.ContainingClass;
                        if(currentType.IsGeneric)
                        {
                            containingClassNames.Add(currentType.GetDisplayName(false));
                        }
                        else
                        {
                            containingClassNames.Add(currentType.Name);
                        }
                    }

                    for(int i = containingClassNames.Count - 1; i >= 0; i--)
                    {
                        sb.Append(".");
                        sb.Append(containingClassNames[i]);
                    }

                    return sb.ToString();
                }
                return base.Namespace;
            }
            set
            {
                base.Namespace = value;
            }
        }

        /// <summary>
        /// Indicates if this class is a nested class, if so the <see cref="ContainingClass"/> property
        /// details its container.
        /// </summary>
        public bool IsNested
        {
            get { return ContainingClass != null; }
        }

        /// <summary>
        /// Indicates if this TypeDef is a structure
        /// </summary>
        public bool IsStructure
        {
            get { return InheritsFrom != null && InheritsFrom.GetFullyQualifiedName() == "System.ValueType"; }
        }

        /// <summary>
        /// Returns the <see cref="Visibility"/> of the TypeDef.
        /// </summary>
        public override Visibility MemberAccess
        {
            get
            {
                switch(Flags & TypeAttributes.VisibilityMask)
                {
                    case TypeAttributes.NestedPublic:
                    case TypeAttributes.Public:
                        return Visibility.Public;
                    case TypeAttributes.NotPublic:
                        return Visibility.Internal;
                    case TypeAttributes.NestedFamAndAssem:
                        return Visibility.Internal;
                    case TypeAttributes.NestedFamily:
                        return Visibility.Protected;
                    case TypeAttributes.NestedPrivate:
                        return Visibility.Private;
                    case TypeAttributes.NestedFamOrAssem:
                        return Visibility.InternalProtected;
                    default:
                        return Visibility.Internal;
                };
            }
        }

        /// <summary>
        /// Indicates if this type is compiler generated.
        /// </summary>
        public bool IsCompilerGenerated
        {
            get
            {
                // a type is generated if it is a child of generated type.
                bool parentGenerated = this.ContainingClass != null ? this.ContainingClass.IsCompilerGenerated : false;
                return parentGenerated ||
                    this.Namespace == "XamlGeneratedNamespace" ||
                    this.Attributes.Find(attribute => attribute.Name == "CompilerGeneratedAttribute") != null;
            }
        }

        /// <summary>
        /// Internal comparer to enable fast binary searching of the event table.
        /// </summary>
        private class EventMapComparer : IComparer<MetadataRow>
        {
            public int Compare(MetadataRow x, MetadataRow y)
            {
                return this.Compare((EventMapMetadataTableRow)x, (EventMapMetadataTableRow)y);
            }
            public int Compare(EventMapMetadataTableRow x, EventMapMetadataTableRow y)
            {
                if(x.Parent < y.Parent) return -1;
                if(x.Parent == y.Parent) return 0;
                return 1;
            }
        }

        /// <summary>
        /// Internal comparer to enable fast binary searching of the property table.
        /// </summary>
        private class PropertyMapComparer : IComparer<MetadataRow>
        {
            public int Compare(MetadataRow x, MetadataRow y)
            {
                return this.Compare((PropertyMapMetadataTableRow)x, (PropertyMapMetadataTableRow)y);
            }

            public int Compare(PropertyMapMetadataTableRow x, PropertyMapMetadataTableRow y)
            {
                if(x.Parent < y.Parent) return -1;
                if(x.Parent == y.Parent) return 0;
                return 1;
            }
        }

        private class TypeDefBuilder
        {
            private BuildReferences _references;
            private TypeDef _builtType;
            private int _indexInMetadataTable;
            private int _rowIndex;
            private int _nextRowIndex;
            private MetadataStream _metadataStream;

            // would be great to remove these and only use _references above
            private AssemblyDef _assembly;
            private TypeDefMetadataTableRow _fromRow;
            private MetadataDirectory _metadata;
            private MetadataToDefinitionMap _map;

            public TypeDefBuilder(BuildReferences references, TypeDefMetadataTableRow fromRow)
            {
                _references = references;

                _assembly = references.Assembly;
                _fromRow = fromRow;
                _metadata = references.Metadata;
                _map = references.Map;
                _metadataStream = _metadata.GetMetadataStream();
            }

            public TypeDef Build()
            {
                _builtType = new TypeDef();

                SetTypeProperties();
                CalculateIndexes();
                LoadGenericParameters();
                LoadMethods();
                LoadProperties();
                LoadEvents();
                LoadFields();

                return _builtType;
            }

            private void LoadFields()
            {
                if(!_metadataStream.Tables.ContainsKey(MetadataTables.Field))
                    return;

                MetadataRow[] table = _metadataStream.Tables[MetadataTables.Field];
                int endOfFieldIndex = table.Length + 1;
                if(_nextRowIndex != -1)
                {
                    endOfFieldIndex = ((TypeDefMetadataTableRow)_metadataStream.Tables[MetadataTables.TypeDef][_nextRowIndex]).FieldList;
                }

                // Now load all the fields between our index and the endOfFieldIndex				
                for(int i = _fromRow.FieldList; i < endOfFieldIndex; i++)
                {
                    FieldMetadataTableRow fieldDefRow = table[i - 1] as FieldMetadataTableRow;

                    FieldDef field = new FieldDef(
                        _references.Assembly.StringStream.GetString(fieldDefRow.Name.Value),
                        _references.Assembly,
                        _builtType,
                        fieldDefRow.Flags,
                        fieldDefRow.Signiture
                        );

                    _map.Add(MetadataTables.Field, fieldDefRow, field);
                    _builtType.Fields.Add(field);
                }
            }

            private void LoadMethods()
            {
                if(!_metadataStream.Tables.ContainsKey(MetadataTables.MethodDef))
                    return;

                MetadataRow[] table = _metadataStream.Tables[MetadataTables.MethodDef];
                int endOfMethodIndex = table.Length + 1;
                if(_nextRowIndex != -1)
                {
                    endOfMethodIndex = ((TypeDefMetadataTableRow)_metadataStream.Tables[MetadataTables.TypeDef][_nextRowIndex]).MethodList;
                }

                // Now load all the methods between our index and the endOfMethodIndex
                for(int i = _fromRow.MethodList; i < endOfMethodIndex; i++)
                {
                    MethodMetadataTableRow methodDefRow = table[i - 1] as MethodMetadataTableRow;
                    MethodDef method = MethodDef.CreateFromMetadata(_references, _builtType, methodDefRow);

                    _map.Add(MetadataTables.MethodDef, methodDefRow, method);
                    _builtType.Methods.Add(method);
                }
            }

            private void LoadGenericParameters()
            {
                var genericParameters = _metadataStream.Tables.GetGenericParametersFor(MetadataTables.TypeDef, _rowIndex + 1);

                if(genericParameters.Count > 0)
                {
                    foreach(GenericParamMetadataTableRow genParam in genericParameters)
                    {
                        _builtType.GenericTypes.Add(
                            GenericTypeRef.CreateFromMetadata(_references, genParam)
                            );
                    }
                }
            }

            private void CalculateIndexes()
            {
                _indexInMetadataTable = _metadataStream.Tables.GetIndexFor(MetadataTables.TypeDef, _fromRow) + 1;

                // calculate the first and last infex of methods for this type, -1 means end of stream
                _rowIndex = _metadataStream.Tables.GetIndexFor(MetadataTables.TypeDef, _fromRow);
                _nextRowIndex = _rowIndex < _metadataStream.Tables[MetadataTables.TypeDef].Length - 1
                    ? _rowIndex + 1
                    : -1;
            }

            private void SetTypeProperties()
            {
                int index = _metadataStream.Tables.GetIndexFor(MetadataTables.TypeDef, _fromRow);

                _builtType._index = index + 1;
                _builtType._table = MetadataTables.TypeDef;

                _builtType.UniqueId = _assembly.CreateUniqueId();
                _builtType.Name = _assembly.StringStream.GetString(_fromRow.Name.Value);
                _builtType.Namespace = _assembly.StringStream.GetString(_fromRow.Namespace.Value);
                _builtType.Assembly = _assembly;
                _builtType._extends = _fromRow.Extends;
                _builtType.Flags = _fromRow.Flags;
                _builtType.IsGeneric = _builtType.Name.Contains("`"); // Should be quicker then checking the genparam table
            }

            private void LoadEvents()
            {
                // Check if we have a property map and then find the property map for the current type
                // if it exists.
                if(!_metadataStream.Tables.ContainsKey(MetadataTables.EventMap))
                    return;

                int startEventList = -1;
                int endEventList = -1;

                EventMapMetadataTableRow searchFor = new EventMapMetadataTableRow();
                searchFor.Parent = _indexInMetadataTable;

                int mapIndex = Array.BinarySearch(_metadataStream.Tables[MetadataTables.EventMap],
                    searchFor,
                    new EventMapComparer()
                    );
                if(mapIndex >= 0)
                {
                    startEventList = ((EventMapMetadataTableRow)_metadataStream.Tables[MetadataTables.EventMap][mapIndex]).EventList;
                    if(mapIndex < _metadataStream.Tables[MetadataTables.EventMap].Length - 1)
                    {
                        endEventList = ((EventMapMetadataTableRow)_metadataStream.Tables[MetadataTables.EventMap][mapIndex + 1]).EventList - 1;
                    }
                    else
                    {
                        endEventList = _metadataStream.Tables[MetadataTables.Event].Length;
                    }
                }

                // If we have properties we need to load them, instantiate a PropertyDef and relate
                // it to its getter and setter.
                if(startEventList != -1)
                {
                    MetadataRow[] table = _metadataStream.Tables[MetadataTables.Event];
                    // Now load all the methods between our index and the endOfMethodIndex
                    for(int i = startEventList; i <= endEventList; i++)
                    {
                        EventMetadataTableRow eventRow = table[i - 1] as EventMetadataTableRow;

                        EventDef eventDef = new EventDef(
                            _references.Assembly.StringStream.GetString(eventRow.Name.Value),
                            _references.Assembly,
                            _builtType
                            );

                        _map.Add(MetadataTables.Event, eventRow, eventDef);
                        _builtType.Events.Add(eventDef);
                    }
                }
            }

            private void LoadProperties()
            {
                // Check if we have a property map and then find the property map for the current type
                // if it exists.
                if(!_metadataStream.Tables.ContainsKey(MetadataTables.PropertyMap))
                    return;
                 
                // TODO: The metadata tables are in order, we should use a sorted search algorithm to
                // find elements we need.
                int startPropertyList = -1;
                int endPropertyList = -1;
                PropertyMapMetadataTableRow searchFor = new PropertyMapMetadataTableRow();
                searchFor.Parent = _indexInMetadataTable;
                int mapIndex = Array.BinarySearch(_metadataStream.Tables[MetadataTables.PropertyMap],
                    searchFor,
                    new PropertyMapComparer()
                    );
                if(mapIndex >= 0)
                {
                    startPropertyList = ((PropertyMapMetadataTableRow)_metadataStream.Tables[MetadataTables.PropertyMap][mapIndex]).PropertyList;
                    if(mapIndex < _metadataStream.Tables[MetadataTables.PropertyMap].Length - 1)
                    {
                        endPropertyList = ((PropertyMapMetadataTableRow)_metadataStream.Tables[MetadataTables.PropertyMap][mapIndex + 1]).PropertyList - 1;
                    }
                    else
                    {
                        endPropertyList = _metadataStream.Tables[MetadataTables.Property].Length;
                    }
                }

                // If we have properties we need to load them, instantiate a PropertyDef and relate
                // it to its getter and setter.
                if(startPropertyList != -1)
                {
                    MetadataRow[] table = _metadataStream.Tables[MetadataTables.Property];
                    MetadataRow[] methodSemantics = _metadataStream.Tables[MetadataTables.MethodSemantics];

                    // Now load all the methods between our index and the endOfMethodIndex
                    for(int i = startPropertyList; i <= endPropertyList; i++)
                    {
                        PropertyMetadataTableRow propertyRow = table[i - 1] as PropertyMetadataTableRow;

                        PropertyDef property = new PropertyDef(
                            _references.Assembly, 
                            _references.Assembly.StringStream.GetString(propertyRow.NameIndex.Value),
                            _builtType);

                        // Get the related getter and setter methods
                        for(int j = 0; j < methodSemantics.Length; j++)
                        {
                            MethodSemanticsMetadataTableRow semantics = methodSemantics[j] as MethodSemanticsMetadataTableRow;
                            CodedIndex index = semantics.Association;
                            if(index.Table == MetadataTables.Property && index.Index == i)
                            {
                                if(semantics.Semantics == MethodSemanticsAttributes.Setter)
                                {
                                    property.Setter = _map.GetDefinition(
                                        MetadataTables.MethodDef,
                                        _metadataStream.GetEntryFor(MetadataTables.MethodDef, semantics.Method)
                                        ) as MethodDef;
                                }
                                else if(semantics.Semantics == MethodSemanticsAttributes.Getter)
                                {
                                    property.Getter = _map.GetDefinition(
                                        MetadataTables.MethodDef,
                                        _metadataStream.GetEntryFor(MetadataTables.MethodDef, semantics.Method)
                                        ) as MethodDef;
                                }
                            }
                        }

                        _map.Add(MetadataTables.Property, propertyRow, property);
                        _builtType.Properties.Add(property);
                    }
                }
            }
        }
    }
}