
namespace TheBoxSoftware.Reflection.Tests.Unit.Signitures
{
    using NUnit.Framework;
    using Reflection.Signitures;

    [TestFixture]
    public class ArrayShapeSignitureTests
    {
        private ArrayShapeSignitureToken Create(byte[] content)
        {
            return new ArrayShapeSignitureToken(content, 0);
        }

        [Test]
        public void ArrayShapeToken_Create_WhenLeastAmountOfInfoProvided_DoesntCrash()
        {
            byte[] content = new byte[]
                {
                    0, // rank
                    0, // size
                    0, // lobounds
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual(0, token.Sizes.Length);
            Assert.AreEqual(0, token.Rank);
            Assert.AreEqual(0, token.LoBounds.Length);
        }

        [Test]
        public void ArrayShapeToken_Create_WhenOneDimensional_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    1, // rank
                    1, // only one size provided
                    100, // size is 100
                    0, // no low bound provided
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual(1, token.Rank);
            Assert.AreEqual(1, token.Sizes.Length);
            Assert.AreEqual(100, token.Sizes[0]);
        }

        [Test]
        public void ArrayShapeToken_Create_WhenOneDimensionalWithLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    1, // rank
                    1, // only one size provided
                    100, // size is 100
                    1, // no low bound provided
                    50 // the lo-bound
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual(1, token.Rank);
            Assert.AreEqual(100, token.Sizes[0]);
            Assert.AreEqual(50, token.LoBounds[0]);
        }

        [Test]
        public void ArrayShapeToken_Create_WhenMultiDimensionalNoLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    3, // rank
                    3, // three sizes provided
                        100, // size is 100
                        75,
                        50,
                    0 // no low bound provided
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual(3, token.Rank);
            Assert.AreEqual(100, token.Sizes[0]);
            Assert.AreEqual(75, token.Sizes[1]);
            Assert.AreEqual(50, token.Sizes[2]);
        }

        [Test]
        public void ArrayShapeToken_Create_WhenMultiDimensionalWithLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    3, // rank
                    3, // three sizes provided
                        100, // size is 100
                        75,
                        50,
                    3, // 3 low bound provided
                        5,
                        10,
                        15
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual(3, token.Rank);
            Assert.AreEqual(100, token.Sizes[0]);
            Assert.AreEqual(75, token.Sizes[1]);
            Assert.AreEqual(50, token.Sizes[2]);
        }

        [Test]
        public void ArrayShapeToken_ToString_WhenLeastAmountOfInfoProvided_OutputsCorrectly()
        {
            byte[] content = new byte[]
                {
                    0, // rank
                    0, // size
                    0, // lobounds
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual("[ArraySize: []]", token.ToString());
        }

        [Test]
        public void ArrayShapeToken_ToString_WhenOneDimensional_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    1, // rank
                    1, // only one size provided
                    100, // size is 100
                    0, // no low bound provided
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual("[ArraySize: [100]]", token.ToString());
        }

        [Test]
        public void ArrayShapeToken_ToString_WhenOneDimensionalWithLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    1, // rank
                    1, // only one size provided
                    100, // size is 100
                    1, // no low bound provided
                    50 // the lo-bound
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual("[ArraySize: [50...100]]", token.ToString());
        }

        [Test]
        public void ArrayShapeToken_ToString_WhenMultiDimensionalNoLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    3, // rank
                    3, // three sizes provided
                        100, // size is 100
                        75,
                        50,
                    0 // no low bound provided
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual("[ArraySize: [100,75,50]]", token.ToString());
        }

        [Test]
        public void ArrayShapeToken_ToString_WhenMultiDimensionalWithLoBounds_ReturnsCorrectly()
        {
            byte[] content = new byte[]
                {
                    3, // rank
                    3, // three sizes provided
                        100, // size is 100
                        75,
                        50,
                    3, // 3 low bound provided
                        5,
                        10,
                        15
                };

            ArrayShapeSignitureToken token = Create(content);

            Assert.AreEqual("[ArraySize: [5...100,10...75,15...50]]", token.ToString());
        }
    }
}
