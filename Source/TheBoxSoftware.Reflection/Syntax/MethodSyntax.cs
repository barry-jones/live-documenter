
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;
    using Reflection.Signitures;

    internal class MethodSyntax : Syntax
    {
        private MethodDef _method;
        private Signiture _signiture;

        public MethodSyntax(MethodDef method)
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
            return ConvertMethodInheritance(_method.Attributes);
        }

        /// <summary>
        /// Obtains the cleaned up identifier for the method.
        /// </summary>
        /// <returns>The name of the method.</returns>
        public string GetIdentifier()
        {
            return _method.Name;
        }

        /// <summary>
        /// Obtains a collection of <see cref="GenericTypeRef"/> instances detailing
        /// the generic types for this method.
        /// </summary>
        /// <returns>The collection of generic parameters for the method.</returns>
        /// <remarks>
        /// This method is only valid when the <see cref="MethodDef.IsGeneric"/> property
        /// has been set to true.
        /// </remarks>
        public List<GenericTypeRef> GetGenericParameters()
        {
            return _method.GetGenericTypes();
        }

        public TypeDetails GetReturnType()
        {
            ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)_signiture.Tokens.Find(
                t => t.TokenType == SignitureTokens.ReturnType
                );
            TypeDetails details = returnType.GetTypeDetails(_method);

            return details;
        }

        /// <summary>
        /// Collects and returns all of the parameters for the associated <see cref="MethodDef" />.
        /// </summary>
        /// <returns>The list of <see cref="ParameterDetails" /> detailing the parameters</returns>
        public List<ParameterDetails> GetParameters()
        {
            List<ParameterDetails> details = new List<ParameterDetails>();
            List<ParamSignitureToken> definedParameters = new List<ParamSignitureToken>(
                _signiture.Tokens.FindAll(t => t.TokenType == SignitureTokens.Param)
                    .ConvertAll<ParamSignitureToken>(p => (ParamSignitureToken)p)
                    .ToArray()
                );
            List<ParamDef> parameters = _method.Parameters;

            // if a method has a return value (sequence 0) we need to miss it out as it is not in the
            // methods normal parameter list. That information is returned by GetReturnType
            bool hasReturnParam = false;
            for(int i = 0; i < parameters.Count; i++)
            {
                if(parameters[i].Sequence == 0)
                {
                    hasReturnParam = true;
                    continue;
                }
                details.Add(new ParameterDetails(
                    parameters[i].Name,
                    definedParameters[hasReturnParam ? i - 1 : i].GetTypeDetails(_method)
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