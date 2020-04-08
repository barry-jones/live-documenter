
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using Reflection.Core.COFF;
    using NUnit.Framework;

    [TestFixture]
    public class EventDefTests
    {
        private TypeDef _container;

        [Test]
        public void FindAddMethod_WhenNoMethod_ReturnsNull()
        {
            EventDef member = Create();

            MethodDef result = member.FindAddMethod();

            Assert.IsNull(result);
        }

        [Test]
        public void FindAddMethod_WhenHasAdd_FindsMethod()
        {
            EventDef member = Create();

            MethodDef add = CreateEventMethod("add", MethodAttributes.Private);
            _container.Methods.Add(add);

            MethodDef result = member.FindAddMethod();

            Assert.AreSame(add, result);
        }

        [Test]
        public void FindRemoveMethod_WhenNoMethod_ReturnsNull()
        {
            EventDef member = Create();

            MethodDef result = member.FindRemoveMethod();

            Assert.IsNull(result);
        }

        [Test]
        public void FindRemoveMethod_WhenHasAdd_FindsMethod()
        {
            EventDef member = Create();

            MethodDef remove = CreateEventMethod("remove", MethodAttributes.Private);
            _container.Methods.Add(remove);

            MethodDef result = member.FindRemoveMethod();

            Assert.AreSame(remove, result);
        }

        [Test]
        public void MemberAccess_WhenAddIsMoreVisibleThanRemove_ShouldReturnAddsVisibilty()
        {
            MethodDef add = CreateEventMethod("add", MethodAttributes.Public);
            MethodDef remove = CreateEventMethod("remove", MethodAttributes.Private);

            EventDef member = Create();

            _container.Methods.Add(add);
            _container.Methods.Add(remove);
            
            Visibility result = member.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void MemberAccess_WhenRemoveIsMoreVisibleThanAdd_ShouldReturnRemovesVisibilty()
        {
            MethodDef add = CreateEventMethod("add", MethodAttributes.Private);
            MethodDef remove = CreateEventMethod("remove", MethodAttributes.Public);

            EventDef member = Create();

            _container.Methods.Add(add);
            _container.Methods.Add(remove);
            
            Visibility result = member.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        [Test]
        public void MemberAccess_WhenNoRemove_ReturnsAddVisibility()
        {
            MethodDef add = CreateEventMethod("add", MethodAttributes.Private);

            EventDef member = Create();

            _container.Methods.Add(add);

            Visibility result = member.MemberAccess;

            Assert.AreEqual(Visibility.Private, result);
        }

        [Test]
        public void MemberAccess_WhenNoAdd_ReturnsRemoveVisibility()
        {
            MethodDef remove = CreateEventMethod("remove", MethodAttributes.Public);
            EventDef member = Create();

            _container.Methods.Add(remove);

            Visibility result = member.MemberAccess;

            Assert.AreEqual(Visibility.Public, result);
        }

        private MethodDef CreateEventMethod(string addOrRemove, MethodAttributes attributes)
        {
            MethodDef remove = new MethodDef();
            remove.Name = $"{addOrRemove}_EventName";
            remove.MethodAttributes = attributes;
            return remove;
        }

        private EventDef Create()
        {
            _container = new TypeDef();
            AssemblyDef assembly = new AssemblyDef();

            return new EventDef("EventName", assembly, _container);
        }
    }
}
