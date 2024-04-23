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
    public class ImageResizerService
    {
        public bool ResizeImage(string imagePath, int maxWidth, int maxHeight)
        {
            var isResizeSuccessful = false;

            try
            {
                using (var image = Image.Load(imagePath))
                {
                    var newSize = CalculateDimensions(image.Width, image.Height, maxWidth, maxHeight);

                    image.Mutate(x => x.Resize(newSize.Width, newSize.Height));

                    string resizedImagePath = Path.Combine("resizedImages/" + Path.GetFileNameWithoutExtension(imagePath)) + "_thumb" + Path.GetExtension(imagePath);
                    
                    image.Save(resizedImagePath);
                    isResizeSuccessful = true;
                    AddLogMessage($"File size changed to {image.Width}x{image.Height}.", MessageType.Information);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage(ex.Message, MessageType.Error);
                isResizeSuccessful = false;
            }

            return isResizeSuccessful;
        }

        private Size CalculateDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            double aspectRatio = (double)originalWidth / originalHeight;

            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
            {
                return new Size(originalWidth, originalHeight);
            }

            if (originalWidth > maxWidth)
            {
                return new Size(maxWidth, (int)Math.Round(maxWidth / aspectRatio));
            }

            return new Size((int)Math.Round(maxHeight * aspectRatio), maxHeight);
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