using FluentAssertions;

namespace PassGen.Tests;

/// <summary>
/// Тесты криптографической стойкости и безопасности генератора паролей.
/// </summary>
public class SecurityTests
{
    private readonly PasswordGenerator _generator;

    public SecurityTests()
    {
        _generator = new PasswordGenerator();
    }

    [Fact]
    public void Generate_ProducesUnpredictablePasswords()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: false,
            Readable: false
        );

        // Act - генерируем 1000 паролей
        var passwords = new HashSet<string>();
        for (int i = 0; i < 1000; i++)
        {
            passwords.Add(_generator.Generate(options));
        }

        // Assert - все должны быть уникальными
        passwords.Should().HaveCount(1000, "генератор должен создавать уникальные пароли");
    }

    [Fact]
    public void Generate_WithSameOptions_ProducesDifferentResults()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password1 = _generator.Generate(options);
        string password2 = _generator.Generate(options);

        // Assert
        password1.Should().NotBe(password2, "каждый вызов должен генерировать уникальный пароль");
    }

    [Fact]
    public void Generate_WithLargeAlphabet_HasGoodDistribution()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 64,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);
        var charFrequency = password.GroupBy(c => c)
            .Select(g => g.Count())
            .ToList();

        // Assert - ни один символ не должен встречаться слишком часто
        int maxFrequency = charFrequency.Max();
        maxFrequency.Should().BeLessThan(50, "распределение должно быть достаточно равномерным");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Generate_RespectsAlphabetAndRequiredCategories(bool force)
    {
        var options = new PasswordGenerator.PasswordOptions(6, true, true, true, true, true, force, false);
        const string special = "`~!@#$%^&*()+={}[]\\|:;\"'<>,.?/";
        foreach (var length in new[] { 6, 16, 64 })
        for (int i = 0; i < 100; i++)
        {
            var password = _generator.Generate(options with { Length = length });
            password.Should().HaveLength(length);
            password.ToCharArray().Should().OnlyContain(c => char.IsAsciiLetterOrDigit(c) || "-_".Contains(c) || special.Contains(c));
            if (force)
            {
                password.ToCharArray().Should().Contain(c => char.IsAsciiLetterUpper(c));
                password.ToCharArray().Should().Contain(c => char.IsAsciiLetterLower(c));
                password.ToCharArray().Should().Contain(c => char.IsAsciiDigit(c));
                password.ToCharArray().Should().Contain(c => "-_".Contains(c));
                password.ToCharArray().Should().Contain(c => special.Contains(c));
            }
        }
    }
}