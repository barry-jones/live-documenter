namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System;
    using TheBoxSoftware.Exporter;
    using TheBoxSoftware.Reflection;

    [TestFixture]
    public class ParametersTests
    {
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
            const Visibility EXPECTED = Visibility.Public;
            string[] input = new string[] { "mylib.dll", "-filters", "public", "-to", "c:\\alocation", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(1, parameters.Filters.Count);
            Assert.AreEqual(EXPECTED, parameters.Filters[0]);
        }

        [Test]
        public void Parameters_WhenMultipleFiltersAreProvided_FiltersAreParsed()
        {
            const Visibility EXPECTED = Visibility.Public;
            string[] input = new string[] { "mylib.dll", "-filters", "public|internal|protected", "-to", "c:\\alocation", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.That(3, Is.EqualTo(parameters.Filters.Count));
            Assert.AreEqual(EXPECTED, parameters.Filters[0]);
        }

        [Test]
        public void Parameters_WhenFiltersIsAll_FiltersAreSpreadToFullList()
        {
            const Visibility EXPECTED = Visibility.Public;
            string[] input = new string[] { "mylib.dll", "-filters", "all", "-to", "c:\\alocation", "-v" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.That(5, Is.EqualTo(parameters.Filters.Count));
            Assert.Contains(EXPECTED, parameters.Filters);
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

        [Test]
        public void Parameters_WhenNoFormatIsProvided_DefaultIsReturned()
        {
            const string EXPECTED = "web-msdn.ldec";

            string[] input = new string[] { "file.dll" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(EXPECTED, parameters.Format);
        }

        [Test]
        public void Parameters_WhenNoFiltersAreProvided_DefaultIsReturned()
        {
            const Visibility EXPECTED = Visibility.Public;

            string[] input = new string[] { "file.dll" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(1, parameters.Filters.Count);
            Assert.AreEqual(EXPECTED, parameters.Filters[0]);
        }

        [Test]
        public void Parameters_WhenManyFiltersAreProvided_AllAreReturned()
        {
            string[] input = new string[] { "file.dll", "-filters", "public|internal" };

            Parameters parameters = new Parameters();
            parameters.Read(input);

            Assert.AreEqual(2, parameters.Filters.Count);
            Assert.That(parameters.Filters, Contains.Item(Visibility.Public));
            Assert.That(parameters.Filters, Contains.Item(Visibility.Internal));
        }

        [Test]
        public void Parameters_WhenInvalidFiltersAreProvided_ExceptionIsThrown()
        {
            string[] input = new string[] { "file.dll", "-filters", "public|internals" };

            Parameters parameters = new Parameters();
            TestDelegate test = () => parameters.Read(input);

            Assert.That(test, Throws.TypeOf<InvalidParameterException>());
        }
    }
}
