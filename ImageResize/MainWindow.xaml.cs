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
        string startMessage = "Ready to handle a new image";
        public MainWindow()
        {
            InitializeComponent();

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
            BitmapImage originalImage = new BitmapImage();
            BitmapImage resizedImage = new BitmapImage();
            bool imageIsReadyToResize = false;

            while (!imageIsReadyToResize)
            {
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
                        originalImage = new BitmapImage(new Uri(selectedImagePath));
                        imageIsReadyToResize = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                        // Set path to empty
                        selectedImagePath = string.Empty;
                        StatusTextBlock.Text = startMessage;
                    }
                }
            }

            // Calculate the new dimensions while maintaining aspect ratio
            int targetSize = 512;
            int newWidth, newHeight;
            if (originalImage.Width > originalImage.Height)
            {
                newWidth = targetSize;
                newHeight = (int)(originalImage.Height * ((double)targetSize / originalImage.Width));
            }
            else
            {
                newHeight = targetSize;
                newWidth = (int)(originalImage.Width * ((double)targetSize / originalImage.Height));
            }

            // Resize the image
            resizedImage.BeginInit();
            resizedImage.DecodePixelWidth = newWidth;
            resizedImage.DecodePixelHeight = newHeight;
            resizedImage.UriSource = new Uri(selectedImagePath);
            resizedImage.EndInit();

            // Save the resized image with "_thumb" suffix
            string resizedImagePath = 
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(selectedImagePath),
                System.IO.Path.GetFileNameWithoutExtension(selectedImagePath) + "_thumb" +
                System.IO.Path.GetExtension(selectedImagePath));

            BitmapEncoder encoder = new PngBitmapEncoder(); // You can change the encoder based on your image format
            encoder.Frames.Add(BitmapFrame.Create(resizedImage));
            using (FileStream stream = new FileStream(resizedImagePath, FileMode.Create))
            {
                encoder.Save(stream);
            }

            MessageBox.Show($"Image resized and saved to: {resizedImagePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            StatusTextBlock.Text = startMessage;
        }
    }
}
