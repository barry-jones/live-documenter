using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBoxSoftware.API.LiveDocumenter;
using System.Xml;

namespace Test.API.LiveDocumenter.Controllers
{
    public class LibraryController : Controller
    {
        //
        // GET: /Library/

        public ActionResult Index(long documentationFor) {
            Documentation docs = LiveDocumenter.Models.Docs.Get();
            string document = docs.GetDocumentationFor("N:TheBoxSoftware.API.LiveDocumenter");
            return this.Content(document, "text/xml");
        }
    }
}
