using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TheBoxSoftware.Reflection.Core;
using TheBoxSoftware.Diagnostics;

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// 
    /// </summary>
	internal abstract class Signiture 
    {
        // 8 bytes
        private Signitures type;
        private List<SignitureToken> tokens;

		/// <summary>
		/// Initialises a new instance of the Signiture class.
		/// </summary>
		/// <param name="tokenType">The type of signiture being represented.</param>
		protected Signiture(Signitures tokenType) 
        {
			this.Type = tokenType;
			this.Tokens = new List<SignitureToken>();
		}

		/// <summary>
		/// Factory method for creating new Signitures.
		/// </summary>
		/// <param name="fileContents">The contents of the library the signiture is being read from</param>
		/// <param name="offset">The offset to the start of the signiture</param>
		/// <param name="file">The PeCoffFile that is being read (same as file contents)</param>
		/// <param name="tokenType">The type of signiture being read.</param>
		/// <returns></returns>
		public static Signiture Create(byte[] fileContents, Offset offset, PeCoffFile file, Signitures tokenType) 
        {
			int startingOffset = offset;
			int lengthOfSigniture = SignitureToken.GetCompressedValue(fileContents, offset);	// The first byte is always the length

			// Read the full signiture
			byte[] signiture = new byte[lengthOfSigniture];
			for (int i = 0; i < lengthOfSigniture; i++) {
				signiture[i] = fileContents[offset.Shift(1)];
			}

			// Instatiate the correct signiture reader and pass control
			Signiture instantiatedSigniture = null;
			switch(tokenType) {
				case Signitures.MethodDef: instantiatedSigniture = new MethodDefSigniture(file, signiture); break;
				case Signitures.MethodRef: instantiatedSigniture = new MethodRefSigniture(file, signiture); break;
				case Signitures.Field: instantiatedSigniture = new FieldSigniture(file, signiture); break;
				case Signitures.Property: instantiatedSigniture = new PropertySigniture(file, signiture); break;
				case Signitures.TypeSpecification: instantiatedSigniture = new TypeSpecificationSigniture(file, signiture); break;
			}
			return instantiatedSigniture;
		}

		/// <summary>
		/// Returns a collection of all the parameter tokens defined in this
		/// signiture.
		/// </summary>
		/// <returns>A collection of parameter tokens.</returns>
		public List<ParamSignitureToken> GetParameterTokens() 
        {
			return (from token in this.Tokens
					where token is ParamSignitureToken
					select (ParamSignitureToken)token).ToList<ParamSignitureToken>();
		}

		/// <summary>
		/// Returns the token that describes the return type defined in the signiture.
		/// </summary>
		/// <returns>The Token or null if no return type defined.</returns>
		public ReturnTypeSignitureToken GetReturnTypeToken() 
        {
			ReturnTypeSignitureToken token = null;
			for (int i = 0; i < this.Tokens.Count; i++) {
				if (this.Tokens[i] is ReturnTypeSignitureToken) {
					token = (ReturnTypeSignitureToken)this.Tokens[i];
					break;
				}
			}
			return token;
		}

		/// <summary>
		/// Describes the type of signiture.
		/// </summary>
		public Signitures Type 
        {
            get { return this.type; }
            protected set { this.type = value; }
        }

		public List<SignitureToken> Tokens
        {
            get { return this.tokens; }
            protected set { this.tokens = value; }
        }
	}
}
