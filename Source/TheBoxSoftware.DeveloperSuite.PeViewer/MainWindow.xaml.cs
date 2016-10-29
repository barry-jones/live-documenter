using System.Windows;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.DeveloperSuite.PEViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model.PEFile _peFile;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initilialises the window for the newly loaded PEFile.
        /// </summary>
        private void InitialiseForNewPEFile()
        {
            this.peViewMap.ItemsSource = _peFile.Entries;
        }

        /// <summary>
        /// Handles the user wanting load an assembly.
        /// </summary>
        /// <param name="sender">Calling object</param>
        /// <param name="e">Event arguments</param>
        private void LoadAssembly_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PeCoffFile coffFile = new PeCoffFile(ofd.FileName);
                coffFile.Initialise();

                _peFile = new Model.PEFile(coffFile);
                this.InitialiseForNewPEFile();
            }
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            TheBoxSoftware.DeveloperSuite.LiveDocumenter.About about = new TheBoxSoftware.DeveloperSuite.LiveDocumenter.About();
            about.ShowDialog();
        }
    }
}
