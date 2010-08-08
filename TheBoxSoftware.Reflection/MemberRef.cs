﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Represents an instance of a Member that is referenced from an external source.
	/// </summary>
	public class MemberRef : ReflectedMember {
		/// <summary>
		/// Internal variable allowing derived types to set the IsConstructor
		/// property.
		/// </summary>
		protected bool isConstructor = false;
		/// <summary>
		/// Internal variable allowing derived types to set the IsOperator
		/// property.
		/// </summary>
		protected bool isOperator = false;

		#region Methods
		/// <summary>
		/// Factor method for instantiating and populating MemberRef instances from
		/// Metadata.
		/// </summary>
		/// <param name="assembly">The assembly the reference is defined in.</param>
		/// <param name="metadata">The metadata the reference is detailed in.</param>
		/// <param name="row">The actual metadata row with the details of the member.</param>
		/// <returns>An instantiated MemberRef instance.</returns>
		internal static MemberRef CreateFromMetadata(
				AssemblyDef assembly, 
				MetadataDirectory metadata, 
				MemberRefMetadataTableRow row) {
			MemberRef memberRef = new MemberRef();

			memberRef.UniqueId = assembly.GetUniqueId();
			memberRef.Type = (TypeRef)assembly.ResolveCodedIndex(row.Class);
			memberRef.Name = assembly.StringStream.GetString(row.Name.Value);
			memberRef.SignitureBlob = row.Signiture;
			memberRef.Assembly = assembly;

			// These methods of detecting different method types are not
			// infalable. A user can create a method for example that starts iwth
			// get_, set_ or op_. This best detail is stored in the MethodSemantics
			// table AND we will need to load that at some point :/
			memberRef.isConstructor = memberRef.Name.StartsWith(".");
			memberRef.isOperator = memberRef.Name.StartsWith("op_");

			return memberRef;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the type which defines this member
		/// </summary>
		public TypeRef Type { get; set; }

		/// <summary>
		/// Gets or sets the name of the member
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Obtains the index in the BlobStream where the methods signiture
		/// is stored.
		/// </summary>
		/// <seealso cref="Signiture"/>
		protected BlobIndex SignitureBlob { get; set; }

		/// <summary>
		/// Gets a value indicating if this member is a constructor.
		/// </summary>
		public bool IsConstructor { 
			get { return this.isConstructor; } 
		}

		/// <summary>
		/// Gets a value indicating if this method referes to an operator overloaded
		/// method implementation.
		/// </summary>
		public bool IsOperator {
			get { return this.isOperator; }
		}

		/// <summary>
		/// Gets the signiture defined for this member.
		/// </summary>
		public Reflection.Signitures.Signiture Signiture {
			get {
				if (!this.Assembly.File.IsMetadataLoaded) {
					throw new InvalidOperationException("The Signiture can not be parsed correctly until the metadata has been loaded.");
				}

				BlobStream stream = (BlobStream)((Core.COFF.CLRDirectory)this.Assembly.File.Directories[
					Core.PE.DataDirectories.CommonLanguageRuntimeHeader]).Metadata.Streams[Streams.BlobStream];
				return stream.GetSigniture((int)this.SignitureBlob.Value, this.SignitureBlob.SignitureType);
			}
		}
		#endregion
	}
}