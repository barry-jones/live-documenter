using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBoxSoftware.API.LiveDocumenter;

namespace Bug18.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            Documentation docs = Models.Docs.Get();
            string document = docs.GetDocumentationFor("N:TheBoxSoftware.API.LiveDocumenter");
            return this.Content(document, "text/xml");
        }

    }
}
