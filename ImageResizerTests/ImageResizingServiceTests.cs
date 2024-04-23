using Moq;
using ImageResizerLibrary;
using NUnit.Framework.Constraints;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageResizerTests
{
    public class Tests
    {

        [Test]
        public void AddingLogMessageWithMessageAndType_DoesNotThrowException()
        {
            // Arrange
            var mockImageResizerService = new Mock<IImageResizerService>();
            var resizerService = new ImageResizerService(mockImageResizerService.Object);

            // Act and Assert
            Assert.DoesNotThrow(() => resizerService.AddLogMessage("Test Message", MessageType.Error));
        }
    }
}