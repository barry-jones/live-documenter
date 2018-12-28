
namespace TheBoxSoftware.Reflection.Tests.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// A set of tests which check various aspects of the loading and storage of various entries in
    /// a pe/coff file.
    /// </summary>
    [TestFixture]
    public class AssemblyDefTests
    {
        private const string TestFile = @"..\..\..\..\testoutput\documentationtest.dll";

        private AssemblyDef _assemblyDef;

        [OneTimeSetUp]
        public void InitialiseAssemblyDefFileUsedForTests()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            _assemblyDef = AssemblyDef.Create(System.IO.Path.Combine(dir, TestFile));
        }

        [Test, Category("Integration")]
        public void Bug45_ClassesWithNoNamspace_WhenFindTypeInAssembly_TypeReturned()
        {
            // [#45] we cant currently find types without namespaces when searching this is a bug and 
            // needs to be resolved. This however also manifests itself in the document mappers and we 
            // need to check the export code as well (which uses document mappers).
            TypeDef found = _assemblyDef.FindType(string.Empty, "Issue45_TypeWithNoNamespace");

            Assert.IsNotNull(found);
            Assert.AreEqual(string.Empty, found.Namespace);
            Assert.AreEqual("Issue45_TypeWithNoNamespace", found.Name);
        }

        [Test, Category("Integration")]
        public void Bug45_ClassesWithNoNamspace_NamespaceIsNull_ReturnsNull()
        {
            // With generic types they can sometimes have a null namespace value, this is to make sure
            // we do not throw an exception in those instances.
            TypeDef found = _assemblyDef.FindType(null, "T");

            Assert.IsNull(found);
        }


        [Test, Category("Integration")]
        public void TypeVisibility_TypeVisibilityShouldBeCorrectlyDefined()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            TypeDef internalType = _assemblyDef.FindType("DocumentationTest", "InternalDocumentedClass");
            TypeDef protectedType = _assemblyDef.FindType("DocumentationTest.PublicDocumentedClass", "ProtectedClass");
            TypeDef privateType = _assemblyDef.FindType("DocumentationTest.PublicDocumentedClass", "PrivateDocumentedClass");
            TypeDef protectedInternalType = _assemblyDef.FindType("DocumentationTest.PublicDocumentedClass", "InternalProtectedClass");

            CheckVisibility(Visibility.Public, publicType.MemberAccess);
            CheckVisibility(Visibility.Internal, internalType.MemberAccess);
            CheckVisibility(Visibility.Protected, protectedType.MemberAccess);
            CheckVisibility(Visibility.InternalProtected, protectedInternalType.MemberAccess);
            CheckVisibility(Visibility.Private, privateType.MemberAccess);
        }

        [Test, Category("Integration")]
        public void MethodVisibility_MethodVisibilityShouldBeCorrectlyDefined()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            List<MethodDef> methods = publicType.GetMethods();

            MethodDef mPrivate = methods.First(c => c.Name == "PrivateMethod");
            MethodDef mProtected = methods.First(c => c.Name == "ProtectedMethod");
            MethodDef mInternalProtected = methods.First(c => c.Name == "ProtectedInternalMethod");
            MethodDef mInternal = methods.First(c => c.Name == "InternalMethod");
            MethodDef mPublic = methods.First(c => c.Name == "PublicMethod");

            CheckVisibility(Visibility.Public, mPublic.MemberAccess);
            CheckVisibility(Visibility.Internal, mInternal.MemberAccess);
            CheckVisibility(Visibility.Protected, mProtected.MemberAccess);
            CheckVisibility(Visibility.InternalProtected, mInternalProtected.MemberAccess);
            CheckVisibility(Visibility.Private, mPrivate.MemberAccess);
        }

        [Test, Category("Integration")]
        public void PropertyVisibility_VisibilityShouldBeCorrectlyDefined()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            List<PropertyDef> properties = publicType.GetProperties();

            PropertyDef mPrivate = properties.First(c => c.Name == "PrivateProperty");
            PropertyDef mProtected = properties.First(c => c.Name == "ProtectedProperty");
            PropertyDef mInternalProtected = properties.First(c => c.Name == "ProtectedInternalProperty");
            PropertyDef mInternal = properties.First(c => c.Name == "InternalProperty");
            PropertyDef mPublic = properties.First(c => c.Name == "PublicProperty");

            CheckVisibility(Visibility.Public, mPublic.MemberAccess);
            CheckVisibility(Visibility.Internal, mInternal.MemberAccess);
            CheckVisibility(Visibility.Protected, mProtected.MemberAccess);
            CheckVisibility(Visibility.InternalProtected, mInternalProtected.MemberAccess);
            CheckVisibility(Visibility.Private, mPrivate.MemberAccess);
        }

        [Test, Category("Integration")]
        public void FieldVisibility_VisibilityShouldBeCorrectlyDefined()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            List<FieldDef> fields = publicType.GetFields();

            FieldDef mPrivate = fields.First(c => c.Name == "PrivateField");
            FieldDef mProtected = fields.First(c => c.Name == "ProtectedField");
            FieldDef mInternalProtected = fields.First(c => c.Name == "ProtectedInternalField");
            FieldDef mInternal = fields.First(c => c.Name == "InternalField");
            FieldDef mPublic = fields.First(c => c.Name == "PublicField");

            CheckVisibility(Visibility.Public, mPublic.MemberAccess);
            CheckVisibility(Visibility.Internal, mInternal.MemberAccess);
            CheckVisibility(Visibility.Protected, mProtected.MemberAccess);
            CheckVisibility(Visibility.InternalProtected, mInternalProtected.MemberAccess);
            CheckVisibility(Visibility.Private, mPrivate.MemberAccess);
        }

        [Test, Category("Integration")]
        public void EventVisibility_VisibilityShouldBeCorrectlyDefined()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            List<EventDef> events = publicType.GetEvents();

            EventDef mPrivate = events.First(c => c.Name == "PrivateEvent");
            EventDef mProtected = events.First(c => c.Name == "ProtectedEvent");
            EventDef mInternalProtected = events.First(c => c.Name == "ProtectedInternalEvent");
            EventDef mInternal = events.First(c => c.Name == "InternalEvent");
            EventDef mPublic = events.First(c => c.Name == "PublicEvent");

            CheckVisibility(Visibility.Public, mPublic.MemberAccess);
            CheckVisibility(Visibility.Internal, mInternal.MemberAccess);
            CheckVisibility(Visibility.Protected, mProtected.MemberAccess);
            CheckVisibility(Visibility.InternalProtected, mInternalProtected.MemberAccess);
            CheckVisibility(Visibility.Private, mPrivate.MemberAccess);
        }

        [Test, Category("Integration")]
        public void Constants_IsConstantShouldBeCorrectlySet()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "Constants");
            List<FieldDef> fields = publicType.GetFields();

            FieldDef aConstant = fields.First(c => c.Name == "PRIVATE_CONST");
            FieldDef aField = fields.First(c => c.Name == "AField");

            Assert.IsTrue(aConstant.IsConstant);
            Assert.IsFalse(aField.IsConstant);
        }

        [Test, Category("Integration")]
        public void TypeWhichContainsTypes_HasTypesAsChildren()
        {
            TypeDef container = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");

            TypeDef child1 = _assemblyDef.FindType("DocumentationTest", "PublicDocumentedClass");
            TypeDef child2 = _assemblyDef.FindType("DocumentationTest", "InternalDocumentedClass");

            // TODO: Verify this as I expected it to contain the parent class but it doesnt currently and we
            //  are only testing current functionality so cant change it even if it is a bug.
            Assert.AreEqual(null, child1.ContainingClass);
            Assert.AreEqual(null, child2.ContainingClass);
        }

        [Test, Category("Integration")]
        public void BaseClass_ShouldHaveBaseClassPopualtedInInheritsFrom()
        {
            TypeDef baseClass = _assemblyDef.FindType("DocumentationTest", "InheritanceTest");
            TypeDef child = _assemblyDef.FindType("DocumentationTest", "InheritanceTest_SecondChild");

            Assert.AreEqual(baseClass, child.InheritsFrom);
        }

        [Test, Category("Integration")]
        public void StaticElements_ShouldHaveStaticAttributeSet()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            List<FieldDef> fields = publicType.GetFields();

            FieldDef staticField = fields.First(c => c.Name == "Field");
            FieldDef noneStaticField = fields.First(c => c.Name == "q");

            Assert.AreEqual(Reflection.Core.COFF.FieldAttributes.Static, staticField.Flags & Reflection.Core.COFF.FieldAttributes.Static);
            Assert.AreEqual(0, (int)(noneStaticField.Flags & Reflection.Core.COFF.FieldAttributes.Static));
        }

        [Test, Category("Integration")]
        public void ReadonlyElements_ShouldHaveInitOnlyAttributeSet()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            List<FieldDef> fields = publicType.GetFields();

            FieldDef testField = fields.First(c => c.Name == "Readonly");
            FieldDef notSetField = fields.First(c => c.Name == "q");

            Assert.AreEqual(Reflection.Core.COFF.FieldAttributes.InitOnly, testField.Flags & Reflection.Core.COFF.FieldAttributes.InitOnly);
            Assert.AreEqual(0, (int)(notSetField.Flags & Reflection.Core.COFF.FieldAttributes.InitOnly));
        }

        [Test, Category("Integration")]
        public void OutParameters_OutIsSet()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            List<MethodDef> methods = publicType.GetMethods();
            List<ParamDef> parameters = methods.First(c => c.Name == "inTest").Parameters;

            ParamDef outParam = parameters[0];
            ParamDef notOutParam = parameters[1];

            Assert.IsTrue(outParam.IsOut);
            Assert.IsFalse(notOutParam.IsOut);
        }

        [Test, Category("Integration")]
        public void OptionalParameters_OptionalFlagIsSet()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            List<MethodDef> methods = publicType.GetMethods();
            List<ParamDef> parameters = methods.First(c => c.Name == "inTest").Parameters;

            ParamDef optParam = parameters[2];
            ParamDef notOptParam = parameters[1];

            Assert.IsTrue(optParam.IsOptional);
            Assert.IsFalse(notOptParam.IsOptional);
        }

        [Test, Category("Integration")]
        public void OptionalParameters_TheAssociatedConstantIsReferenced()
        {
            TypeDef publicType = _assemblyDef.FindType("DocumentationTest", "AllOutputTypesClass");
            List<MethodDef> methods = publicType.GetMethods();
            List<ParamDef> parameters = methods.First(c => c.Name == "inTest").Parameters;

            ParamDef optParam = parameters[2];

            Assert.AreEqual(1, optParam.Constants.Count());
        }

        private void CheckVisibility(Visibility expected, Visibility actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}
