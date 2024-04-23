using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Processing;

namespace ImageResizerLibrary
{
    public class ImageResizer
    {
        public bool ResizeImage(string imagePath, int maxWidth, int maxHeight)
        {
            var isResizeSuccessful = false;

            try
            {
                using (var image = Image.Load(imagePath))
                {
                    var (newWidth, newHeight) = CalculateDimensions(image.Width, image.Height, maxWidth, maxHeight);

                    image.Mutate(x => x.Resize(newWidth, newHeight));

                    string resizedImagePath = Path.Combine(Path.GetFileNameWithoutExtension(imagePath)) + "_thumb" + Path.GetExtension(imagePath);
                    
                    image.Save(resizedImagePath);
                    isResizeSuccessful = true;
                    AddLogMessage($"File size changed to {newWidth} and {newHeight}.", MessageType.Information);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage(ex.Message, MessageType.Error);
                isResizeSuccessful = false;
            }

            return isResizeSuccessful;
        }

        private (int width, int height) CalculateDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            int newWidth, newHeight;
            double aspectRatio = originalWidth / originalHeight;

            if(originalWidth > maxWidth)
            {
                if(aspectRatio > 1)
                {
                    newWidth = maxWidth;
                    newHeight = (int)Math.Round(maxWidth * aspectRatio);
                }
                else
                {
                    newWidth = (int)Math.Round(maxHeight * aspectRatio);
                    newHeight = maxHeight;
                }
            }
            else
            {
                newWidth = originalWidth;
                newHeight = originalHeight;
            }

            return (newWidth, newHeight);
        }

        public void AddLogMessage(string message, MessageType messageType)
        {
            var resizeLogPath = "log/resizelog.txt";

            if (!File.Exists(resizeLogPath))
            {
                File.Create(resizeLogPath).Dispose();
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(resizeLogPath)
                .CreateLogger();

            switch (messageType)
            {
                case MessageType.Information:
                    Log.Information(message); 
                    break;
                case MessageType.Debug:
                    Log.Debug(message);
                    break;
                case MessageType.Error:
                    Log.Error(message);
                    break;
            }
        }

        
    }
}