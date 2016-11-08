
namespace TheBoxSoftware.Reflection.Tests.Unit.Core
{
    using NUnit.Framework;
    using Reflection.Core;

    [TestFixture]
    public class OffsetTests
    {
        [Test]
        public void PeOffset_WhenInitialisedWithAnInt_CurrentIsSetCorrectly()
        {
            Offset offset = 0;

            Assert.AreEqual(0, offset.Current);
        }

        [Test]
        public void PeOffset_WhenOffetIsShifted_ItReturnsTheOffsetBeforeTheShift()
        {
            Offset offset = 10;

            int result = offset.Shift(10);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void PeOffset_WhenOffsetIsShifted_CurrentIsMovedOn()
        {
            Offset offset = 10;

            offset.Shift(10);

            Assert.AreEqual(20, offset.Current);
        }

        [Test]
        public void PeOffSet_WhenConvertedToInt_ConversionIsBasedOnCurrent()
        {
            Offset offset = 5;
            int result = offset;

            Assert.AreEqual(5, result);
        }
    }
}
