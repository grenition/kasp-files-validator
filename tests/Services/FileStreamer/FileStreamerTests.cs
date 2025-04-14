namespace Project.Tests.Services.FileStreamer;

public class FileStreamerTests
{
    [Fact]
    public void Test_ShouldChangeLinesInFile()
    {
        var fileStreamer = new Project.Services.FileStreamer.FileStreamer();
        SimpleFileChangeTest(fileStreamer);
    }
    
    [Fact]
    public void Test_ShouldChangeLinesInMultipleFile()
    {
        var fileStreamer = new Project.Services.FileStreamer.FileStreamer();
        SimpleFileChangeTest(fileStreamer);
        SimpleFileChangeTest(fileStreamer);
        SimpleFileChangeTest(fileStreamer);
    }
    
    private void SimpleFileChangeTest(Project.Services.FileStreamer.FileStreamer fileStreamer)
    {
        var sourceFileContent = "Test line 1\nTest line 2\nTest line 3\nTest line 4\nTest line 5\nTest line 6";
        var targetFileContent = "Test line 1\nChanged line 1\nTest line 3\nChanged line 2\nTest line 5\nChanged line 3";

        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var filePath = Path.Combine(tempDir, "test.txt");

        try
        {
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(filePath, sourceFileContent);

            fileStreamer.OpenFile(filePath);

            fileStreamer.GetLine(out var line1);
            fileStreamer.GetLine(out var line2);
            fileStreamer.ModifyPreviousLine("Changed line 1");
            fileStreamer.GetLine(out var line3);
            fileStreamer.GetLine(out var line4);
            fileStreamer.ModifyPreviousLine("Changed line 2");
            fileStreamer.GetLine(out var line5);
            fileStreamer.GetLine(out var line6);
            fileStreamer.ModifyPreviousLine("Changed line 3");

            fileStreamer.CloseFile();

            var outputContent = File.ReadAllText(filePath);

            Assert.Equal(targetFileContent, outputContent);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }
}
