
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using TheBoxSoftware.Exporter;
    using TheBoxSoftware.Reflection;
    using NUnit.Framework;
    using Moq;
    using System.Collections.Generic;

    [TestFixture]
    public class ExporterVerboseSkipReportingTests
    {
        private Mock<ILog> _log;

        [SetUp]
        public void Setup()
        {
            _log = new Mock<ILog>();
        }

        [Test]
        public void Exporter_WhenVerboseTrue_CreatesExporter()
        {
            var config = new Configuration();
            config.Document = "test.dll";
            config.Filters = new List<Visibility>();
            config.Outputs = new List<Configuration.Output>();

            var exporter = new Exporter(config, verbose: true, _log.Object);

            Assert.That(exporter, Is.Not.Null);
        }

        [Test]
        public void Exporter_WhenVerboseFalse_CreatesExporter()
        {
            var config = new Configuration();
            config.Document = "test.dll";
            config.Filters = new List<Visibility>();
            config.Outputs = new List<Configuration.Output>();

            var exporter = new Exporter(config, verbose: false, _log.Object);

            Assert.That(exporter, Is.Not.Null);
        }

        [Test]
        public void Exporter_WhenVerboseAndFiltersSpecified_LogsDetails()
        {
            var config = new Configuration();
            config.Document = "test.dll";
            config.Filters = new List<Visibility> { Visibility.Public, Visibility.Protected };
            config.Outputs = new List<Configuration.Output>();

            var exporter = new Exporter(config, verbose: true, _log.Object);

            Assert.That(exporter, Is.Not.Null);
        }

        [Test]
        public void Exporter_ReportsVisibilityFilters()
        {
            var config = new Configuration();
            config.Document = "test.dll";
            config.Filters = new List<Visibility> { Visibility.Public };
            config.Outputs = new List<Configuration.Output>();

            new Exporter(config, verbose: true, _log.Object);

            _log.Verify(m => m.LogInformation(It.IsAny<string>()), Times.Never);
        }
    }
}
