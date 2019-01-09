
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using TheBoxSoftware.Reflection;
    using System.Diagnostics;

    internal class NoXmlComments : Paragraph
    {
        private const string HelpUri = "http://livedocumenter.barryjones.me.uk/docs/issues/no-xml-comments";
        public NoXmlComments(ReflectedMember entry)
        {
            this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
            this.Style = (Style)this.FindResource("NoComments");

            this.Inlines.Add(new Run(
                string.Format("No XML comments file found for declaring assembly '{0}'. ", 
                System.IO.Path.GetFileName(entry.Assembly.FileName))
                ));

            Hyperlink info = new Hyperlink(new Run("More information."));
            info.NavigateUri = new Uri(HelpUri);
            info.RequestNavigate += info_RequestNavigate;
            this.Inlines.Add(info);
        }

        void info_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}