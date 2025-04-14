namespace Project.Configs
{
    public class RegexMask
    {
        public string? Pattern { get; set; }
        public string? Replacement { get; set; } 
    }

    public class BannedWord
    {
        public string? Word { get; set; } 
        public string? Replacement { get; set; } 
    }

    public class AppConfig
    {
        public string? ExtensionFilter { get; set; }

        public List<RegexMask>? RegexMasks { get; set; }

        public List<BannedWord>? BannedWords { get; set; }
    }
}
