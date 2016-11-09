
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System.Windows.Documents;

    /// <summary>
    /// Represents a link to a type or member that is referenced in the
    /// current compilation environment. This relates to the see xml code
    /// comment.
    /// </summary>
    public sealed class See : Hyperlink
    {
        internal See(CrefEntryKey key, string name)
            : base()
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Inlines.Add(new Run(name));
            if(key != null)
            {
                this.Tag = key;
                this.Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
            }
            else
            {
                this.IsEnabled = false;
            }
        }
    }
}