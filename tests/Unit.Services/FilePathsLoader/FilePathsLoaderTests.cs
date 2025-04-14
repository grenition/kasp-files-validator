namespace Project.Tests.Services.FilePathsLoader;

public class FilePathsLoaderTests
{
    [Fact]
    public void Test_ReturnsOnlyMatchingEtensions()
    {
        const string extensionFilter = "*.log";
        var acceptFilePaths = new[]
        {
            "test1.log",
            "test2.log",
            "sub/test1.log",
            "sub/test2.log",
            "sub/sub2/test1.log",
            "sub/sub2/test2.log"
        };
        var rejectFilePaths = new[]
        {
            "test1.md",
            "test2.png",
            "test3.env",
            "sub/test1.md",
            "sub/test2.png",
            "sub/test3.env",
            "sub/sub2/test3.md"
        };
        var allFilePaths = acceptFilePaths.Concat(rejectFilePaths);
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        try
        {
            Directory.CreateDirectory(tempDir);

            void CreateFile(string relativePath)
            {
                var filePath = Path.Combine(tempDir, relativePath);
                var directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory!);

                File.WriteAllText(filePath, string.Empty);
            }

            foreach (var filePath in allFilePaths)
                CreateFile(filePath);

            var filePathsLoader = new Project.Services.FilePathsLoader.FilePathsLoader();
            var paths = filePathsLoader.LoadFilePaths(tempDir, extensionFilter);

            var expectedPaths = acceptFilePaths.Select(x => Path.Combine(tempDir, x)).ToArray();

            Array.Sort(paths);
            Array.Sort(expectedPaths);

            Assert.Equal(paths.Length, acceptFilePaths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                Assert.Equal(expectedPaths[i], paths[i]);
            }
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }
}
