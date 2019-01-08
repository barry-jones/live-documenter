
namespace TheBoxSoftware.Documentation.Tests.Unit
{
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System.IO;
    using System.Xml;
    using TheBoxSoftware.Documentation;

    public class VS2017ProjectFileReaderTests
    {
        [Test]
        public void VS2017Project_Create()
        {
            create();
        }

        [Test]
        public void VS2017Project_Test1_DefaultOuputPath()
        {
            const string ExpectedOutput = @"bin\Debug\netstandard2.0\vs2017_test1.dll";
            VS2017ProjectFileReader reader = create();
            ProjectFileReader.ProjectFileProperties props = reader.ParseProject();

            Assert.That(props.OutputPath, Is.EqualTo(ExpectedOutput));
        }

        private VS2017ProjectFileReader create()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(@"test-files\vs2017_test1.csproj"));

            return new VS2017ProjectFileReader(doc, "afilename.csproj");
        }
    }
}
