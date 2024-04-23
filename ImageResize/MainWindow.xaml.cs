using ImageResizerLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageResize
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IImageResizerService _imageResizerService;
        
        string selectedImagePath;
        int maxWidth = 512; 
        int maxHeight = 512;

        public MainWindow()
        {
            InitializeComponent();

            _imageResizerService = new ImageResizerService(_imageResizerService);
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                StatusTextBlock.Text = $"Selected Image: {selectedImagePath}";
            }
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            bool resizeCompleted = false;

            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Please select an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    resizeCompleted = _imageResizerService.ResizeImage(selectedImagePath, maxWidth, maxHeight);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong, check the log file for detailed information!");
                    _imageResizerService.AddLogMessage(ex.Message, MessageType.Error); ;
                    selectedImagePath = string.Empty;
                }
            }

            if (resizeCompleted)
            {
                MessageBox.Show($"Image resized and saved to: {selectedImagePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Something went wrong during the resizing process. Check the log file for more information!");
            }

            selectedImagePath = string.Empty;
            StatusTextBlock.Text = string.Empty;
        }
    }
}
