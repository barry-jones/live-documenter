
namespace TheBoxSoftware.Documentation.Tests.Unit.Exporting
{
    using TheBoxSoftware.Documentation.Exporting;
    using NUnit.Framework;

    [TestFixture]
    public class ExportConfigFileTests
    {
        [Test]
        public void ExportConfigFile_Create()
        {
            ExportConfigFile config = new ExportConfigFile("testfile.zip");
        }
    }
}
