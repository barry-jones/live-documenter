
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
            get { return _attributeType.Type.Name; }
        }
    }
}