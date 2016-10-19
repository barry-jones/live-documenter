using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection.Tests.Core.COFF.Unit
{
    [TestFixture]
    public class StringStreamTests
    {
        [Test]
        public void StringStream_WhenCreatedWithZeroLengthArray_ThrowsException()
        {
            byte[] contents = new byte[0];
            Assert.Throws<IndexOutOfRangeException>(delegate() {
                StringStream stream = new StringStream(contents, 0, 0);
                });
        }

        [Test]
        public void StringStream_WhenCreatedFirstCharIsNotNullTerminator_ThrowsException()
        {
            byte[] contents = { 1 };
            Assert.Throws<InvalidOperationException>(delegate () {
                StringStream stream = new StringStream(contents, 0, 1);
            });
        }

        [Test]
        public void StringStream_GetAll_WhenPopulated_ReturnsCorrectNumberOfStrings()
        {
            const int EXPECTED = 15; // includes first empty string
            StringStream stream = CreateStringStream();
            Dictionary<int, string> allStrings = stream.GetAllStrings();

            Assert.AreEqual(EXPECTED, allStrings.Count);
        }

        [Test]
        public void StringStream_GetString_WhenValidStringRequested_ReturnsCorrectString()
        {
            const int POSITION_OF_STRING = 260;
            const string REQUESTED = "thisisateststring";

            StringStream stream = CreateStringStream();

            string returned = stream.GetString(POSITION_OF_STRING);

            Assert.AreEqual(REQUESTED, returned);
        }

        private StringStream CreateStringStream()
        {
            List<byte> data = new List<byte>();
            data.Add(0); // required first null terminating character
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("thisisateststring"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);
            data.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("theboxsoftware.reflection.core.tests"));
            data.Add(0);

            byte[] convertedData = data.ToArray();
            return new StringStream(convertedData, 0, convertedData.Length);
        }
    }
}
