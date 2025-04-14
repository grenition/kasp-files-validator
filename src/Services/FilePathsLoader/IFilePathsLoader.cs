namespace Project.Services.FilePathsLoader;

public interface IFilePathsLoader
{
    public string[] LoadFilePaths(string directory, string extensionsFilter = "*.*");
}
