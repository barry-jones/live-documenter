
namespace TheBoxSoftware.Reflection.Comments
{
    using System;

    public class ErrorXmlCodeElement : XmlCodeElement
    {
        public ErrorXmlCodeElement(Exception exception)
            : base(XmlCodeElements.Text)
        {
            Text = $"[ERROR:{exception.Message}]";
            IsInline = true;
        }
    }
}
