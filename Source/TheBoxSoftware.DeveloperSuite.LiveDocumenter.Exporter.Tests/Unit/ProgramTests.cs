
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using TheBoxSoftware.Exporter;
    using NUnit.Framework;
    using Moq;

    [TestFixture]
    public class ProgramTests
    {
        private Mock<IUserInterface> _ui;
        private Mock<ILog> _log;
        private Mock<IFileSystem> _filesystem;

        [Test]
        public void Program_Create()
        {
            Program p = CreateProgram(new string[] { });
        }

        [Test]
        public void Program_WhenFileSpecifiedDoesntExist_ErrorIsLogged()
        {
            string[] arguments = new string[] { "nonexistentfile" };
            Program p = CreateProgram(arguments);

            _filesystem.Setup(m => m.FileExists(It.IsAny<string>())).Returns(false);

            p.HandleExport();

            _log.Verify(m => m.Log(It.IsRegex("does not exist"), LogType.Error), "Error was not logged");
        }

        [Test]
        public void Program_WhenNoFileSpecified_ErrorIsLogged()
        {
            string[] arguments = new string[] { "" };
            Program p = CreateProgram(arguments);

            p.HandleExport();

            _log.Verify(m => m.Log(It.IsRegex("No file was specified"), LogType.Error), "Error was not logged");
        }
        
        [Test]
        public void Program_IfHelpRequested_HelpIsShown()
        {
            string[] arguments = new string[] { "-h" };
            Program p = CreateProgram(arguments);

            p.HandleExport();

            _log.Verify(m => m.Log(It.IsRegex("show help information")));
        }

        [Test]
        public void Program_IfInvalidParameterProvided_ErrorIsLogged()
        {
            string[] arguments = new string[] { "test.dll", "-filters", "pulsic" };
            Program p = CreateProgram(arguments);

            p.HandleExport();

            _log.Verify(m => m.Log(It.IsRegex("pulsic"), LogType.Error));
        }

        private Program CreateProgram(string[] arguments)
        {
            _ui = new Mock<IUserInterface>();
            _log = new Mock<ILog>();
            _filesystem = new Mock<IFileSystem>();
            
            return new Program(arguments, _filesystem.Object, _ui.Object, _log.Object);
        }
    }
}
