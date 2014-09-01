using System;
using System.Text;
using TheBoxSoftware.API.LiveDocumenter;
using System.Collections.Generic;

namespace LD_Documentation.Models
{
    // We want the uri for an entry to be human readable, this will help search engines map
    // these resources to the actual documentation as well. To that end the following structure
    // is wanted:
    //  /documentation/namespace/type/member
    //
    // A member can also be a collection of entries, such as constructors, methods, properties etc. This
    // is a problem because there is no reason why a member can not be named that too.
    // We will diferentiate between these container entries and normal member entries as:
    //  /documentation/namespace/type/property
    //  /documentation/namespace/type/[methods]|[properies]|[etc...]
    //
    // We also have an issue where a name does not dictate that a member of a type should be a property
    // or a field etc. We need to be able to differentiate between actual members/fields/properties etc
    // in the url as well.
    // * perhaps the safename will be ok for these and since they are generated from cref paths they could
    //   be unqiue in a type.

    /// <summary>
    /// Maps a reference to a documentation entry to a URI on the site and vica-versa
    /// </summary>
    public static class UriMapper
    {
        public static string GetUri(long key, string subKey)
        {
            Documentation documentation = DocumentationSingleton.GetSingleton();
            TableOfContents toc = documentation.GetTableOfContents();

            return GetUri(toc.GetDocumentationFor(key, subKey));
        }

        public static string GetUri(ContentEntry forEntry)
        {
            // note: we will need to consider the use of the name parts here as they may not be
            // good for the web and also will not look very nice. GenericClass'T'S is a bit crap.
            // however the display name always works well but cant be used to get a ContentEntry
            // by searching later on... oh dear.

            List<string> pathElements = new List<string>();
            CRefPath cref = forEntry.CRefPath;

            // we do not have namespace containers at the moment so we will always have a
            // namespace as a starting point
            pathElements.Add("/documentation");
            
            // elements may or may not exist we need to determine if it is a container first
            if (forEntry.IsContainer)
            {
                // what is it a container of? The display name should have a bit that indicates what it
                // contains such as methods etc we will just use what these default to.

                // conver the display name which is always a pluralised version to the nameing
                // convention for elment types
                string containerName = string.Empty;
                switch (forEntry.DisplayName.ToLower())
                {
                    case "methods":
                        containerName = "method";
                        break;
                    case "constructors":
                        containerName = "constructor";
                        break;
                    case "properties":
                        containerName = "property";
                        break;
                    case "fields":
                        containerName = "field";
                        break;
                    case "events":
                        containerName = "event";
                        break;
                }
                if (forEntry.Parent != null) // if the parent is null it is a namespace... so many assumptions :/
                {
                    pathElements.Add(forEntry.Parent.CRefPath.Namespace);
                    pathElements.Add(forEntry.Parent.CRefPath.TypeName);  // we always need to add the type name before the container names
                }
                else
                {
                    pathElements.Add(cref.Namespace);
                }
                pathElements.Add(containerName); // todo: we will need to match these up with pathtypes from below somehow...
            }            
            else if (!string.IsNullOrEmpty(cref.ElementName))
            {
                pathElements.Add(cref.Namespace);
                pathElements.Add(cref.TypeName);

                // add a section to the uri
                switch (cref.PathType)
                {
                    case CRefTypes.Event:
                        pathElements.Add("event");
                        break;
                    case CRefTypes.Field:
                        pathElements.Add("field");
                        break;
                    case CRefTypes.Method:
                        pathElements.Add("method");
                        break;
                    case CRefTypes.Property:
                        pathElements.Add("property");
                        break;
                    default: break;
                }

                if (!string.IsNullOrEmpty(cref.Parameters))
                {
                    pathElements.Add(string.Format("{0}{1}", cref.ElementName, cref.Parameters));
                }
                else
                {
                    pathElements.Add(cref.ElementName);
                }
            }
            else if (!string.IsNullOrEmpty(cref.TypeName))
            {
                pathElements.Add(cref.Namespace);
                pathElements.Add(cref.TypeName);
            }

            return string.Join("/", pathElements.ToArray());
        }

        public static ContentEntry GetEntry(string ns, string type, string section, string element)
        {
            Documentation documentation = DocumentationSingleton.GetSingleton();
            TableOfContents toc = documentation.GetTableOfContents();
            StringBuilder sb = new StringBuilder();

            // we need to convert this to a cref path when the elements of the path refer
            // to a member, when they do not we need to get as far in to the table of contents
            // as possible with what we do have.

            // since we have three parts we will start expecting all sections and end work
            // backwords
            if (!string.IsNullOrEmpty(element))
            {
                // the section should determine the path type
                switch ((CRefTypes)Enum.Parse(typeof(CRefTypes), section, true))
                {
                    case CRefTypes.Event:
                        sb.Append("E:");
                        break;
                    case CRefTypes.Field:
                        sb.Append("F:");
                        break;
                    case CRefTypes.Method:
                        sb.Append("M:");
                        break;
                    case CRefTypes.Property:
                        sb.Append("P:");
                        break;
                }
                sb.Append(ns);
                sb.Append(".");
                sb.Append(type);
                sb.Append(".");
                sb.Append(element);

                return toc.GetDocumentationFor(sb.ToString());
            }
            else if (!string.IsNullOrEmpty(type))
            {
                // this includes the search for the section, if we find a type and have a section
                // then we will search the type for the container and return that otherwise just
                // return the type
                sb.Append("T:");
                sb.Append(ns);
                sb.Append(".");
                sb.Append(type);

                ContentEntry found = toc.GetDocumentationFor(sb.ToString());

                if (!string.IsNullOrEmpty(section))
                {
                    string containerName = string.Empty;
                    switch (section.ToLower())
                    {
                        case "method": containerName = "methods"; break;
                        case "property": containerName = "properties"; break;
                        case "constructor": containerName = "constructors"; break;
                        case "field": containerName = "fields"; break;
                        case "event": containerName = "events"; break;
                    }

                    for(int i = 0; i < found.Children.Count; i++)
                    {
                        ContentEntry current = found.Children[i];
                        if(string.Compare(current.DisplayName, containerName, true) == 0)
                        {
                            found = current;
                            break;
                        }
                    }
                }

                return found;
            }
            else if (!string.IsNullOrEmpty(ns))
            {
                return toc.GetDocumentationFor(string.Format("N:{0}", ns));
            }

            return null;
        }
    }
}