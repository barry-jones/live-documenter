using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// Exception that describes issues working with reflection based code.
	/// </summary>
	public class ReflectionException : Exception, ISerializable, IExtendedException {
		/// <summary>
		/// Initialises a new instance of the ReflectionException.
		/// </summary>
		public ReflectionException() : base() { }

		/// <summary>
		/// Initialises a new instance of the ReflectionException.
		/// </summary>
		/// <param name="message">Message describing the error.</param>
		public ReflectionException(string message) : base(message) { }

		/// <summary>
		/// Initialises a new instance of the ReflectionException.
		/// </summary>
		/// <param name="message">Message describing the error.</param>
		/// <param name="innerException">The thrown exception.</param>
		public ReflectionException(string message, Exception innerException)
			: base(message, innerException) {
		}

		/// <summary>
		/// Initialises a new instance of the ReflectionException.
		/// </summary>
		/// <param name="member">The member associated with this exception.</param>
		/// <param name="message">Message describing the error.</param>
		/// <param name="innerException">The thrown exception.</param>
		public ReflectionException(ReflectedMember member, string message, Exception innerException)
			: base(message, innerException) {
				this.Member = member;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		/// <param name="info">The info describing the exception.</param>
		/// <param name="context">The serialization context.</param>
		protected ReflectionException(SerializationInfo info, StreamingContext context)
			: base(info, context) {
		}

		/// <summary>
		/// The member the exception is related to.
		/// </summary>
		public ReflectedMember Member { get; set; }

		/// <summary>
		/// Serializes the custom details of this exception to the SerializationInfo.
		/// </summary>
		/// <param name="info">The info to populate with custom details</param>
		/// <param name="context">The context</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context) {
			if (info == null) throw new ArgumentNullException("info");

			base.GetObjectData(info, context);
		}

		#region IExtendedException Members
		/// <summary>
		/// Extracts as much information from the internal state of the exception.
		/// </summary>
		/// <returns>The formatted extended information</returns>
		public string GetExtendedInformation() {
			StringBuilder builder = new StringBuilder();

			if (this.Member != null) {
				MemberRef memberRef = this.Member as MemberRef;

				// output the string stream name to help the user provide more details if required
				builder.AppendLine(string.Format("Name: {0}", this.Member.Name));

				if (this.Member is TypeRef) {
					try {
						builder.AppendLine(string.Format("Namespace: {0}", ((TypeRef)this.Member).Namespace));
					}
					catch(Exception) {}
				}
				else if (memberRef != null) {
					try { builder.AppendLine(string.Format("Containing Type: {0}", memberRef.Type.Name)); }
					catch(Exception) {}
					try { builder.AppendLine(string.Format("Namespace: {0}", memberRef.Type.Namespace)); }
					catch(Exception) {}
				}

				// attempt to output the syntax for the member
				try {
					Reflection.Syntax.IFormatter formatter = Reflection.Syntax.SyntaxFactory.Create(
						this.Member, Syntax.Languages.CSharp
						);
					builder.AppendLine(string.Format("Syntax: {0}", formatter.Format().ToString()));
				}
				catch(Exception) {} // ignore any errors here we know already we are walking on thin ice
			}
			else {
				builder.Append("Member is null, so no further information is available.");
			}

			return builder.ToString();
		}
		#endregion
	}
}
