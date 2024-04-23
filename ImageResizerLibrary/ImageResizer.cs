using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Processing;

namespace ImageResizerLibrary
{
    public class ImageResizer
    {
        public bool ResizeImage(string imagePath, string outputDirectory, int maxWidth, int maxHeight)
        {
            var isResizeSuccessful = false;
            try
            {
                using (var image = Image.Load(imagePath))
                {
                    var (newWidth, newHeight) = CalculateDimensions(image.Width, image.Height, maxWidth, maxHeight);

                    image.Mutate(x => x.Resize(newWidth, newHeight));

                    string resizedImagePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(imagePath)) + "_thumb" + Path.GetExtension(imagePath);
                    
                    image.Save(resizedImagePath);
                }
            }
            catch (Exception outerException)
            {
                Console.WriteLine(outerException.Message);
            }

            return isResizeSuccessful;
        }

        public (int width, int height) CalculateDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
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
    }
}