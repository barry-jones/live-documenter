
namespace TheBoxSoftware.DeveloperSuite.PEViewer
{
    using System.Windows;
    using TheBoxSoftware.Reflection.Core;

    public partial class MainWindow : Window
    {
        private Model.PEFile _peFile;

        public MainWindow()
        {
            InitializeComponent();
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
                LoadAndInitialiseAssembly(ofd.FileName);
            }
        }

        private void LoadAndInitialiseAssembly(string filename)
        {
            PeCoffFile coffFile = new PeCoffFile(filename, new FileSystem());
            coffFile.Initialise();

            _peFile = new Model.PEFile(coffFile);
            InitialiseForNewPEFile();
        }

        private void ShowAboutDialog(object sender, RoutedEventArgs e)
        {
            LiveDocumenter.About about = new LiveDocumenter.About();
            about.ShowDialog();
        }

        /// <summary>
        /// Initilialises the window for the newly loaded PEFile.
        /// </summary>
        private void InitialiseForNewPEFile()
        {
            peViewMap.ItemsSource = _peFile.Entries;
        }
    }
}
