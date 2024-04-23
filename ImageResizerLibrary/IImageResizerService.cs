namespace ImageResizerLibrary;

public interface IImageResizerService
{
    bool ResizeImage(string imagePath, int maxWidth, int maxHeight);
    void AddLogMessage(string message, MessageType messageType);
}