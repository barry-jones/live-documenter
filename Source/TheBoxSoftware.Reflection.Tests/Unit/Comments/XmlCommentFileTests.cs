
namespace TheBoxSoftware.Reflection.Tests.Unit.Comments
{
    using NUnit.Framework;
    using Moq;
    using Reflection.Comments;

    [TestFixture]
    public class XmlCommentFileTests
    {
        private string _testXmlFilePath = @"source\theboxsoftware.reflection.tests\testfiles\xmlcommentfile.xml";
        private Mock<IFileSystem> _fileSystem;

        private XmlCommentFile CreateXmlCommentFile(string filename)
        {
            _fileSystem = new Mock<IFileSystem>();

            _fileSystem.Setup(p => p.ReadAllBytes(It.IsAny<string>())).Returns(
                System.IO.File.ReadAllBytes(_testXmlFilePath)
                );

            return new XmlCommentFile(filename, _fileSystem.Object);
        }

        [Test]
        public void XmlCommentFile_FileExists_WhenFileIsEmptyString_ReturnsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile(string.Empty);

            bool result = commentFile.Exists();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void XmlCommentFile_Exists_WhenFileNameIsNotEmptyButDoesntExist_ReturnsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("doesnt_exist.dll");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(false);

            bool result = commentFile.Exists();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void XmlCommentFile_Exists_WhenFileNameIsNotEmptyAndExists_ReturnsTrue()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("exists.dll");
            _fileSystem.Setup(p => p.FileExists("exists.dll")).Returns(true);

            bool result = commentFile.Exists();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void XmlCommentFile_Load_WhenFilenameIsEmpty_DoesntTryToLoadFile()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile(string.Empty);

            commentFile.Load();

            _fileSystem.Verify(p => p.ReadAllBytes(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void XmlCommentFile_Load_WhenFileDoesntExist_DoesntTryToLoadFile()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("doesnt_exist.dll");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(false);

            commentFile.Load();

            _fileSystem.Verify(p => p.ReadAllBytes(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void XmlCommentFile_Load_WhenFileExists_Loads()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("exists.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);

            commentFile.Load();

            _fileSystem.Verify(p => p.ReadAllBytes(It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]
        public void XmlCommentFile_IsLoaded_WhenNotLoaded_ReturnsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("test.xml");

            bool result = commentFile.IsLoaded;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void XmlCommentFile_IsLoaded_WhenLoaded_ReturnsTrue()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("test.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            commentFile.Load();

            bool result = commentFile.IsLoaded;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void XmlCommentFile_GetComment_WhenCrefPathIsNull_ReturnsEmptyComment()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);

            commentFile.Load();

            XmlCodeComment result = commentFile.GetComment(null);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetComment_WhenCRefPathIsError_ReturnsEmptyComment()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = new CRefPath();
            crefPath.PathType = CRefTypes.Error;

            commentFile.Load();

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetComment_WhenCalledAndNotLoaded_ReturnsEmptyComment()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = CRefPath.Parse("T:Namespace.TypeName");

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetComment_WhenCRefPathValid_ReturnsComment()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = CRefPath.Parse("T:Namespace.MyType");

            commentFile.Load();

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreNotSame(XmlCodeComment.Empty, result);
            Assert.AreEqual(1, result.Elements.Count);
        }

        [Test]
        public void XmlCommentFile_GetComment_WhenCrefPathValidButNoRecord_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = CRefPath.Parse("T:Nowhere.DoesntExist");

            commentFile.Load();

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetSummary_WhenCRefNull_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);

            commentFile.Load();

            XmlCodeComment result = commentFile.GetSummary(null);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetSummary_WhenCRefPointsToInvalidElement_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = CRefPath.Parse("T:Nothing");

            commentFile.Load();

            XmlCodeComment result = commentFile.GetSummary(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void XmlCommentFile_GetSummary_WhenCRefPointToValidElement_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("Myfile.xml");
            CRefPath crefPath = CRefPath.Parse("T:Namespace.MyType");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);

            commentFile.Load();

            XmlCodeComment result = commentFile.GetSummary(crefPath);

            Assert.AreEqual("Summary text", result.Elements[0].Text);
        }
    }
}
