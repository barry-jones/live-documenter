using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBoxSoftware.API.LiveDocumenter;


namespace Test.API.LiveDocumenter.Models {
    public static class Docs {
        public static Documentation Get() {
            Documentation documentation = (Documentation)System.Web.HttpContext.Current.Application["documentation"];
            if (documentation == null) {
                documentation = new Documentation(@"C:\Users\Barry\Documents\Current Projects\Live Documenter\The Box Software Developer Suite.sln");
                documentation.Load();

                System.Web.HttpContext.Current.Application["documentation"] = documentation;
            }
            return documentation;
        }
    }
}