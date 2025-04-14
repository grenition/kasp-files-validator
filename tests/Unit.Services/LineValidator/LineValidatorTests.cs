using Microsoft.Extensions.DependencyInjection;
using Project.Configs;
using Project.Extensions;
using Project.Services.LineValidator;

namespace Project.Tests.Unit.Services.LineValidator;

public class LineValidatorTests
{
    [Fact]
    public void Test_ShouldMaskPasswordAssignments()
    {
        var lineValidator = GetLineValidator();

        Assert.True(lineValidator.IsBadLine("password=secret123", out var result1));
        Assert.Equal("password= ***PASSWORD***", result1);

        Assert.True(lineValidator.IsBadLine("password : hunter2", out var result2));
        Assert.Equal("password: ***PASSWORD***", result2);

        Assert.True(lineValidator.IsBadLine("password=123456", out var result3));
        Assert.Equal("password= ***PASSWORD***", result3);
        
        Assert.True(lineValidator.IsBadLine(@"password=......\\\\\--==))((", out var result4));
        Assert.Equal("password= ***PASSWORD***", result4);
    }
    
    [Fact]
    public void Test_ShouldNotMaskIncorrectPasswordCases()
    {
        var lineValidator = GetLineValidator();

        Assert.False(lineValidator.IsBadLine("Password=secret123", out var result1));
        Assert.Equal("Password=secret123", result1);

        Assert.False(lineValidator.IsBadLine("my password = 123", out var result2));
        Assert.Equal("my password = 123", result2);

        Assert.False(lineValidator.IsBadLine("pass=123", out var result3));
        Assert.Equal("pass=123", result3);
        
        Assert.False(lineValidator.IsBadLine("password=", out var result4));
        Assert.Equal("password=", result4);
    }
    
    [Fact]
    public void Test_ShouldMaskLicenceKeys()
    {
        var lineValidator = GetLineValidator();

        Assert.True(lineValidator.IsBadLine("ABCDE-12345-FGH67-ZXCVB", out var result1));
        Assert.Equal("***LICENCE KEY***", result1);
        
        Assert.True(lineValidator.IsBadLine("AAAAA-BBBBB-CCCCC-DDDDD", out var result2));
        Assert.Equal("***LICENCE KEY***", result2);
        
        Assert.True(lineValidator.IsBadLine("11111-22222-33333-44444", out var result3));
        Assert.Equal("***LICENCE KEY***", result3);
    }
    
    [Fact]
    public void Test_ShouldNotMaskInvalidLicenceKeys()
    {
        var lineValidator = GetLineValidator();

        Assert.False(lineValidator.IsBadLine("ABCDE-12345-FGH67-ZXCVB-EXTRA", out var result1));
        Assert.Equal("ABCDE-12345-FGH67-ZXCVB-EXTRA", result1);
        
        Assert.False(lineValidator.IsBadLine("abcde-12345-FGH67-ZXCVB", out var result2));
        Assert.Equal("abcde-12345-FGH67-ZXCVB", result2);
        
        Assert.False(lineValidator.IsBadLine("AAAAA-БББББ-CCCCC-DDDDD", out var result3));
        Assert.Equal("AAAAA-БББББ-CCCCC-DDDDD", result3);
    }

    [Fact]
    public void Test_ShouldReplaceBannedWords()
    {
        var lineValidator = GetLineValidator();

        Assert.True(lineValidator.IsBadLine("master", out var result1));
        Assert.Equal("primary", result1);

        Assert.True(lineValidator.IsBadLine("dummy", out var result2));
        Assert.Equal("stub", result2);
    }

    [Fact]
    public void Test_ShouldNotReplaceBannedWords()
    {
        var lineValidator = GetLineValidator();

        Assert.False(lineValidator.IsBadLine("Dummy", out var result1));
        Assert.Equal("Dummy", result1);

        Assert.False(lineValidator.IsBadLine("dummytest", out var result2));
        Assert.Equal("dummytest", result2);
        
        Assert.False(lineValidator.IsBadLine("dummy test", out var result3));
        Assert.Equal("dummy test", result3);
    }

    private ILineValidator GetLineValidator()
    {
        var services = new ServiceCollection();
        services.AddYamlConfiguration(out var configuration);
        services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
        services.AddTransient<ILineValidator, Project.Services.LineValidator.LineValidator>();

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<ILineValidator>();
    }
}
