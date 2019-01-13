
namespace TheBoxSoftware.Documentation.Tests.Unit.Exporting.Rendering
{
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System.Text;
    using System.Xml;
    using TheBoxSoftware.Documentation.Exporting;
    using TheBoxSoftware.Documentation.Exporting.Rendering;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;

    [TestFixture]
    class MethodXmlRendererTests
    {
        public void Create()
        {
            CreateRenderer();
        }

        private XmlRenderer CreateRenderer()
        {
            MethodDef method = new MethodDef();
            method.Type = new TypeDef();
            method.Name = "aname";
            Mock<ICommentSource> comments = new Mock<ICommentSource>();

            Entry entry = new Entry(method, "aname", comments.Object);

            return new MethodXmlRenderer(entry);
        }
    }
}
