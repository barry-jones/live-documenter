
namespace TheBoxSoftware.Reflection.Tests.Unit.Syntax
{
    using System;
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class ClassSyntaxTests
    {
        [TestCase("TypeName", false, "TypeName")]
        [TestCase("TypeName`2", true, "TypeName")]
        public void ClassSyntax_GetIdentifier(string name, bool isGeneric, string expectedResult)
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.IsGeneric = isGeneric;
            typeDef.Name = name;

            string result = syntax.GetIdentifier();

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ClassSyntax_GetInterfaces_WhenTypeImplementsNothing_ShouldReturnNoEntries()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.Implements = new System.Collections.Generic.List<TypeRef>();

            Array result = syntax.GetInterfaces();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void ClassSyntax_GetInterfaces_WhenTypeImplementsNonTypeSpecEntries_ShouldReturnEntries()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.Implements = new System.Collections.Generic.List<TypeRef>() {
                new TypeRef { Name = "First" },
                new TypeRef { Name = "Second" }
            };

            // cant test this with typespec entries yet as there is an internal load which requires us to be
            // able to set private variables.

            Array result = syntax.GetInterfaces();

            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void ClassSyntax_GetInheritance_WhenFlagsNotSet_ShouldReturnDefault()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            Assert.AreEqual(Inheritance.Default, syntax.GetInheritance());
        }

        [Test]
        public void ClassSyntax_GetInheritance_WhenFlagsAbstractAndSealed_ShouldReturnStatic()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.Flags |= Reflection.Core.COFF.TypeAttributes.Abstract;
            typeDef.Flags |= Reflection.Core.COFF.TypeAttributes.Sealed;

            Assert.AreEqual(Inheritance.Static, syntax.GetInheritance());
        }

        [Test]
        public void ClassSyntax_GetInheritance_WhenFlagsAbstract_ShouldReturnAbstract()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.Flags = Reflection.Core.COFF.TypeAttributes.Abstract;

            Assert.AreEqual(Inheritance.Abstract, syntax.GetInheritance());
        }

        [Test]
        public void ClassSyntax_GetInheritance_WhenFlagsSealed_ShouldReturnSealed()
        {
            TypeDef typeDef = new TypeDef();
            ClassSyntax syntax = new ClassSyntax(typeDef);

            typeDef.Flags = Reflection.Core.COFF.TypeAttributes.Sealed;

            Assert.AreEqual(Inheritance.Sealed, syntax.GetInheritance());
        }
    }
}
