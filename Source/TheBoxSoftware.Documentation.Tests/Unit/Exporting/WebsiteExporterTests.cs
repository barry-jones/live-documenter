
namespace TheBoxSoftware.Documentation.Tests.Unit.Exporting
{
    using NUnit.Framework;
    using Documentation.Exporting;

    [TestFixture]
    public class WebsiteExporterTests
    {
        [Test]
        public void WebsiteExporter_Create()
        {
            WebsiteExporter exporter = new WebsiteExporter(null, null, null);
        }
    }
}
