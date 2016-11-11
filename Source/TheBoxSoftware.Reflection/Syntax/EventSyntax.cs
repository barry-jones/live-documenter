
namespace TheBoxSoftware.Reflection.Syntax
{
    using TheBoxSoftware.Reflection.Signatures;

    /// <summary>
    /// Allows the caller to obtain details of an event in a structured way.
    /// </summary>
    internal class EventSyntax : Syntax
    {
        private EventDef _eventDef;
        private MethodDef _add;
        private MethodDef _remove;

        /// <summary>
        /// Initialises a new instance of the EventSyntax class.
        /// </summary>
        /// <param name="eventDef">The details of the event to get the information from.</param>
        public EventSyntax(EventDef eventDef)
        {
            _eventDef = eventDef;
            _add = eventDef.FindAddMethod();
            _remove = eventDef.FindRemoveMethod();
        }

        public string GetIdentifier()
        {
            return _eventDef.Name;
        }

        /// <summary>
        /// Obtains the visibility for the event.
        /// </summary>
        /// <returns>The visibility.</returns>
        /// <remarks>
        /// This visibility of an event, as with a property, is
        /// determined by the most accessible method.
        /// </remarks>
        public Visibility GetVisbility()
        {
            return _eventDef.MemberAccess;
        }

        public Inheritance GetInheritance()
        {
            MethodDef method = _add ?? _remove;
            return ConvertMethodInheritance(method.MethodAttributes);
        }

        public new TypeDetails GetType()
        {
            MethodDef method = _remove ?? _add;
            ParamSignatureToken delegateType = method.Signiture.GetParameterTokens()[0];
            TypeDetails details = delegateType.GetTypeDetails(method);
            return details;
        }
    }
}