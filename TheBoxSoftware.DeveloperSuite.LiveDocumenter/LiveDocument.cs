using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Core;
	using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages;
	using TheBoxSoftware.Reflection.Comments;

	/// <summary>
	/// Represents a live document, which is a collection of pages which display
	/// code information and diagrams etc. For all of the loaded files for the
	/// current LiveDocumentFile projects.
	/// </summary>
	public sealed class LiveDocument {
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LiveDocument() { }

		/// <summary>
		/// Initialises a new LiveDocument
		/// </summary>
		/// <param name="files">The files to be managed by this LiveDocument.</param>
		public LiveDocument(List<DocumentedAssembly> files) {
			this.DocumentedFiles = files;
			this.DocumentMap = new System.Collections.ObjectModel.ObservableCollection<Entry>();
			this.Update();
		}

		/// <summary>
		/// Updates the contents of the live document for the current list of
		/// file references.
		/// </summary>
		public void Update() {
			if (this.DocumentMap == null) {
				this.DocumentMap = new System.Collections.ObjectModel.ObservableCollection<Entry>();
			}
			this.DocumentMap.Clear();
			this.GenerateDocumentMap();
		}

		/// <summary>
		/// Generates the document map for the currently referenced files in this
		/// LiveDocument.
		/// </summary>
		private void GenerateDocumentMap() {
			this.DocumentMap = new System.Collections.ObjectModel.ObservableCollection<Entry>();
			int fileCounter = 1;

			// For each of the documentedfiles generate the document map and add
			// it to the parent node of the document map
			for(int i = 0; i < this.DocumentedFiles.Count; i++) {
				Entry assemblyEntry = this.GenerateDocumentForAssembly(
					this.DocumentedFiles[i], ref fileCounter
					);
				this.DocumentMap.Add(assemblyEntry);
			}
		}

		/// <summary>
		/// Finds the entry in the document map with the specified key.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <param name="checkChildren">Wether or not to check the child entries</param>
		/// <returns>The entry that relates to the key or null if not found</returns>
		private Entry FindByKey(long key, string subKey, bool checkChildren) {
			Entry found = null;
			for(int i = 0; i < this.DocumentMap.Count; i++) {
				found = this.DocumentMap[i].FindByKey(key, subKey, checkChildren);
				if (found != null) {
					break;
				}
			}
			return found;
		}

		/// <summary>
		/// Searches the contents of the document map and returns any elements which
		/// contain the text provided in their names.
		/// </summary>
		/// <param name="searchText">The text to search for</param>
		/// <returns>The list of found entries or a single entry describing no results if not found</returns>
		internal List<Entry> Search(string searchText) {
			List<Entry> results = new List<Entry>();
			if (this.DocumentMap != null) {
				for(int i = 0; i < this.DocumentMap.Count; i++) {
					results.AddRange(this.DocumentMap[i].Search(searchText));
				}
			}
			return results;
		}

		#region Map Generation Methods
		public Entry GenerateDocumentForAssembly(DocumentedAssembly current, ref int fileCounter) {
			AssemblyDef assembly = AssemblyDef.Create(current.FileName);
			current.LoadedAssembly = assembly;

			XmlCodeCommentFile xmlComments = null;
			bool fileExists = System.IO.File.Exists(current.XmlFileName);
			if (fileExists) {
				xmlComments = new XmlCodeCommentFile(current.XmlFileName);
			}
			else {
				xmlComments = new XmlCodeCommentFile();
			}

			Entry assemblyEntry = new Entry(assembly, System.IO.Path.GetFileName(current.FileName), xmlComments);
			assembly.UniqueId = fileCounter++;
			assemblyEntry.Key = Helper.GetUniqueKey(assembly);
			assemblyEntry.IsSearchable = false;
			assemblyEntry.HasXmlComments = fileExists;

			// Add the namespaces to the document map
			foreach (KeyValuePair<string, List<TypeDef>> currentNamespace in assembly.GetTypesInNamespaces()) {
				if (string.IsNullOrEmpty(currentNamespace.Key)) {
					continue;
				}
				Entry namespaceEntry = assemblyEntry.FindByKey(assemblyEntry.Key, currentNamespace.Key, false);
				//namespaceEntry.Item = currentNamespace;
				if (namespaceEntry == null) {
					namespaceEntry = new Entry(currentNamespace, currentNamespace.Key, xmlComments, assemblyEntry);
					namespaceEntry.Key = assemblyEntry.Key;
					namespaceEntry.SubKey = currentNamespace.Key;
					namespaceEntry.IsSearchable = false;
					namespaceEntry.FullName = currentNamespace.Key;
				}

				// Add the types from that namespace to its map
				foreach (TypeDef currentType in currentNamespace.Value) {
					if (currentType.Name.StartsWith("<")) {
						continue;
					}
					Entry typeEntry = new Entry(currentType, currentType.GetDisplayName(false), xmlComments, namespaceEntry);
					typeEntry.Key = Helper.GetUniqueKey(assembly, currentType);
					typeEntry.IsSearchable = true;
					typeEntry.FullName = currentType.GetFullyQualifiedName();
					namespaceEntry.Children.Add(typeEntry);

					// For some elements we will not want to load the child objects
					// this is currently for System.Enum derived values.
					if (
						currentType.InheritsFrom != null && currentType.InheritsFrom.GetFullyQualifiedName() == "System.Enum" ||
						currentType.IsDelegate) {
						// Ignore children
					}
					else {
						this.GenerateTypeMap(currentType, typeEntry, xmlComments);
						typeEntry.Children.Sort();
					}
				}
				if (namespaceEntry.Children.Count > 0) {
					assemblyEntry.Children.Add(namespaceEntry);
					namespaceEntry.Children.Sort();
				}
			}

			assemblyEntry.Children.Sort();

			return assemblyEntry;
		}

		/// <summary>
		/// Generates the document map for all of the types child elements, fields, properties
		/// and methods.
		/// </summary>
		/// <param name="typeDef">The type to generate the map for.</param>
		/// <param name="typeEntry">The entry to add the child elements to.</param>
		/// <param name="commentsXml">The assembly comment file.</param>
		private void GenerateTypeMap(TypeDef typeDef, Entry typeEntry, XmlCodeCommentFile commentsXml) {
			List<MethodDef> methods = typeDef.GetMethods();
			List<MethodDef> constructors = typeDef.GetConstructors();
			List<FieldDef> fields = typeDef.GetFields();
			List<PropertyDef> properties = typeDef.GetProperties();
			List<EventDef> events = typeDef.GetEvents();
			List<MethodDef> operators = typeDef.GetOperators();

			if (typeDef.HasMembers) {
				Entry membersEntry = new Entry(typeDef, "Members", commentsXml, typeEntry);
				membersEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				membersEntry.SubKey = "Members";
				typeEntry.Children.Add(membersEntry);
			}

			if (constructors.Count > 0) {
				Entry constructorsEntry = new Entry(constructors, "Constructors", commentsXml, typeEntry);
				constructorsEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				constructorsEntry.SubKey = "Constructors";
				constructorsEntry.IsSearchable = false;
				typeEntry.Children.Add(constructorsEntry);

				// Add the method pages child page entries to the map
				int count = constructors.Count;
				for(int i = 0; i < count; i++) {
				// foreach (MethodDef currentMethod in constructors) {
					MethodDef currentMethod = constructors[i];
					Entry constructorEntry = new Entry(currentMethod, currentMethod.GetDisplayName(false, false), commentsXml, constructorsEntry);
					constructorEntry.IsSearchable = true;
					constructorEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, currentMethod);
					constructorsEntry.Children.Add(constructorEntry);
				}
				constructorsEntry.Children.Sort();
			}

			// Add a methods containing page and the associated methods
			if (methods.Count > 0) {				
				Entry methodsEntry = new Entry(methods, "Methods", commentsXml, typeEntry);
				methodsEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				methodsEntry.SubKey = "Methods";
				methodsEntry.IsSearchable = false;
				typeEntry.Children.Add(methodsEntry);
				
				// Add the method pages child page entries to the map
				int count = methods.Count;
				for(int i = 0; i < count; i++) {
					MethodDef currentMethod = methods[i];
					Entry methodEntry = new Entry(currentMethod, currentMethod.Name, commentsXml, methodsEntry);
					methodEntry.IsSearchable = true;
					methodEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, currentMethod);
					methodsEntry.Children.Add(methodEntry);
				}
				methodsEntry.Children.Sort();
			}

			if (operators.Count > 0) {
				Entry operatorsEntry = new Entry(operators, "Operators", commentsXml, typeEntry);
				operatorsEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				operatorsEntry.SubKey = "Operators";
				operatorsEntry.IsSearchable = false;
				typeEntry.Children.Add(operatorsEntry);

				int count = operators.Count;
				for (int i = 0; i < count; i++) {
					MethodDef current = operators[i];
					Entry operatorEntry = new Entry(current, current.GetDisplayName(false, false), commentsXml, operatorsEntry);
					operatorEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, current);
					operatorEntry.IsSearchable = true;
					operatorsEntry.Children.Add(operatorEntry);
				}
				operatorsEntry.Children.Sort();
			}

			// Add entries to allow the viewing of the types fields			
			if (fields.Count > 0) {
				Entry fieldsEntry = new Entry(fields, "Fields", commentsXml, typeEntry);
				fieldsEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				fieldsEntry.SubKey = "Fields";
				fieldsEntry.IsSearchable = false;
				typeEntry.Children.Add(fieldsEntry);

				foreach (FieldDef currentField in fields) {
					Entry fieldEntry = new Entry(currentField, currentField.Name, commentsXml, fieldsEntry);
					fieldEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, currentField);
					fieldEntry.IsSearchable = true;
					fieldsEntry.Children.Add(fieldEntry);
				}
				fieldsEntry.Children.Sort();
			}

			// Display the properties defined in the current type
			if (properties.Count > 0) {
				Entry propertiesEntry = new Entry(properties, "Properties", commentsXml, typeEntry);
				propertiesEntry.IsSearchable = false;
				propertiesEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				propertiesEntry.SubKey = "Properties";
				typeEntry.Children.Add(propertiesEntry);

				foreach (PropertyDef currentProperty in properties) {
					Entry propertyEntry = new Entry(currentProperty, currentProperty.Name, commentsXml, propertiesEntry);
					propertyEntry.IsSearchable = true;
					propertyEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, currentProperty);
					propertiesEntry.Children.Add(propertyEntry);
				}
				propertiesEntry.Children.Sort();
			}

			// Display the properties defined in the current type
			if (events.Count > 0) {
				Entry propertiesEntry = new Entry(events, "Events", commentsXml, typeEntry);
				propertiesEntry.IsSearchable = false;
				propertiesEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, typeDef);
				propertiesEntry.SubKey = "Events";
				typeEntry.Children.Add(propertiesEntry);

				foreach (EventDef currentProperty in events) {
					Entry propertyEntry = new Entry(currentProperty, currentProperty.Name, commentsXml, propertiesEntry);
					propertyEntry.IsSearchable = true;
					propertyEntry.Key = Helper.GetUniqueKey(typeDef.Assembly, currentProperty);
					propertiesEntry.Children.Add(propertyEntry);
				}
				propertiesEntry.Children.Sort();
			}
		}
		#endregion

		/// <summary>
		/// The files that are used to build the live document, this is the
		/// project, solution and library references.
		/// </summary>
		public List<DocumentedAssembly> DocumentedFiles { get; set; }

		/// <summary>
		/// The base document map entry for this live document.
		/// </summary>
		internal System.Collections.ObjectModel.ObservableCollection<Entry> DocumentMap { get; set; }
	}
}