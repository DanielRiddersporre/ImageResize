using Moq;
using ImageResizerLibrary;

namespace ImageResizerTests;

public class Tests
{
    [Test]
    public void AddingLogMessageWithMessageAndType_DoesNotThrowException()
    {
        // Arrange
        var resizerService = new ImageResizerService();

        // Act and Assert
        Assert.DoesNotThrow(() => resizerService.AddLogMessage("Test Message", MessageType.Error));
    }
}