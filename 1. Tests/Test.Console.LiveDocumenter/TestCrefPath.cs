using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Documentation;
using TheBoxSoftware.Reflection.Comments;

namespace Test.Console.LiveDocumenter
{
    class TestCrefPath
    {
        private Document document;

        public void RunTests(Document document)
        {
            this.document = document;

            this.TestSearch();
        }

        private void TestSearch()
        {
            CRefPath path = CRefPath.Parse("M:theboxsoftware.api.livedocumenter.tableofcontents.getdocumentationfor(system.string)");
            Entry member = this.document.Find(path);
            System.Console.Write(string.Format("[pass] found member {0} from path {1}\n", member.Name, path.ToString()));
        }
    }
}
