
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    internal class Call : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Call));

        /// <summary>
        /// Static constructor initialises the default styles associated with this control
        /// </summary>
        static Call()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Call),
                new FrameworkPropertyMetadata(typeof(Call)));
        }

        public Call() : base() { }
        public Call(Model.Diagram.SequenceDiagram.Call c)
        {
            this.ActualCall = c;
            this.Text = c.Name;
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public Model.Diagram.SequenceDiagram.Call ActualCall
        {
            get;
            set;
        }
    }
}