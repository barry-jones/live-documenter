using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBoxSoftware.API.LiveDocumentor;
using System.Xml;

namespace Test.API.LiveDocumentor.Controllers
{
    public class LibraryController : Controller
    {
        //
        // GET: /Library/

        public ActionResult Index(long documentationFor) {
            Documentation docs = LiveDocumentor.Models.Docs.Get();
            XmlDocument document = docs.GetDocumentationFor(documentationFor);
            return this.Content(document.InnerXml, "text/xml");
        }
    }
}
