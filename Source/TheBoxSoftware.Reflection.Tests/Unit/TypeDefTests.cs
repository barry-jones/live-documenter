
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public class TypeDefTests
    {
        [Test]
        public void Create()
        {
            TypeDef def = new TypeDef();
        }

        [Test]
        public void GetFields_WhenNoFields_IsEmpty()
        {
            TypeDef def = new TypeDef();

            Assert.AreEqual(0, def.GetFields().Count);
        }

        [Test]
        public void GetFields_WhenOnlyFieldIsSystemGenerated_IsEmpty()
        {
            TypeDef def = new TypeDef();
            def.Fields.Add(new FieldDef() {
                Name = "value__"
            });

            Assert.AreEqual(0, def.GetFields().Count);
        }

        [Test]
        public void GetFields_WhenNonSystemGenerated_ReturnsFields()
        {
            TypeDef def = new TypeDef();
            def.Fields.Add(new FieldDef() {
                Name = "myField"
            });

            Assert.AreEqual(1, def.GetFields().Count);
        }

        [Test]
        public void GetFields_WhenIncludeSystemGenerated_ReturnsFields()
        {
            TypeDef def = new TypeDef();
            def.Fields.AddRange(new FieldDef[]
            {
                new FieldDef() { Name = "value__" },
                new FieldDef() { Name = "myField" }
            });

            Assert.AreEqual(2, def.GetFields(true).Count);
        }
    }
}
