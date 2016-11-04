
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class for all elements that are reflected from a PeCoffFile.
    /// </summary>
    public abstract class ReflectedMember
    {
        private List<CustomAttribute> _attributes = new List<CustomAttribute>();
        private int _uniqueId;
        private AssemblyDef _assembly;
        private string _name;

        /// <summary>
        /// Returns an identifier which can uniquely identify this member across many <see cref="AssemblyDef"/>s.
        /// </summary>
        /// <returns>A globally unique identifier</returns>
        public virtual long GetGloballyUniqueId()
        {
            long id = (long)Assembly.UniqueId << 32;
            id += _uniqueId;
            return id;
        }

        /// <summary>
        /// Represents an identifier that uniquly identifies this reflected element inside 
        /// its containing <see cref="AssemblyDef"/>.
        /// </summary>
        public virtual int UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// A reference to the assembly which defines this member.
        /// </summary>
        public AssemblyDef Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        /// <summary>
        /// Gets or sets the name for this ReflectedMember.
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual Visibility MemberAccess
        {
            get { return Visibility.NotApplicable; }
        }

        /// <summary>
        /// The attributes associated with this member.
        /// </summary>
        public List<CustomAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }
    }
}