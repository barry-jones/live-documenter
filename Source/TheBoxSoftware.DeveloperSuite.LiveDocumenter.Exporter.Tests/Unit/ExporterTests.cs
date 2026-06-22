
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.Unit
{
    using TheBoxSoftware.Exporter;
    using TheBoxSoftware.Reflection;
    using NUnit.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// Tests for the verbose skip-reporting output. These assert the GROUPING SEMANTICS:
    /// a member is skipped when its own visibility is NOT in the allowed filter set, so the
    /// report must be keyed by the EXCLUDED visibilities, never by the allowed ones. The
    /// inversion guard below fails against the original Run 11 code (which iterated the
    /// allowed filters and only ever printed "Public — 0 members excluded.").
    /// </summary>
    [TestFixture]
    public class ExporterVerboseSkipReportingTests
    {
        [Test]
        public void BuildSkipSummaryLines_ReportsExcludedVisibility_NotTheAllowedFilter()
        {
            // filter allows Public only; a Private member was therefore skipped
            List<Visibility> allowed = new List<Visibility> { Visibility.Public };
            Dictionary<Visibility, List<string>> skipped = new Dictionary<Visibility, List<string>>
            {
                { Visibility.Private, new List<string> { "PrivateA", "PrivateB" } }
            };

            List<string> lines = Exporter.BuildSkipSummaryLines(allowed, skipped);

            // the excluded reason (Private) is reported with its real count...
            Assert.That(lines, Has.Some.Contains("Private — 2 members excluded."));
            // ...and the ALLOWED visibility (Public) is never reported as an excluded group
            Assert.That(lines, Has.None.Contains("Public"));
        }

        [Test]
        public void BuildSkipSummaryLines_ZeroExcluded_StillListsEachExcludedVisibility()
        {
            List<Visibility> allowed = new List<Visibility> { Visibility.Public };
            Dictionary<Visibility, List<string>> skipped = new Dictionary<Visibility, List<string>>();

            List<string> lines = Exporter.BuildSkipSummaryLines(allowed, skipped);

            // FORK 3 zero-case: one "0 members excluded" line per excludable visibility
            Assert.That(lines, Has.Some.Contains("Private — 0 members excluded."));
            Assert.That(lines, Has.Some.Contains("Protected — 0 members excluded."));
            Assert.That(lines, Has.Some.Contains("Internal — 0 members excluded."));
            Assert.That(lines, Has.None.Contains("Public"));
        }

        [Test]
        public void BuildSkipSummaryLines_AllowedVisibilityIsNeverAnExcludedGroup()
        {
            // Protected is allowed, so even if (erroneously) collected it must not appear
            List<Visibility> allowed = new List<Visibility> { Visibility.Public, Visibility.Protected };
            Dictionary<Visibility, List<string>> skipped = new Dictionary<Visibility, List<string>>
            {
                { Visibility.Internal, new List<string> { "InternalA" } }
            };

            List<string> lines = Exporter.BuildSkipSummaryLines(allowed, skipped);

            Assert.That(lines, Has.Some.Contains("Internal — 1 members excluded."));
            // Protected is allowed → no line for it. Match on prefix so "InternalProtected" (a
            // different, excluded visibility) does not satisfy a naive substring check.
            Assert.That(lines, Has.None.Matches<string>(line => line.StartsWith("Protected ")));
            Assert.That(lines, Has.None.Matches<string>(line => line.StartsWith("Public ")));
        }

        [Test]
        public void BuildSkipDetailLines_ListsMembersUnderTheirExcludedVisibility()
        {
            List<Visibility> allowed = new List<Visibility> { Visibility.Public };
            Dictionary<Visibility, List<string>> skipped = new Dictionary<Visibility, List<string>>
            {
                { Visibility.Private, new List<string> { "PrivateA" } }
            };

            List<string> lines = Exporter.BuildSkipDetailLines(allowed, skipped);

            Assert.That(lines, Has.Some.Contains("Private:"));
            Assert.That(lines, Has.Some.Contains("PrivateA"));
        }

        [Test]
        public void BuildSkipDetailLines_OmitsExcludedVisibilitiesWithNoMembers()
        {
            List<Visibility> allowed = new List<Visibility> { Visibility.Public };
            Dictionary<Visibility, List<string>> skipped = new Dictionary<Visibility, List<string>>();

            List<string> lines = Exporter.BuildSkipDetailLines(allowed, skipped);

            // detail view is per-member only; empty groups produce no output
            Assert.That(lines, Is.Empty);
        }
    }
}
