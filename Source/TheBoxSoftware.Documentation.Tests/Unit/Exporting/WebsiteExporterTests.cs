
namespace TheBoxSoftware.Documentation.Tests.Unit.Exporting
{
    using Moq;
    using NUnit.Framework;
    using Documentation.Exporting;
    using System.Collections.Generic;

    [TestFixture]
    public class WebsiteExporterTests
    {
        [Test]
        public WebsiteExporter WebsiteExporter_Create()
        {
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            Document document = new Document(new List<DocumentedAssembly>());
            ExportSettings settings = new ExportSettings();
            ExportConfigFile config = new ExportConfigFile("test.config");

            return new WebsiteExporter(document, settings, config, filesystem.Object);
        }
    }
}
