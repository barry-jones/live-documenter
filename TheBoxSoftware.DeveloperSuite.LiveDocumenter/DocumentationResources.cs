using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Reflection;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter
{
	internal static class DocumentationResources
	{
		#region MergedDictionaries

		public static readonly DependencyProperty MergedDictionariesProperty =
			DependencyProperty.RegisterAttached("MergedDictionaries",
				typeof(string), typeof(DocumentationResources),
				new FrameworkPropertyMetadata((string)null, new PropertyChangedCallback(OnMergedDictionariesChanged))
				);

		public static string GetMergedDictionaries(DependencyObject d) {
			return (string)d.GetValue(MergedDictionariesProperty);
		}

		public static void SetMergedDictionaries(DependencyObject d, string value) {
			d.SetValue(MergedDictionariesProperty, value);
		}

		private static void OnMergedDictionariesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if (!string.IsNullOrEmpty(e.NewValue as string)) {
				foreach (string dictionaryName in (e.NewValue as string).Split(';')) {
					ResourceDictionary dictionary = GetResourceDictionary(dictionaryName);
					if (dictionary != null) {
						if (d is FrameworkElement) {
							(d as FrameworkElement).Resources.MergedDictionaries.Add(dictionary);
						}
						else if (d is FrameworkContentElement) {
							(d as FrameworkContentElement).Resources.MergedDictionaries.Add(dictionary);
						}
					}
				}
			}
		}

		#endregion

		private static ResourceDictionary GetResourceDictionary(string dictionaryName) {
			ResourceDictionary result = null;
			if (sharedDictionaries.ContainsKey(dictionaryName)) {
				result = sharedDictionaries[dictionaryName].Target as ResourceDictionary;
			}
			if (result == null) {
				string assemblyName = System.IO.Path.GetFileNameWithoutExtension(
					Assembly.GetExecutingAssembly().ManifestModule.Name
					);
				result = Application.LoadComponent(
					new Uri(assemblyName + ";component/Resources/" + dictionaryName + ".xaml", UriKind.Relative)
					) as ResourceDictionary;
				sharedDictionaries[dictionaryName] = new WeakReference(result);
			}
			return result;
		}

		public static ResourceDictionary BaseResources {
			get { return DocumentationResources.GetResourceDictionary("DefaultDocumentationStyle"); }
		}

		private static Dictionary<string, WeakReference> sharedDictionaries = new Dictionary<string, WeakReference>();
	}
}
