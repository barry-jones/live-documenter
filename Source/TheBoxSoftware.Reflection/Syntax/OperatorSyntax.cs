
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;
    using Reflection.Signitures;

    internal class OperatorSyntax : Syntax
    {
        private MethodDef _method;
        private Signiture _signiture;

        public OperatorSyntax(MethodDef method)
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
        /// <remarks>
        /// Operators have names such as op_Equality, but when actually producing
        /// the overload in code is == (c#). This will return the name as defined
        /// metadata, e.g. op_Equality - it is up to the language specific implementation
        /// to convert that to a more suitable representation.
        /// </remarks>
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
            return _method.GenericTypes;
        }

        public TypeDetails GetReturnType()
        {
            ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)_signiture.Tokens.Find(
                t => t.TokenType == SignitureTokens.ReturnType
                );
            TypeDetails details = returnType.GetTypeDetails(_method);

            return details;
        }

        public List<ParameterDetails> GetParameters()
        {
            List<ParameterDetails> details = new List<ParameterDetails>();
            List<ParamSignitureToken> definedParameters = new List<ParamSignitureToken>(_signiture.Tokens.FindAll(
                t => t.TokenType == SignitureTokens.Param
                ).ConvertAll<ParamSignitureToken>(p => (ParamSignitureToken)p).ToArray());
            List<ParamDef> parameters = _method.Parameters;

            for(int i = 0; i < parameters.Count; i++)
            {
                details.Add(new ParameterDetails(
                    parameters[i].Name,
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