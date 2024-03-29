﻿
namespace TheBoxSoftware.Reflection.Tests.Unit.Signatures
{
    using NUnit.Framework;
    using Reflection.Signatures;

    [TestFixture]
    public class CountSignitureTokenTests
    {
        // This is currently using a ushort when the specification states it should be a uint
        // we will test with the current implementation first and change it later as we are only
        // supposed to be adding unit tests in this branch

        // Also it is supposed to be a compressed value and we are currently only converting two
        // bytes in to a short!

        [Test]
        public void WhenSimpleValueProvided_Create_SetsCountToOne()
        {
            byte[] content = new byte[] { 1, 0 }; // little-endian

            CountSignatureToken token = new CountSignatureToken(content, 0);

            Assert.AreEqual(1, token.Count);
        }
    }
}
