
namespace TheBoxSoftware.Reflection.Syntax
{
    /// <summary>
    /// Details class which provides access to information necessary
    /// to understand the construction of a parameter.
    /// </summary>
    /// <see cref="Signitures.TypeDetails"/>
    internal class ParameterDetails
    {
        private ParamDef _parameter;

        /// <summary>
        /// Initialises a new instance of the ParameterDetails class.
        /// </summary>
        public ParameterDetails() { }

        /// <summary>
        /// Initialises a new instance of the ParameterDetails class.
        /// </summary>
        /// <param name="parameter">The details of the parameter.</param>
        /// <param name="details">The details of the type for the parameter.</param>
        public ParameterDetails(ParamDef parameter, Signitures.TypeDetails details)
        {
            TypeDetails = details;
            _parameter = parameter;
        }

        /// <summary>
        /// The underlying parameter
        /// </summary>
        public ParamDef Parameter
        {
            get { return _parameter; }
        }

        /// <summary>
        /// The full details of the type.
        /// </summary>
        public Signitures.TypeDetails TypeDetails { get; set; }

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name
        {
            get { return _parameter.Name; }
        }
    }
}