using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AcensiPhotoShop;
using GalaSoft.MvvmLight.Command;
using Patterns.UndoRedo;

namespace PhotoShop
{
    public class MousePositionConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = value as MouseEventArgs;
            var pos = Mouse.GetPosition((IInputElement)args.Source);
            return new Tuple<int, int> ((int)pos.X, (int)pos.Y);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Picture _picture;

        public MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
        }

        private void Save(string fileName, string extension, string filter)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = fileName,
                DefaultExt = extension,
                Filter = filter
            };

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    using (var stream5 = new FileStream(filename, FileMode.Create))
                    {
                        PngBitmapEncoder encoder5 = new PngBitmapEncoder();
                        encoder5.Frames.Add(BitmapFrame.Create(_picture.Bitmap.Clone()));
                        encoder5.Save(stream5);
                        stream5.Close();
                    }
                }
            }
        }

        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _picture != null;
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Save(Title, ".png", "Image files (.png)|*.png");
        }

        private void NewCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var dlog = new NewDocument
            {
                Owner = this
            };
            var result = dlog.ShowDialog();
            if (!result.HasValue || !result.Value)
            {
                return;
            }
            var bmp = new WriteableBitmap(
                dlog.ViewModel.Width,
                dlog.ViewModel.Height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            _picture = new Picture(dlog.ViewModel.Name, bmp);
            Title = _picture.Name;
            ViewModel.Document = new ReversibleDocument<IPicture>(_picture);
            ViewModel.AccessPolicy = new PictureAccessPolicy(_picture);
            _image.Source = bmp;
            _image.Stretch = Stretch.None;
            RenderOptions.SetBitmapScalingMode(_image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(_image, EdgeMode.Aliased);
        }

        #region Undo / Redo
        private void CanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.Undo.CanExecute(sender);
        }

        private void Undo(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.Undo.Execute(sender);
        }

        private void CanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.Redo.CanExecute(sender);
        }

        private void Redo(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.Redo.Execute(sender);
        }
        #endregion

    }

}
