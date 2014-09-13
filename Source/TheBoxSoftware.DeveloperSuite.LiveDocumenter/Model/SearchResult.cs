using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Represents an individual result from a search across the <see cref="LiveDocument.Map" />.
	/// </summary>
	internal sealed class SearchResult : IComparable<SearchResult> {
		private string summary;
		private ReflectedMember member;

		/// <summary>
		/// Initialises a new instance of the SearchResult class.
		/// </summary>
		/// <param name="relatedEntry">The entry related to this result</param>
		public SearchResult(Entry relatedEntry) {
			this.RelatedEntry = relatedEntry;

			member = null;
			if (this.RelatedEntry.Item is List<ReflectedMember>) {
				// ignore these are list entries e.g. Properties
			}
			else if (this.RelatedEntry.Item is KeyValuePair<string, List<TypeDef>>) {
				// namespace
			}
			else {
				member = (ReflectedMember)this.RelatedEntry.Item;
			}

			if (member != null) {
				if (member is PropertyDef) {
					this.Name = string.Format("{1} in {0}", ((PropertyDef)member).Type.GetDisplayName(false), ((PropertyDef)member).GetDisplayName(false, true));
				}
				else if (member is TypeDef) {
					this.Name = ((TypeDef)member).GetDisplayName(false);
				}
				else if (member is MethodDef) {
					this.Name = string.Format("{1} in {0}", ((MethodDef)member).Type.GetDisplayName(false), ((MethodDef)member).GetDisplayName(false, true));
				}
			}

			if (string.IsNullOrEmpty(this.Name)) {
				this.Name = this.RelatedEntry.Name;
			}
		}

		/// <summary>
		/// Sets the limit for the summary text and adds the "..." section if it is too long.
		/// </summary>
		private void LimitSummary() {
			if (!string.IsNullOrEmpty(this.summary) && this.summary.Length > 200) {
				this.summary = this.summary.Substring(0, 197) + "...";
			}
		}

		/// <summary>
		/// Icon that represents the type of member.
		/// </summary>
        public string Icon {
            get {
                if (member != null) {
                    return ElementIconConstants.GetIconPathFor(member);
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
		public string Summary {
			get {
				if (string.IsNullOrEmpty(this.summary)) {
					if (this.RelatedEntry.XmlCommentFile != null && this.RelatedEntry.XmlCommentFile.Exists && this.member != null) {
						CRefPath crefPath = CRefPath.Create(member);
						this.summary = PlainTextSummaryConverter.Convert(member.Assembly, this.RelatedEntry.XmlCommentFile, crefPath);
					}

					if (string.IsNullOrEmpty(this.summary)) {
						this.summary = "No summary.";
					}

					this.LimitSummary();
				}

				return this.summary;
			}
		}

		/// <summary>
		/// The name of the result to display to the user
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// The fully qualified (includes namespace and type) name of the member.
		/// </summary>
		public string FullyQualifiedName {
			get;
			set;
		}

		/// <summary>
		/// The entry that this search result is about.
		/// </summary>
		public Entry RelatedEntry {
			get;
			set;
		}

		public override string ToString() {
			return string.IsNullOrEmpty(this.Name) ? String.Empty : this.Name;
		}

		#region IComparable<SearchResult> Members

		public int CompareTo(SearchResult other) {
			return this.Name.CompareTo(other.Name);
		}

		#endregion
	}
}
