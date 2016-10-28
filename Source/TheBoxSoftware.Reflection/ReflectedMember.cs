
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class for all elements that are reflected from a PeCoffFile.
    /// </summary>
    public abstract class ReflectedMember
    {
        private List<CustomAttribute> _attributes = new List<CustomAttribute>();

        /// <summary>
        /// Returns an identifier which can uniquely identify this member across many <see cref="AssemblyDef"/>s.
        /// </summary>
        /// <returns>A globally unique identifier</returns>
        public virtual long GetGloballyUniqueId()
        {
            long id = ((long)this.Assembly.UniqueId) << 32;
            id += this.UniqueId;
            return id;
        }

        /// <summary>
        /// Returns the unique id of the Assembly
        /// </summary>
        /// <returns></returns>
        public virtual long GetAssemblyId()
        {
            return this.Assembly.UniqueId;
        }

        /// <summary>
        /// Returns the unique assembly identifier from a global id.
        /// </summary>
        /// <param name="id">The global identifier</param>
        /// <returns>The id of the assembly</returns>
        public static long GetAssemblyId(long id)
        {
            return id >> 32;
        }

        /// <summary>
        /// Represents an identifier that uniquly identifies this reflected element inside 
        /// its containing <see cref="AssemblyDef"/>.
        /// </summary>
        public virtual int UniqueId { get; set; }

        /// <summary>
        /// A reference to the assembly which defines this member.
        /// </summary>
        public AssemblyDef Assembly { get; set; }

        /// <summary>
        /// Gets or sets the name for this ReflectedMember.
        /// </summary>
        public virtual string Name { get; set; }

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