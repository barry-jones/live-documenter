using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBoxSoftware.API.LiveDocumenter;

namespace LD_Documentation.Models
{
    public class DocumentationSingleton
    {
        private static Documentation documentation;
        private static object padlock = new object();
        private static bool isLoaded = false;

        /// <summary>
        /// Retrieves a reference to the single instance of the Documentation.
        /// </summary>
        /// <returns>A reference to the Documentation class.</returns>
        /// <remarks>
        /// THe first call to this method will call the load method on the documentation which
        /// loads the library from disk and processes it. This will cause a delay when the singleton
        /// is requested for the first time.
        /// </remarks>
        public static Documentation GetSingleton()
        {
            if (!isLoaded)
            {
                lock (padlock)
                {
                    string path = HttpContext.Current.Server.MapPath(
                        @"~/documentation/theboxsoftware.api.livedocumenter.dll"
                        );
                    documentation = new Documentation(path);
                    documentation.Load();
                    isLoaded = true;
                }
            }

            return documentation;
        }
    }
}