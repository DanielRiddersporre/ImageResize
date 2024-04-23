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
        string selectedImagePath;
        readonly ImageResizer imageResizer;
        int maxWidth, maxHeight = 512;
        string startMessage = "Ready to handle a new image..";

        public MainWindow()
        {
            InitializeComponent();

            imageResizer = new ImageResizer();

            StatusTextBlock.Text = startMessage;
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs routedEvent)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;

                // Display the selected image path or perform any other action
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
                    // Load the selected image
                    resizeCompleted = imageResizer.ResizeImage(selectedImagePath, maxWidth, maxHeight);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    // Set path to empty and reset status text
                    selectedImagePath = string.Empty;
                    StatusTextBlock.Text = startMessage;
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

            StatusTextBlock.Text = startMessage;
            selectedImagePath = string.Empty;
        }
    }
}
