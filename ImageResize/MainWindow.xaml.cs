using ImageResizerLibrary;
using Microsoft.Win32;
using System;
using System.Windows;

namespace ImageResize;

public partial class MainWindow : Window
{
    private readonly IImageResizerService _imageResizerService;
    
    string selectedImagePath;
    int maxWidth = 512; 
    int maxHeight = 512;

    public MainWindow()
    {
        InitializeComponent();

        _imageResizerService = new ImageResizerService();
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
