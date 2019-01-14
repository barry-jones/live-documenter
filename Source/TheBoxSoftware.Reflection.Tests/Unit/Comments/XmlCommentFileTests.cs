
namespace TheBoxSoftware.Reflection.Tests.Unit.Comments
{
    using NUnit.Framework;
    using Moq;
    using Reflection.Comments;

    [TestFixture]
    public class XmlCommentFileTests
    {
        private string _testXmlFilePath = @"testfiles\xmlcommentfile.xml";
        private Mock<IFileSystem> _fileSystem;

        private XmlCommentFile CreateXmlCommentFile(string filename)
        {
            _fileSystem = new Mock<IFileSystem>();

            string dir = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _testXmlFilePath);

            _fileSystem.Setup(p => p.ReadAllBytes(It.IsAny<string>())).Returns(
                System.IO.File.ReadAllBytes(dir)
                );

            return new XmlCommentFile(filename, _fileSystem.Object);
        }

        private void SetFileExistsAndLoadXml(XmlCommentFile commentFile)
        {
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            commentFile.Load();
        }


        [Test]
        public void WhenFilenameIsEmptyString_Exists_IsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile(string.Empty);

            bool result = commentFile.Exists();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenFileDoesntExist_Exists_IsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("doesnt_exist.dll");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(false);

            bool result = commentFile.Exists();

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenFileExists_Exists_IsTrue()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("exists.dll");
            _fileSystem.Setup(p => p.FileExists("exists.dll")).Returns(true);

            bool result = commentFile.Exists();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenExistsIsFalse_Load_IsNotCalled()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile(string.Empty);
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(false);

            commentFile.Load();

            _fileSystem.Verify(p => p.ReadAllBytes(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void WhenFileExists_Load_IsCalled()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("exists.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);

            commentFile.Load();

            _fileSystem.Verify(p => p.ReadAllBytes(It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]
        public void WhenNotLoaded_IsLoaded_IsFalse()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("test.xml");

            bool result = commentFile.IsLoaded;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenLoaded_IsLoaded_IsTrue()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("test.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            commentFile.Load();

            bool result = commentFile.IsLoaded;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenPassedNull_GetComment_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            SetFileExistsAndLoadXml(commentFile);

            XmlCodeComment result = commentFile.GetComment(null);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void WhenCRefIsErrorPath_GetComment_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            SetFileExistsAndLoadXml(commentFile);
            CRefPath crefPath = new CRefPath();
            crefPath.PathType = CRefTypes.Error;

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void WhenNotLoaded_GetComment_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            _fileSystem.Setup(p => p.FileExists(It.IsAny<string>())).Returns(true);
            CRefPath crefPath = CRefPath.Parse("T:Namespace.TypeName");

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void WhenCRefIsValid_GetComment_GetsCorrectComment()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            SetFileExistsAndLoadXml(commentFile);
            CRefPath crefPath = CRefPath.Parse("T:Namespace.MyType");

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreNotSame(XmlCodeComment.Empty, result);
            Assert.AreEqual(1, result.Elements.Count);
        }

        [Test]
        public void WhenCRefValidButNoData_GetComment_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            SetFileExistsAndLoadXml(commentFile);
            CRefPath crefPath = CRefPath.Parse("T:Nowhere.DoesntExist");

            XmlCodeComment result = commentFile.GetComment(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void WhenCRefInvalid_GetSummary_ReturnsEmpty()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("myfile.xml");
            SetFileExistsAndLoadXml(commentFile);
            CRefPath crefPath = CRefPath.Parse("T:Nothing");

            XmlCodeComment result = commentFile.GetSummary(crefPath);

            Assert.AreSame(XmlCodeComment.Empty, result);
        }

        [Test]
        public void WhenCRefValid_GetSummary_ReturnsSummary()
        {
            XmlCommentFile commentFile = CreateXmlCommentFile("Myfile.xml");
            CRefPath crefPath = CRefPath.Parse("T:Namespace.MyType");
            SetFileExistsAndLoadXml(commentFile);

            XmlCodeComment result = commentFile.GetSummary(crefPath);

            Assert.AreEqual("Summary text", result.Elements[0].Text);
        }
    }
}
