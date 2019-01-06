
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using TheBoxSoftware.Exporter;
    using NUnit.Framework;
    using Moq;

    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Program_Create()
        {
            Mock<IUserInterface> ui = new Mock<IUserInterface>();
            Mock<ILog> log = new Mock<ILog>();
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            string[] arguments = new string[] { };
            
            Program p = new Program(arguments, filesystem.Object, ui.Object, log.Object);
        }
    }
}
