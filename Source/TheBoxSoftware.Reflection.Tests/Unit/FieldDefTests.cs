
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class FieldDefTests
    {
        [Test]
        public void IsSystemGenerated_WhenNameStartsWithValue_ReturnsTrue()
        {
            FieldDef field = new FieldDef();
            field.Name = "value__";

            Assert.IsTrue(field.IsSystemGenerated);
        }

        [Test]
        public void IsSystemGenerated_WhenHasCompilerGeneratedAttribute_ReturnsTrue()
        {
            MemberRef compilerGenerated = new MemberRef();
            compilerGenerated.Name = ".ctor";
            TypeRef attributeType = new TypeRef();
            attributeType.Name = "CompilerGeneratedAttribute";
            compilerGenerated.Type = attributeType;


            FieldDef field = new FieldDef();
            field.Attributes.Add(new CustomAttribute(compilerGenerated));

            Assert.IsTrue(field.IsSystemGenerated);
        }

        [Test]
        public void IsSystemGenerated_WhenNoValueOrCompilerAttribute_ReturnsFalse()
        {
            FieldDef field = new FieldDef();

            Assert.IsFalse(field.IsSystemGenerated);
        }
    }
}
