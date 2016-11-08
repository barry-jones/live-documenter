
namespace TheBoxSoftware.Documentation.Tests.Unit.Exporting
{
    using NUnit.Framework;
    using Documentation.Exporting;

    [TestFixture]
    public class WebsiteExporterTests
    {
        [Test]
        public void DoNothing()
        {
            WebsiteExporter exporter = new WebsiteExporter(null, null, null);
        }
    }
}
