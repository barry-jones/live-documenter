
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using TheBoxSoftware.Documentation;

    internal class TreeViewItemDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Entry current = (Entry)item;
            FrameworkElement element = container as FrameworkElement;

            if(current.Item is Reflection.ReflectedMember && string.IsNullOrEmpty(current.SubKey))
            {
                return element.FindResource("documentMapItemTemplate") as DataTemplate;
            }
            else if(current.Item is KeyValuePair<string, List<TheBoxSoftware.Reflection.TypeDef>>)
            {
                return element.FindResource("documentMapItemTemplate") as DataTemplate;
            }
            else
            {
                return element.FindResource("documentMapItemNoIconTemplate") as DataTemplate;
            }
        }
    }
}