using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBoxSoftware.Diagnostics;
using TheBoxSoftware.Reflection;

namespace signitures
{
    /// <summary>
    /// Main application program which contains the entry point.
    /// </summary>
    internal class Program
    {
        private AssemblyDef assemblyDef;

        /// <summary>
        /// Main application entry point.
        /// </summary>
        /// <param name="args">Comand line arguments.</param>
        public static void Main(string[] args)
        {
            TraceHelper.Initialise(new TraceSource("myTraceSource"));

            Program p = new Program();
            p.PrintOutSignituresAsTokens();
        }

        /// <summary>
        /// Returns a reference to the assembly to test with.
        /// </summary>
        /// <returns></returns>
        private AssemblyDef LoadAssembly()
        {
            if (this.assemblyDef == null)
            {
                this.assemblyDef = AssemblyDef.Create(System.Configuration.ConfigurationSettings.AppSettings["assembly"]);
            }

            return this.assemblyDef;
        }

        /// <summary>
        /// Print all signitures as tokens to verify parsing
        /// </summary>
        private void PrintOutSignituresAsTokens()
        {
            AssemblyDef def = this.LoadAssembly();
            def.PrintTypeSpecSignitures();
        }
    }
}
