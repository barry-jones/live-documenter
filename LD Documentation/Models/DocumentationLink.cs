using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBoxSoftware.API.LiveDocumenter;

namespace LD_Documentation.Models
{
    // simple class that describes a link to some documentation in the API
    public class DocumentationLink
    {
        private string title;
        private string uri;
        private bool isSelected;
        private ContentEntry content = null;
        private TableOfContents toc = null;

        public DocumentationLink(ContentEntry forContent)        
        {
            this.title = forContent.DisplayName;
            this.uri = string.Empty;
            this.content = forContent;
        }

        public DocumentationLink(TableOfContents toc)
        {
            this.title = "Documentation";
            this.uri = string.Empty;
            this.toc = toc;
        }

        public List<DocumentationLink> GetParents()
        {
            List<DocumentationLink> parents = new List<DocumentationLink>();
            if (this.content != null)
            {
                foreach (ContentEntry current in this.content.GetParents())
                {
                    parents.Add(new DocumentationLink(current));
                }
            }
            return parents;
        }

        public List<DocumentationLink> GetChildren()
        {
            List<DocumentationLink> children = new List<DocumentationLink>();
            if (this.content != null)
            {
                for (int i = 0; i < this.content.Children.Count; i++)
                {
                    children.Add(new DocumentationLink(this.content.Children[i]));
                }
            }
            else if (this.toc != null)
            {
                foreach (ContentEntry current in this.toc)
                {
                    children.Add(new DocumentationLink(current));
                }
            }
            return children;
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Uri
        {
            get 
            {
                
                if (this.toc != null)
                {
                    return "/documentation";
                }
                else
                {
                    return UriMapper.GetUri(this.content);
                }
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.isSelected = value; }
        }
    }
}