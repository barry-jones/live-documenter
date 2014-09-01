using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBoxSoftware.API.LiveDocumenter;
using LD_Documentation.Models;
using System.Xml;
using System.Xml.Xsl;
using System.Text;
using System.Text.RegularExpressions;
using Saxon.Api;
using System.IO;

namespace LD_Documentation.Controllers
{
    public class DocumentationController : Controller
    {
        //
        // GET: /Documentation/
        [HttpGet]
        public ActionResult Index(string ns, string type, string section, string element)
        {
            Documentation documentation = DocumentationSingleton.GetSingleton();
            ContentEntry content = Models.UriMapper.GetEntry(ns, type, section, element);

            if (content != null)
            {
                // get the navigation
                DocumentationLink link = new DocumentationLink(content);
                this.ViewData["nav"] = link;

                // get the content
                string xml = documentation.GetDocumentationFor(content);

                this.ViewData["title"] = content.DisplayName;
                this.ViewData["hasComments"] = content.HasComments;

                // temporarily beautify the xml outpout for display
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";
                settings.NewLineChars = "\r\n";
                settings.NewLineHandling = NewLineHandling.Replace;

                MemoryStream sourceDocument = null;
                try
                {
                    // get the input as a stream
                    sourceDocument = new MemoryStream(Encoding.Unicode.GetBytes(xml));

                    Processor p = new Processor();
                    using (FileStream xsltStream = System.IO.File.OpenRead(HttpContext.Server.MapPath("~/Content/webexport.xslt")))
                    {
                        
                        XsltTransformer transform = p.NewXsltCompiler().Compile(xsltStream).Load();

                        // create destination stream
                        StringBuilder output = new StringBuilder();
                        using (XmlWriter writer = XmlWriter.Create(output))
                        {
                            TextWriterDestination destination = new TextWriterDestination(writer);

                            transform.SetInputStream(sourceDocument, new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location));
                            transform.Run(destination);

                            string processedContent = output.ToString();

                            // this xslt produces a tags that have the {key}-{subkey.htm and {key}.htm format, we need
                            // process this output and modify these to work with our output .... more processing :(
                            string regex = @"\b href=""(?<name>[\w._-]*).htm""";
                            Regex r = new Regex(regex, RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);
                            processedContent = r.Replace(processedContent, new MatchEvaluator(this.Evaluator));

                            this.ViewData["content"] = processedContent;
                        }
                    }
                }
                finally
                {
                    if (sourceDocument != null) sourceDocument.Dispose();
                }

                //Processor p = new Processor();
                //using (FileStream xsltStream = System.IO.File.OpenRead(HttpContext.Server.MapPath("~/Content/webexport.xslt")))
                //{
                //    XsltTransformer transform = p.NewXsltCompiler().Compile(xsltStream).Load();
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //    using(XmlWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
                //    {
                //        TextWriterDestination destination = new TextWriterDestination(writer);

                //        using (XmlWriter source = XmlWriter.Create(sb, settings)) {
                //            transform.SetInputStream(source, null);
                //            transform.Run(destination);
                //        }

                //        ms.Seek(0, System.IO.SeekOrigin.Begin);
                //        string processedContent = new System.IO.StreamReader(ms).ReadToEnd();

                //        // this xslt produces a tags that have the {key}-{subkey.htm and {key}.htm format, we need
                //        // process this output and modify these to work with our output .... more processing :(
                //        string regex = @"\b href=""(?<name>[\w._-]*).htm""";
                //        Regex r = new Regex(regex, RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);
                //        processedContent = r.Replace(processedContent, new MatchEvaluator(this.Evaluator));

                //        this.ViewData["content"] = processedContent;
                //    }
                //}

                //XslCompiledTransform transform = new XslCompiledTransform();
                //System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //using(XmlWriter writer = new XmlTextWriter(ms, Encoding.UTF8))
                //{
                //    transform.Load(HttpContext.Server.MapPath("~/Content/webexport.xslt"), new XsltSettings(true, false), new XmlUrlResolver());
                //    transform.Transform(xml, writer);

                //    ms.Seek(0, System.IO.SeekOrigin.Begin);
                //    string processedContent = new System.IO.StreamReader(ms).ReadToEnd();

                //    // this xslt produces a tags that have the {key}-{subkey.htm and {key}.htm format, we need
                //    // process this output and modify these to work with our output .... more processing :(
                //    string regex = @"\b href=""(?<name>[\w._-]*).htm""";
                //    Regex r = new Regex(regex, RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);
                //    processedContent = r.Replace(processedContent, new MatchEvaluator(this.Evaluator));

                //    this.ViewData["content"] = processedContent;
                //}
            }
            else
            {
                this.ViewData["title"] = "Not found";
            }

            return this.View();
        }

        private string Evaluator(Match target)
        {
            // each target has matched a uri e.g. http:\\{key}-{subkey}.htm
            string uri = target.Groups["name"].Value;

            string[] keys = uri.Split('-');

            string ourUri = string.Empty;
            if (keys.Length == 1)
            {
                ourUri = UriMapper.GetUri(long.Parse(keys[0]), string.Empty);
            }
            else
            {
                ourUri = UriMapper.GetUri(long.Parse(keys[0]), keys[1]);
            }

            return string.Format(" href=\"{0}\"", ourUri);
        }
    }
}
