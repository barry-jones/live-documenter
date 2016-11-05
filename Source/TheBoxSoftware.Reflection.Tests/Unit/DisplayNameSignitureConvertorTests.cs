
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Reflection.Signitures;

    [TestFixture]
    public class DisplayNameSignitureConvertorTests
    {
        [Test]
        public void Create()
        {
            TypeDef type = new TypeDef();
            var convertor = new DisplayNameSignitureConvertor(type, false);
        }
    }
}
