
namespace TheBoxSoftware.Reflection.Tests.Unit
{
    using NUnit.Framework;
    using Moq;
    using Reflection.Core.COFF;

    [TestFixture]
    public class AssemblyRefTests
    {
        private const int CultureIndex = 1;
        private const int NameIndex = 2;

        [Test]
        public void AssemblyRef_Create_IsInitialisedCorrectlyFromSource()
        {
            Mock<IStringStream> stringStream = new Mock<IStringStream>();
            AssemblyDef assemblyDef = new AssemblyDef();
            AssemblyRefMetadataTableRow row = BuildTestMetadataRow();

            assemblyDef.StringStream = stringStream.Object;

            // setup string methods to return strings in createfrom... method
            stringStream.Setup(p => p.GetString(CultureIndex)).Returns("culture");
            stringStream.Setup(p => p.GetString(NameIndex)).Returns("name");

            AssemblyRef reference = AssemblyRef.CreateFromMetadata(assemblyDef, row);

            Assert.AreEqual("1.4.10.1204", reference.Version.ToString());
            Assert.AreEqual("culture", reference.Culture);
            Assert.AreEqual("name", reference.Name);
            Assert.AreSame(assemblyDef, reference.Assembly);
        }

        private AssemblyRefMetadataTableRow BuildTestMetadataRow()
        {
            AssemblyRefMetadataTableRow row = new AssemblyRefMetadataTableRow();
            row.MajorVersion = 1;
            row.MinorVersion = 4;
            row.BuildNumber = 10;
            row.RevisionNumber = 1204;
            row.Culture = new StringIndex { Value = CultureIndex };
            row.Name = new StringIndex { Value = NameIndex };
            return row;
        }
    }
}
