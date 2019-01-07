
namespace TheBoxSoftware.Documentation.Tests.Unit
{
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System.Collections.Generic;
    using TheBoxSoftware.Documentation;

    public class InputFileReaderTests
    {
        [Test]
        public void InputFileReader_Create()
        {
            new InputFileReader();
        }

        [Test]
        public void InputFileReader_WhenFileIsEmptyString_ThrowsException()
        {
            InputFileReader fileReader = new InputFileReader();

            ActualValueDelegate<List<DocumentedAssembly>> test = 
                () => fileReader.Read(string.Empty, string.Empty);

            Assert.That(test, Throws.ArgumentNullException);
        }

        [Test]
        public void InputFileReader_WhenFileExtensionIsInvalid_ThrowsException()
        {
            const string TestFileName = "invalid.extension";

            InputFileReader fileReader = new InputFileReader();

            ActualValueDelegate<List<DocumentedAssembly>> test =
                () => fileReader.Read(TestFileName, string.Empty);

            Assert.That(test, Throws.ArgumentException);
        }
    }
}
