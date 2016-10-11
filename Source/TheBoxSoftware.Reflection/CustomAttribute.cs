namespace TheBoxSoftware.Reflection
{
    public class CustomAttribute
    {
        private MemberRef attributeType;

        public CustomAttribute(MemberRef attributeType)
        {
            this.attributeType = attributeType;
        }

        public string Name
        {
            get { return this.attributeType.Type.Name; }
        }
    }
}