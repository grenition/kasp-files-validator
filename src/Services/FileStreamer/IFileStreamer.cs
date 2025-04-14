namespace Project.Services.FileStreamer;

public interface IFileStreamer
{
    public void OpenFile(string filePath);
    public bool GetLine(out string line);
    public void ModifyPreviousLine(string newValue);
    public void CloseFile();
}
