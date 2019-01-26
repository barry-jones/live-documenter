
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

        [Test]
        public void GetMethods_WhenNoMethods_IsEmpty()
        {
            TypeDef def = new TypeDef();

            Assert.AreEqual(0, def.GetMethods().Count);
        }

        [Test]
        public void GetMethods_WhenHasMethods_Returns()
        {
            TypeDef def = new TypeDef();
            def.Methods.Add(new MethodDef() { Name = "AMethod" });

            Assert.AreEqual(1, def.GetMethods().Count);
        }

        [Test]
        public void GetMethods_WhenOnlySystemGenerated_IsEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = "<somemethod", MethodAttributes = Reflection.Core.COFF.MethodAttributes.SpecialName },
                new MethodDef() { Name = "<anothergeneratedmethod", MethodAttributes = Reflection.Core.COFF.MethodAttributes.SpecialName }
            });

            Assert.AreEqual(0, def.GetMethods().Count);
        }

        [Test]
        public void GetMethods_IncludeSystemGenerated_IsNotEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = "<somemethod", MethodAttributes = Reflection.Core.COFF.MethodAttributes.SpecialName },
                new MethodDef() { Name = "<anothergeneratedmethod", MethodAttributes = Reflection.Core.COFF.MethodAttributes.SpecialName },
                new MethodDef() { Name = "notcompilergenerated" }
            });

            Assert.AreEqual(3, def.GetMethods(true).Count);
        }

        [Test]
        public void GetConstructors_WhenSystemGenerated_IsEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = ".ctor", IsConstructor = true }
            });
            def.Methods[0].Attributes.Add(CreateGeneratedAttribute());

            Assert.AreEqual(0, def.GetConstructors().Count);
        }

        [Test]
        public void GetConstructors_WhenNotSystemGenerated_IsNotEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = ".ctor", IsConstructor = true },
                new MethodDef() { Name = ".ctor", IsConstructor = true }
            });

            Assert.AreEqual(2, def.GetConstructors().Count);
        }

        [Test]
        public void GetOperators_WhenSystemGenerated_IsEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = "op_one", IsOperator = true }
            });
            def.Methods[0].Attributes.Add(CreateGeneratedAttribute());

            Assert.AreEqual(0, def.GetOperators().Count);
        }

        [Test]
        public void GetOperators_WhenNotSystemGenerated_IsNotEmpty()
        {
            TypeDef def = new TypeDef();
            def.Methods.AddRange(new MethodDef[] {
                new MethodDef() { Name = "op_one", IsOperator = true },
                new MethodDef() { Name = "op_tow", IsOperator = true }
            });

            Assert.AreEqual(2, def.GetOperators().Count);
        }

        private CustomAttribute CreateGeneratedAttribute()
        {
            MemberRef attributeType = new MemberRef();
            attributeType.Type = new TypeRef() { Name = "CompilerGeneratedAttribute" };
            return new CustomAttribute(attributeType);
        }
    }
}
