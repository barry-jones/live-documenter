
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Reflection;
    using Reflection.Core.COFF;

    [TestFixture]
    public class ParamDefTests
    {
        [Test]
        public void IsIn_WhenAttributesStateInParameter_ShouldReturnTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.In);

            bool result = parameter.IsIn;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsIn_WhenAttributesStateNotInParameter_ShouldReturnFalse()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.None);

            bool result = parameter.IsIn;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsOut_WhenAttributesStateOutParameter_ShouldReturnTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.Out);

            bool result = parameter.IsOut;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsOut_WhenAttributesStateNotAnOutParameter_ShouldReturnTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.None);

            bool result = parameter.IsOut;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void IsOptional_WhenAttributesStateOptionalParameter_ShouldReturnTrue()
        {
            ParamDef parameter = CreateWithSpecifiedAttributes(ParamAttributeFlags.Optional);

            bool result = parameter.IsOptional;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void IsOptional_WhenAttributesStateNotAnOptionalParameter_ShouldReturnTrue()
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
