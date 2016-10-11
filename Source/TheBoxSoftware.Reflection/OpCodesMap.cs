using System;
using System.Collections.Generic;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Creates a nice searchable and iterable version of the
    /// OpCodes class.
    /// </summary>
    public sealed class OpCodesMap : List<OpCode>
    {
        private static OpCodesMap _singleton;
        private Dictionary<short, int> _map = new Dictionary<short, int>();

        /// <summary>
        /// Initialises a new instance of the OpCodesMap class
        /// </summary>
        private OpCodesMap()
        {
            Type t = typeof(OpCodes);
            foreach(System.Reflection.FieldInfo info in t.GetFields())
            {
                OpCode code = (OpCode)info.GetValue(null);
                this.Add(code);
                _map.Add(code.Value, this.IndexOf(code));
            }
        }

        /// <summary>
        /// Obtains a reference to the singleton
        /// </summary>
        /// <returns>The reference</returns>
        public static OpCodesMap GetSingleton()
        {
            if(OpCodesMap._singleton == null)
            {
                OpCodesMap._singleton = new OpCodesMap();
            }

            return OpCodesMap._singleton;
        }

        /// <summary>
        /// Returns the description of the instruction represented
        /// by 'instruction'
        /// </summary>
        /// <param name="instruction">The instruction</param>
        /// <returns>The OpCode details</returns>
        public OpCode GetCode(short instruction)
        {
            if(!this._map.ContainsKey(instruction))
            {
                return OpCodes.Nop;
            }

            return this[this._map[instruction]];
        }
    }
}