using FluentAssertions;

namespace PassGen.Tests;

/// <summary>
/// Тесты для PasswordGenerator.GetAlphabetSize (используется для оценки энтропии).
/// </summary>
public class AlphabetSizeTests
{
    private readonly PasswordGenerator _generator = new();

    private static PasswordGenerator.PasswordOptions Options(
        bool capital = false, bool lower = false, bool numbers = false,
        bool line = false, bool special = false, bool readable = false) =>
        new(Length: 12, capital, lower, numbers, line, special, ForceCategories: false, readable);

    [Fact]
    public void GetAlphabetSize_WithAllCategories_Returns94()
    {
        var options = Options(capital: true, lower: true, numbers: true, line: true, special: true);

        _generator.GetAlphabetSize(options).Should().Be(94);
    }

    [Fact]
    public void GetAlphabetSize_WithLettersAndDigits_Returns62()
    {
        var options = Options(capital: true, lower: true, numbers: true);

        _generator.GetAlphabetSize(options).Should().Be(62);
    }

    [Fact]
    public void GetAlphabetSize_WithReadableMode_ExcludesConfusingCharacters()
    {
        var options = Options(capital: true, lower: true, numbers: true, readable: true);

        _generator.GetAlphabetSize(options).Should().Be(23 + 21 + 8);
    }

    [Fact]
    public void GetAlphabetSize_WithNoCategories_ReturnsZero()
    {
        _generator.GetAlphabetSize(Options()).Should().Be(0);
    }
}
