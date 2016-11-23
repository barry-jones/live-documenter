
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains the details and MSIL for a method definition.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The internal constructor is so that these classes can be instantiated from
    /// a valid MethodDef instance. Further this is the only way these objects should
    /// be created.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodDef.GetMethodBody()"/>
    /// <seealso cref="MethodDef"/>
    public sealed class MethodBody
    {
        private List<ILInstruction> _instructions;
        private int _maxStack;

        /// <summary>
        /// Initialsies a new instance of the MethodBody class.
        /// </summary>
        /// <param name="instructions">The instructions that make up the methods body.</param>
        /// <param name="maxStack">The maximum size of the stack for this method.</param>
        internal MethodBody(List<ILInstruction> instructions, Int32 maxStack)
        {
            _instructions = instructions;
            _maxStack = maxStack;
        }

        /// <summary>
        /// Obtains the MSIL instructions for this method.
        /// </summary>
        public List<ILInstruction> Instructions
        {
            get { return _instructions; }
        }

        /// <summary>
        /// Indicates the maximum number of items that appear on the stack in this
        /// method.
        /// </summary>
        public Int32 MaxStack
        {
            get { return _maxStack; }
        }
    }
}
