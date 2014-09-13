using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

/*
 * Signiture defined in: 23.2.13
 * 
 * RANK->NUMSIZES->SIZE<->NUMLOBOUNDS->LOBOUND<->
 */

namespace TheBoxSoftware.Reflection.Signitures 
{
    /// <summary>
    /// A signiture that describes the shape of an array as defined.
    /// </summary>
	internal class ArrayShapeSignitureToken : SignitureToken 
    {
        // 16 bytes
        private Int32 rank;         // specifies the number of dimensions (1 or more)
        private Int32[] sizes;      // size of each dimension
        private Int32[] loBounds;   // the lower boundaries of those dimensions

        /// <summary>
        /// Initialises a new instance of the ArrayShapeSignitureToken which reads the array share
        /// signiture from the provided <paramref name="signiture"/> at <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob to read this token from.</param>
        /// <param name="offset">The offset where this roken begins.</param>
		public ArrayShapeSignitureToken(byte[] signiture, Offset offset)
			: base(SignitureTokens.ArrayShape) 
        {
            Int32 numSizes = 0;
            Int32 numLoBounds = 0;

			this.Rank = SignitureToken.GetCompressedValue(signiture, offset);

			numSizes = SignitureToken.GetCompressedValue(signiture, offset);
			this.Sizes = new Int32[numSizes];
			for (int i = 0; i < numSizes; i++) 
            {
				this.Sizes[i] = SignitureToken.GetCompressedValue(signiture, offset);
			}

			numLoBounds = SignitureToken.GetCompressedValue(signiture, offset);
			this.LoBounds = new Int32[numLoBounds];
			for (int i = 0; i < numLoBounds; i++) 
            {
				this.LoBounds[i] = SignitureToken.GetCompressedValue(signiture, offset);
			}
		}

        /// <summary>
        /// Produces a string representation e.g. '[1..6, 5, ,]' of the array 
        /// shape token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ArraySize: [");

            for (int i = 0; i < this.Rank; i++)
            {
                if (this.LoBounds.Length < i)
                {
                    sb.AppendFormat("{0}...", this.LoBounds[i]);
                }
                if (this.Sizes.Length < i)
                {
                    sb.AppendFormat("{0}", this.Sizes[i]);
                }
                if (i != this.Rank - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]]");

            return sb.ToString();
        }

        /// <summary>
        /// The number of ranks in the array shape.
        /// </summary>
		public Int32 Rank 
        {
            get { return this.rank; }
            private set { this.rank = value; }
        }

        /// <summary>
        /// The defined sizes
        /// </summary>
		public Int32[] Sizes 
        {
            get { return this.sizes; }
            private set { this.sizes = value; }
        }

        /// <summary>
        /// The values of those low boundaries.
        /// </summary>
		public Int32[] LoBounds 
        {
            get { return this.loBounds; }
            private set { this.loBounds = value; }
        }
	}
}
