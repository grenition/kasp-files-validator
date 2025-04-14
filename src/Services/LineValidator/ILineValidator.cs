namespace Project.Services.LineValidator;

public interface ILineValidator
{
    public bool IsBadLine(string? line, out string? correctLine);
}
