
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using TheBoxSoftware.Reflection;

    [TestFixture]
    public class PropertyDefTests
    {
        [Test]
        public void WhenGetterIsMoreVisibleThanSetter_GetterDeterminesVisibility()
        {
            MethodDef getter = new MethodDef();
            getter.MethodAttributes = Reflection.Core.COFF.MethodAttributes.Public;
            MethodDef setter = new MethodDef();
            setter.MethodAttributes = Reflection.Core.COFF.MethodAttributes.Private;

            PropertyDef property = new PropertyDef();
            property.Getter = getter;
            property.Setter = setter;

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void WhenSetterIsMoreVisibleThanGetter_SetterDeteriminesVisibility()
        {
            MethodDef getter = new MethodDef();
            getter.MethodAttributes = Reflection.Core.COFF.MethodAttributes.Private;
            MethodDef setter = new MethodDef();
            setter.MethodAttributes = Reflection.Core.COFF.MethodAttributes.Public;

            PropertyDef property = new PropertyDef();
            property.Getter = getter;
            property.Setter = setter;

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void WhenNoGetterAndSetterProvided_VisibilityShouldBeNotApplicable()
        {
            PropertyDef property = new PropertyDef();

            Visibility result = property.MemberAccess;

            Assert.AreEqual(Visibility.NotApplicable, result);
        }
    }
}
