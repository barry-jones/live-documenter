
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception that describes issues working with reflection based code.
    /// </summary>
    public class ReflectionException : Exception, IExtendedException
    {
        private ReflectedMember _member;

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
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the ReflectionException.
        /// </summary>
        /// <param name="member">The member associated with this exception.</param>
        /// <param name="message">Message describing the error.</param>
        /// <param name="innerException">The thrown exception.</param>
        public ReflectionException(ReflectedMember member, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Member = member;
        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info">The info describing the exception.</param>
        /// <param name="context">The serialization context.</param>
        protected ReflectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Extracts as much information from the internal state of the exception.
        /// </summary>
        /// <returns>The formatted extended information</returns>
        public string GetExtendedInformation()
        {
            StringBuilder builder = new StringBuilder();

            if(_member != null)
            {
                MemberRef memberRef = _member as MemberRef;

                // output the string stream name to help the user provide more details if required
                builder.AppendLine($"Name: {_member.Name}");

                if(_member is TypeRef)
                {
                    try
                    {
                        builder.AppendLine($"Namespace: {((TypeRef)_member).Namespace}");
                    }
                    catch(Exception) { }
                }
                else if(memberRef != null)
                {
                    try { builder.AppendLine($"Containing Type: {memberRef.Type.Name}"); }
                    catch(Exception) { }
                    try { builder.AppendLine($"Namespace: {memberRef.Type.Namespace}"); }
                    catch(Exception) { }
                }

                // attempt to output the syntax for the member
                try
                {
                    Reflection.Syntax.IFormatter formatter = Reflection.Syntax.SyntaxFactory.Create(
                        _member, Syntax.Languages.CSharp
                        );
                    builder.AppendLine($"Syntax: {formatter.Format().ToString()}");
                }
                catch(Exception) { } // ignore any errors here we know already we are walking on thin ice
            }
            else
            {
                builder.Append("Member is null, so no further information is available.");
            }

            return builder.ToString();
        }

        /// <summary>
        /// The member the exception is related to.
        /// </summary>
        public ReflectedMember Member
        {
            get { return _member; }
            set { _member = value; }
        }
    }
}