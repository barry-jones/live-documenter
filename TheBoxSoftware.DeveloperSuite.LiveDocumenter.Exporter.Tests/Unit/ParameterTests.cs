namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using NUnit.Framework;
    using TheBoxSoftware.Exporter;

    [TestFixture]
    public class ParametersTests
    {
        const string VALID_CONFIGFILE = @"data\configuration\configuration.json";
        const string VALID_INPUTFILE = @"data\simplefile.csv";

        [Test]
        public void Parameters_Create()
        {
            Parameters parameters = new Parameters();
        }

        [Test]
        public void Parameters_WhenExportFileProvided_ReadsFilename()
        {
            const string EXPECTED = "mylib.dll";
            string[] input = new string[] { "mylib.dll", };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.FileToExport);
        }

        [Test]
        public void Parameters_WhenVerbosityProvided_VerboseSetToTrue()
        {
            const bool EXPECTED = true;
            string[] input = new string[] { "mylib.dll", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.Verbose);
        }

        [Test]
        public void Parameters_WhenForamtProvided_FormatIsRead()
        {
            const string EXPECTED = "web";
            string[] input = new string[] { "mylib.dll", "-v", "-format", "web" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.Format);
        }

        [Test]
        public void Parameters_WhenForamtProvidedWithNoValue_FormatIsEmptyString()
        {
            const string EXPECTED = "";
            string[] input = new string[] { "mylib.dll", "-v", "-format", };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.Format);
        }

        [Test]
        public void Parameters_WhenToProvided_ToIsRead()
        {
            const string EXPECTED = "c:\\alocation";
            string[] input = new string[] { "mylib.dll", "-to", "c:\\alocation", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.To);
        }

        [Test]
        public void Parameters_WhenFiltersProvided_FiltersAreRead()
        {
            const string EXPECTED = "public";
            string[] input = new string[] { "mylib.dll", "-filters", "public", "-to", "c:\\alocation", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.Filters);
        }

        [Test]
        public void Parameters_WheHelpRequested_ShowHelpIsTrue()
        {
            const bool EXPECTED = true;
            string[] input = new string[] { "-h" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.ShowHelp);
        }

        [Test]
        public void Parameters_WhenNoParametersProvided_HasParametersIsFalse()
        {
            const bool EXPECTED = false;
            string[] input = new string[] { };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.HasParameters);
        }

        [Test]
        public void Parameters_WhenParametersProvided_HasParametersIsTrue()
        {
            const bool EXPECTED = true;
            string[] input = new string[] { "-h" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.HasParameters);
        }
    }
}
