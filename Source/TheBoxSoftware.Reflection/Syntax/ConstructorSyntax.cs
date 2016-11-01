
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;
    using Signitures;

    internal class ConstructorSyntax : Syntax
    {
        private MethodDef _method;
        private Signiture _signiture;

        public ConstructorSyntax(MethodDef method)
        {
            _method = method;
            _signiture = method.Signiture;
        }

        public Visibility GetVisibility()
        {
            return _method.MemberAccess;
        }

        public Inheritance GetInheritance()
        {
            // TODO: Fix static is not returned for static constructors
            //  it seems as if .cctor is a static constructor and .ctor is not in IL though they both have static set.
            //  When viewing the metadata via the peviewer it shows .cctor but here it is displayed as .ctor - I think we
            //  must be changing it somewhere.
            return ConvertMethodInheritance(_method.Attributes);
        }

        /// <summary>
        /// Obtains the cleaned up identifier for the method.
        /// </summary>
        /// <returns>The name of the method.</returns>
        public string GetIdentifier()
        {
            // convert from .ctor to the typename as this is the standard language syntax for constructors
            string typeName = _method.Type.Name;

            if(_method.Type.IsGeneric)
            {
                typeName = typeName.Substring(0, typeName.IndexOf('`'));
            }

            return typeName;
        }

        public List<ParameterDetails> GetParameters()
        {
            List<ParameterDetails> details = new List<ParameterDetails>();
            List<ParamSignitureToken> definedParameters = new List<ParamSignitureToken>(_signiture.Tokens.FindAll(
                t => t.TokenType == SignitureTokens.Param
                ).ConvertAll(p => (ParamSignitureToken)p).ToArray());
            List<ParamDef> parameters = _method.Parameters;

            for(int i = 0; i < parameters.Count; i++)
            {
                details.Add(new ParameterDetails(
                    parameters[i],
                    definedParameters[i].GetTypeDetails(_method)
                    ));
            }

            return details;
        }

        public MethodDef Method
        {
            get { return _method; }
        }
    }
}