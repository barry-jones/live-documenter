
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

        [Test]
        public void IsInterface()
        {
            TypeDef inter = new TypeDef();
            inter.Flags = Reflection.Core.COFF.TypeAttributes.Interface;
            TypeDef none = new TypeDef();

            Assert.IsTrue(inter.IsInterface);
            Assert.IsFalse(none.IsInterface);
        }

        [Test]
        public void Namespace_WhenInContainingType_IsCorrect()
        {
            // currently returns a space at the end...
            const string ExpectedResult = "BaseNamespace.Container ";

            TypeDef testType = new TypeDef() { Name = "MyClass" };
            testType.ContainingClass = new TypeDef() { Namespace = "BaseNamespace", Name = "Container " };

            Assert.AreEqual(ExpectedResult, testType.Namespace);
        }
        
        [Test]
        public void Namespace()
        {
            const string ExpectedResult = "BaseNamespace";
            TypeDef testType = new TypeDef() { Namespace = "BaseNamespace", Name = "MyClass" };

            Assert.AreEqual(ExpectedResult, testType.Namespace);
        }

        [Test]
        public void IsGenerated()
        {
            TypeDef generated = new TypeDef();
            generated.Attributes.Add(CreateGeneratedAttribute());
            TypeDef def = new TypeDef();
            TypeDef child = new TypeDef
            {
                ContainingClass = generated
            };

            Assert.IsTrue(generated.IsCompilerGenerated, "When compiler generated");
            Assert.IsFalse(def.IsCompilerGenerated, "When not compiler generated");
            Assert.IsTrue(child.IsCompilerGenerated, "When child of compiler generated");
        }

        private CustomAttribute CreateGeneratedAttribute()
        {
            MemberRef attributeType = new MemberRef();
            attributeType.Type = new TypeRef() { Name = "CompilerGeneratedAttribute" };
            return new CustomAttribute(attributeType);
        }
    }
}
