
namespace TheBoxSoftware.Reflection
{
    public class CustomAttribute
    {
        private MemberRef _attributeType;

        public CustomAttribute(MemberRef attributeType)
        {
            _attributeType = attributeType;
        }

        public string Name
        {
            // name has to be obtained from the type of the member, because attributes are generally
            // added as the constructors. So a CompilerGenerated attribute is the constructor method and
            // the type contains the actual name of the attribute.

            get { return _attributeType.Type.Name; }
        }
    }
}