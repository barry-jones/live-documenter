
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Reflection;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ParamDefTests
    {
        [Test]
        public void WhenAttributesStateInParameter_IsIn_IsTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.In);

            bool result = parameter.IsIn;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenAttributesStateNotInParameter_IsIn_IsFalse()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.None);

            bool result = parameter.IsIn;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenAttributesStateOutParameter_IsOut_IsTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.Out);

            bool result = parameter.IsOut;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenAttributesStateNotAnOutParameter_IsOut_IsTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.None);

            bool result = parameter.IsOut;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenAttributesStateOptionalParameter_IsOptional_IsTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.Optional);

            bool result = parameter.IsOptional;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenAttributesStateNotAnOptionalParameter_IsOptional_IsTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.None);

            bool result = parameter.IsOptional;

            Assert.AreEqual(false, result);
        }

        private ParamDef CreateWithSpecifiedAttributes(ParamAttributeFlags flags)
        {
            AssemblyDef assembly = new AssemblyDef();
            return new ParamDef(string.Empty, null, 0, assembly, flags);
        }
    }
}
