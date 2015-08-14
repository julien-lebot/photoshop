using System.Windows;

namespace PhotoShop
{
    /// <summary>
    /// Interaction logic for NewDocument.xaml
    /// </summary>
    public partial class NewDocument : Window
    {
        public NewDocumentViewModel ViewModel
        {
            get;
            private set;
        }

        public NewDocument()
        {
            ViewModel = new NewDocumentViewModel
            {
                Name = "New Document",
                Width = 256,
                Height = 256
            };
            InitializeComponent();
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsValid())
            {
                return;
            }
            DialogResult = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        // Validate all dependency objects in a window
    }
}
