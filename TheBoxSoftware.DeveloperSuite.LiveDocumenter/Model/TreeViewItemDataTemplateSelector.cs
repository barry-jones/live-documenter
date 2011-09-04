using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	using TheBoxSoftware.Documentation;

	public class TreeViewItemDataTemplateSelector : DataTemplateSelector {
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			Entry current = (Entry)item;
			FrameworkElement element = container as FrameworkElement;

			if (current.Item is Reflection.ReflectedMember && string.IsNullOrEmpty(current.SubKey)) {
				return element.FindResource("documentMapItemTemplate") as DataTemplate;
			}
			else if (current.Item is KeyValuePair<string, List<TheBoxSoftware.Reflection.TypeDef>>) {
				return element.FindResource("documentMapItemTemplate") as DataTemplate;
			}
			else {
				return element.FindResource("documentMapItemNoIconTemplate") as DataTemplate;
			}
		}	
	}
}
