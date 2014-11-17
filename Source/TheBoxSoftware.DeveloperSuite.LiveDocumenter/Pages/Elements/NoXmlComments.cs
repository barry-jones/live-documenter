using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using TheBoxSoftware.Reflection;
using System.Diagnostics;
using System.Windows.Navigation;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	internal class NoXmlComments : Paragraph {
		public NoXmlComments(ReflectedMember entry) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Style = (Style)this.FindResource("NoComments");

			this.Inlines.Add(new Run(
				string.Format("No XML comments file found for declaring assembly '{0}'. ", System.IO.Path.GetFileName(entry.Assembly.File.FileName))
				));

            Hyperlink info = new Hyperlink(new Run("More information."));
            info.NavigateUri = new Uri("http://documentation.livedocumenter.com/no-xml-comments");
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
