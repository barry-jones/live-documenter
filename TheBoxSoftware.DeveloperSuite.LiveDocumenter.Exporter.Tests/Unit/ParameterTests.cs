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

        /*
        public void Parameters_WhenValidInput_ReadsValidationFile()
        {
            const string EXPECTED = "testing.csv";
            string[] input = new string[] { "testing.csv", "-with", "config.json" };

            _parameters.Read(input);

            Assert.AreEqual(EXPECTED, _parameters.FileToValidate);
        }

        public void Parameters_WhenCorrectlyConfigured_IsValid()
        {
            const bool EXPECTED_RESULT = true;
            string[] input = new string[] { VALID_INPUTFILE, "-with", VALID_CONFIGFILE };

            _parameters.Read(input);

            bool isValid = _parameters.IsValid();

            Assert.AreEqual(EXPECTED_RESULT, isValid);
        }

        public void Parameters_WhenNoParameters_NoValuesAreSet()
        {
            _parameters.Read(new string[] { });

            Assert.AreEqual(string.Empty, _parameters.Configuration);
            Assert.AreEqual(string.Empty, _parameters.FileToValidate);
        }

        public void Parameters_WhenParameterProvidedWithNoValue_NoValueIsSet()
        {
            ParameterWithNoValue(new string[] { "-with" });
            ParameterWithNoValue(new string[] { "file.csv" });
        }

        public void Parameters_WhenNoValuesProvide_IsNotValid()
        {
            const bool EXPECTED = false;

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        public void Parameters_WhenOnlyValidationFileProvided_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { VALID_INPUTFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        public void Parameters_WhenOnlyConfigurationFile_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-with", VALID_CONFIGFILE });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        public void Parameters_WhenConfigFileDoesntExist_IsNotValid()
        {
            const bool EXPECTED = false;

            _parameters.Read(new string[] { "-with", @"nonexistent-configuration.json" });

            bool result = _parameters.IsValid();

            Assert.AreEqual(EXPECTED, result);
        }

        private void ParameterWithNoValue(string[] parameters)
        {
            const string EXPECTED = "";

            _parameters.Read(parameters);

            Assert.AreEqual(EXPECTED, _parameters.Configuration);
        }
        */
    }
}
