
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    internal class Object : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Object));

        /// <summary>
        /// Static constructor initialises the default styles associated with this control
        /// </summary>
        static Object()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Object),
                new FrameworkPropertyMetadata(typeof(Object)));
        }

        public Object() : base() { }
        public Object(Model.Diagram.SequenceDiagram.Object o)
            : base()
        {
            this.ActualObject = o;
            this.Text = o.Name;
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public Model.Diagram.SequenceDiagram.Object ActualObject
        {
            get;
            set;
        }
    }
}