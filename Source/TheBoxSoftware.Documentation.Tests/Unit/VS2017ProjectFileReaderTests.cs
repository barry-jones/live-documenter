
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
            create(@"test-files\vs2017_test1.example");
        }

        [Test]
        public void VS2017Project_Test1_DefaultOuputPath_IsCorrect()
        {
            const string ExpectedOutput = @"bin\Debug\netstandard2.0\";
            VS2017ProjectFileReader reader = create(@"test-files\vs2017_test1.example");
            ProjectFileReader.ProjectFileProperties props = reader.ParseProject();

            Assert.That(props.OutputPath, Is.EqualTo(ExpectedOutput));
        }

        [Test]
        public void VS2017Project_Test2_OuputPathWhenBasePathSupplied_IsCorrect()
        {
            const string ExpectedOutput = @"basepath\Debug\netcoreapp2.2\";
            VS2017ProjectFileReader reader = create(@"test-files\vs2017_test2.example");
            ProjectFileReader.ProjectFileProperties props = reader.ParseProject();

            Assert.That(props.OutputPath, Is.EqualTo(ExpectedOutput));
        }

        [Test]
        public void VS2017Project_Test3_WhenOutputPathSupplied_IsCorrect()
        {
            const string ExpectedOutput = @"outputpath\netcoreapp2.2\";
            VS2017ProjectFileReader reader = create(@"test-files\vs2017_test3.example");
            ProjectFileReader.ProjectFileProperties props = reader.ParseProject();

            Assert.That(props.OutputPath, Is.EqualTo(ExpectedOutput));
        }

        private VS2017ProjectFileReader create(string testfile)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(testfile));

            VS2017ProjectFileReader reader = new VS2017ProjectFileReader(doc, "afilename.csproj");
            reader.BuildConfiguration = "Debug";

            return reader;
        }
    }
}
