
using System;

namespace TheBoxSoftware.Exporter
{
    public class InvalidParameterException : Exception
    {
        private readonly string _parameter;
        private readonly string _value;

        public InvalidParameterException() { }

        public InvalidParameterException(string parameter, string value)
            : this(parameter, value, string.Empty)
        {
        }

        public InvalidParameterException(string parameter, string value, string message) : base(message)
        {
            _parameter = parameter;
            _value = value;
        }

        public string Parameter
        {
            get => _parameter;
        }

        public string Value
        {
            get => _value;
        }
    }
}
