
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    internal class Activation : Control
    {
        /// <summary>
        /// Static constructor initialises the default styles associated with this control
        /// </summary>
        static Activation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Activation),
                new FrameworkPropertyMetadata(typeof(Activation)));
        }

        public Activation() : base() { }
        public Activation(Model.Diagram.SequenceDiagram.Activation a)
        {
            this.ActualActivation = a;
        }

        public Model.Diagram.SequenceDiagram.Activation ActualActivation
        {
            get;
            set;
        }
    }
}