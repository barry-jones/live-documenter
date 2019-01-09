
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
    using System;
    using System.Collections.Generic;
    using TheBoxSoftware.Reflection;
    using TheBoxSoftware.Reflection.Comments;
    using TheBoxSoftware.Documentation;
    using Reflection.Signatures;

    /// <summary>
    /// Represents an individual result from a search across the <see cref="DocumentedAssembly" />.
    /// </summary>
    internal sealed class SearchResult : IComparable<SearchResult>
    {
        private string _summary;
        private ReflectedMember _member;

        /// <summary>
        /// Initialises a new instance of the SearchResult class.
        /// </summary>
        /// <param name="relatedEntry">The entry related to this result</param>
        public SearchResult(Entry relatedEntry)
        {
            this.RelatedEntry = relatedEntry;

            _member = null;
            if(this.RelatedEntry.Item is List<ReflectedMember>)
            {
                // ignore these are list entries e.g. Properties
            }
            else if(this.RelatedEntry.Item is KeyValuePair<string, List<TypeDef>>)
            {
                // namespace
            }
            else
            {
                _member = (ReflectedMember)this.RelatedEntry.Item;
            }

            if(_member != null)
            {
                if(_member is PropertyDef)
                {
                    PropertyDef property = _member as PropertyDef;

                    string propertyName = new DisplayNameSignitureConvertor(property, false, true).Convert();

                    Name = propertyName + " in " + property.OwningType.GetDisplayName(false);
                }
                else if(_member is TypeDef)
                {
                    this.Name = ((TypeDef)_member).GetDisplayName(false);
                }
                else if(_member is MethodDef)
                {
                    this.Name = string.Format("{1} in {0}", ((MethodDef)_member).Type.GetDisplayName(false), ((MethodDef)_member).GetDisplayName(false, true));
                }
            }

            if(string.IsNullOrEmpty(this.Name))
            {
                this.Name = this.RelatedEntry.Name;
            }
        }

        /// <summary>
        /// Sets the limit for the summary text and adds the "..." section if it is too long.
        /// </summary>
        private void LimitSummary()
        {
            if(!string.IsNullOrEmpty(this._summary) && this._summary.Length > 200)
            {
                this._summary = this._summary.Substring(0, 197) + "...";
            }
        }

        /// <summary>
        /// Icon that represents the type of member.
        /// </summary>
        public string Icon
        {
            get
            {
                if(_member != null)
                {
                    return ElementIconConstants.GetIconPathFor(_member);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// The summary text to display to the user
        /// </summary>
        /// <remarks>
        /// The code for generating the summary has been moved to the property so that it
        /// is not generated for each search result before it is displayed to the user. This
        /// change and the use of a virtualizing stack panel should make the search usable
        /// even with large projects.
        /// </remarks>
        public string Summary
        {
            get
            {
                if(string.IsNullOrEmpty(this._summary))
                {
                    if(this.RelatedEntry.XmlCommentFile != null && this.RelatedEntry.XmlCommentFile.Exists() && this._member != null)
                    {
                        CRefPath crefPath = CRefPath.Create(_member);
                        this._summary = PlainTextSummaryConverter.Convert(_member.Assembly, this.RelatedEntry.XmlCommentFile, crefPath);
                    }

                    if(string.IsNullOrEmpty(this._summary))
                    {
                        this._summary = "No summary.";
                    }

                    this.LimitSummary();
                }

                return this._summary;
            }
        }

        /// <summary>
        /// The name of the result to display to the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fully qualified (includes namespace and type) name of the member.
        /// </summary>
        public string FullyQualifiedName { get; set; }

        /// <summary>
        /// The entry that this search result is about.
        /// </summary>
        public Entry RelatedEntry { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? String.Empty : this.Name;
        }

        public int CompareTo(SearchResult other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}