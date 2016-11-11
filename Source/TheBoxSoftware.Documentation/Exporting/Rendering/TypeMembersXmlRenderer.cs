
namespace TheBoxSoftware.Documentation.Exporting.Rendering
{
    using System.Linq;
    using Reflection;
    using Reflection.Comments;
    using Reflection.Signatures;

    internal sealed class TypeMembersXmlRenderer : XmlRenderer
    {
        private TypeDef _containingType = null;
        private ICommentSource _xmlComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMembersXmlRenderer"/> class.
        /// </summary>
        /// <param name="entry">The associated entry.</param>
        public TypeMembersXmlRenderer(Entry entry)
        {
            _containingType = (TypeDef)entry.Parent.Item;
            _xmlComments = entry.XmlCommentFile;
            AssociatedEntry = entry;
        }

        public override void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("members");
            writer.WriteAttributeString("id", this.AssociatedEntry.Key.ToString());
            writer.WriteAttributeString("subId", this.AssociatedEntry.SubKey);
            WriteCref(this.AssociatedEntry, writer);

            string typeDisplayName = _containingType.GetDisplayName(false);
            string pageDisplayName = $"{typeDisplayName} {this.AssociatedEntry.Name}";
            writer.WriteStartElement("name");
            writer.WriteAttributeString("safename", Exporter.CreateSafeName(pageDisplayName));
            writer.WriteAttributeString("type", typeDisplayName);
            writer.WriteString(pageDisplayName);
            writer.WriteEndElement();

            // we need to write the entries that appear as children to this document map
            // entry. it is going to be easier to use the Entry elements
            writer.WriteStartElement("entries");
            switch (AssociatedEntry.SubKey.ToLower())
            {
                case "properties":
                case "fields":
                case "constants":
                case "operators":
                case "constructors":
                    foreach (Entry current in AssociatedEntry.Children)
                    {
                        ReflectedMember m = current.Item as ReflectedMember;
                        this.WriteEntry(writer, m, ReflectionHelper.GetDisplayName(m));
                    }
                    break;

                case "methods":
                    // write normal methods
                    foreach (Entry current in AssociatedEntry.Children)
                    {
                        ReflectedMember m = current.Item as ReflectedMember;
                        WriteEntry(writer, m, ReflectionHelper.GetDisplayName(m));
                    }
                    // write extension methods
                    var extensionMethods = from method in ((TypeRef)AssociatedEntry.Parent.Item).ExtensionMethods
                                           orderby method.Name
                                           select method;
                    foreach (MethodDef currentMethod in extensionMethods)
                    {
                        DisplayNameSignitureConvertor displayNameSig = new DisplayNameSignitureConvertor(currentMethod, false, true, true);
                        WriteEntry(writer, currentMethod, currentMethod.GetDisplayName(false, true), "extensionmethod");
                    }
                    break;
            }
            writer.WriteEndElement(); // entries
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName, string type)
        {
            CRefPath currentPath = CRefPath.Create(entryMember);
            XmlCodeComment currentComment = _xmlComments.GetComment(currentPath);

            writer.WriteStartElement("entry");
            writer.WriteAttributeString("id", entryMember.GetGloballyUniqueId().ToString());
            writer.WriteAttributeString("subId", string.Empty);
            writer.WriteAttributeString("type", type);
            writer.WriteAttributeString("visibility", ReflectionHelper.GetVisibility(entryMember));
            writer.WriteAttributeString("cref", currentPath.ToString());

            writer.WriteStartElement("name");
            writer.WriteString(displayName);
            writer.WriteEndElement();

            // find and output the summary
            if (currentComment != XmlCodeComment.Empty && currentComment.Elements != null)
            {
                XmlCodeElement summary = currentComment.Elements.Find(currentBlock => currentBlock is SummaryXmlCodeElement);
                if (summary != null)
                {
                    Serialize(summary, writer);
                }
            }
            writer.WriteEndElement();
        }

        private void WriteEntry(System.Xml.XmlWriter writer, ReflectedMember entryMember, string displayName)
        {
            WriteEntry(writer, entryMember, displayName, ReflectionHelper.GetType(entryMember));
        }
    }
}