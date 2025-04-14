namespace Project.Configs
{
    public class RegexMask
    {
        public string Pattern { get; set; } = null!;
        public string Replacement { get; set; } = null!;
    }

    public class BannedWord
    {
        public string Word { get; set; } = null!;
        public string Replacement { get; set; } = null!;
    }

    public class AppConfig
    {
        public string? ExtensionFilter { get; set; }

        public List<RegexMask>? RegexMasks { get; set; }

        public List<BannedWord>? BannedWords { get; set; }
    }
}
