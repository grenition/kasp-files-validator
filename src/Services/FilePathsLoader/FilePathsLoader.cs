namespace Project.Services.FilePathsLoader;

public class FilePathsLoader : IFilePathsLoader
{
    public string[] LoadFilePaths(string directory, string extensionsFilter = "*.*")
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Directory '{directory}' not found.");

        var files = Directory.GetFiles(directory, extensionsFilter, SearchOption.AllDirectories);
        return files;
    }
}