using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Project.Configs;

namespace Project.Services.LineValidator
{
    public class LineValidator(IOptions<AppConfig> config) : ILineValidator
    {
        private readonly AppConfig _config = config.Value;
        
        public bool IsBadLine(string line, out string correctLine)
        {
            correctLine = line;
            var isChanged = false;

            if (_config.RegexMasks != null)
                foreach (var mask in _config.RegexMasks)
                    if (Regex.IsMatch(correctLine, $"^{mask.Pattern}$"))
                    {
                        correctLine = Regex.Replace(correctLine, $"^{mask.Pattern}$", mask.Replacement);
                        isChanged = true;
                    }

            var bannedWord = _config.BannedWords?.FirstOrDefault(x => x.Word == line);

            if (bannedWord != null)
            {
                correctLine = bannedWord.Replacement;
                isChanged = true;
            }

            return isChanged;
        }
    }
}
