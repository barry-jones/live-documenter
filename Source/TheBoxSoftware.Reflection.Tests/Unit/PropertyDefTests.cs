
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Reflection;
    using Reflection.Core.COFF;

    [TestFixture]
    public class PropertyDefTests
    {
        [Test]
        public void MemberAccess_WhenGetterIsMoreVisibleThanSetter_GetterDeterminesVisibility()
        {
            PropertyDef property = CreateWithSetAndGetVisibility(MethodAttributes.Public, MethodAttributes.Private);

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void MemberAccess_WhenSetterIsMoreVisibleThanGetter_SetterDeteriminesVisibility()
        {
            PropertyDef property = CreateWithSetAndGetVisibility(MethodAttributes.Private, MethodAttributes.Public);

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void MemberAccess_WhenNoGetterAndSetterProvided_VisibilityShouldBeNotApplicable()
        {
            PropertyDef property = new PropertyDef();

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.NotApplicable, result);
        }

        [Test]
        public void IsIndexer_WhenGetterHasProperties_IsIndexerReturnsTrue()
        {
            ParamDef parameter = new ParamDef();

            MethodDef getMethod = new MethodDef();
            getMethod.Parameters = new System.Collections.Generic.List<ParamDef>();
            getMethod.Parameters.Add(parameter);

            PropertyDef property = new PropertyDef();
            property.Getter = getMethod;

            bool result = property.IsIndexer;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsIndexer_WhenSetterHasProperties_IsIndexerReturnsTrue()
        {
            ParamDef parameter = new ParamDef();

            MethodDef setMethod = new MethodDef();
            setMethod.Parameters = new System.Collections.Generic.List<ParamDef>();
            setMethod.Parameters.Add(parameter);

            PropertyDef property = new PropertyDef();
            property.Setter = setMethod;

            bool result = property.IsIndexer;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsIndexer_WhenBothGetterAndSetterHaveNoProperties_PropertyIsNotIndexer()
        {
            MethodDef getMethod = new MethodDef();
            MethodDef setMethod = new MethodDef();

            PropertyDef property = new PropertyDef();
            property.Getter = getMethod;
            property.Setter = setMethod;

            bool result = property.IsIndexer;

            Assert.AreEqual(false, result);
        }

        private PropertyDef CreateWithSetAndGetVisibility(MethodAttributes getVisibility, MethodAttributes setVisibility)
        {
            MethodDef getter = new MethodDef();
            getter.MethodAttributes = getVisibility;
            MethodDef setter = new MethodDef();
            setter.MethodAttributes = setVisibility;

            PropertyDef property = new PropertyDef();
            property.Getter = getter;
            property.Setter = setter;

            return property;
        }
    }
}
