
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages
{
    using System.Windows.Documents;

    /// <summary>
    /// A place holder page to display when there is an error displaying a normal members page.
    /// </summary>
    internal class ErrorPage : Page
    {
        /// <summary>
        /// Initialises a new instance of the ErrorPage class.
        /// </summary>
        public ErrorPage()
        {
            this.Blocks.Add(new Elements.Header1("An error occurred"));
            this.Blocks.Add(new Paragraph(
                new Run(
                    "Sorry. An error occurred while trying to display the documentation for the selected member. " +
                    "Please take a moment to send the error details to our support team.")
                ));
        }
    }
}