using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Represents an individual result from a search across the
	/// <see cref="LiveDocument.DocumentMap" />.
	/// </summary>
	internal sealed class SearchResult {
		private string summary;
		private ReflectedMember member;

		/// <summary>
		/// Initialises a new instance of the SearchResult class.
		/// </summary>
		/// <param name="relatedEntry">The entry related to this result</param>
		public SearchResult(Entry relatedEntry) {
			this.RelatedEntry = relatedEntry;

			try {
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
						this.Name = new DisplayNameSignitureConvertor(
							(PropertyDef)member, false, true
							).Convert();
					}
					else if (member is TypeDef) {
						this.Name = new DisplayNameSignitureConvertor(
							(TypeDef)member, false
							).Convert();
					}
					else if (member is MethodDef) {
						this.Name = new DisplayNameSignitureConvertor(
							(MethodDef)member, false, true
							).Convert();
					}
				}

				if (string.IsNullOrEmpty(this.Name)) {
					this.Name = this.RelatedEntry.Name;
				}
			}
			catch (Exception ex) {
				int x = 0;
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
					if (this.RelatedEntry.HasXmlComments && this.member != null) {
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
		/// The entry that this search result is about.
		/// </summary>
		public Entry RelatedEntry {
			get;
			set;
		}

		public override string ToString() {
			return string.IsNullOrEmpty(this.RelatedEntry.Name) ? String.Empty : this.RelatedEntry.Name;
		}
	}
}
