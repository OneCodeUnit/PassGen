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
    [InlineData(6)]
    [InlineData(12)]
    [InlineData(16)]
    [InlineData(32)]
    public void Generate_DoesNotProduceObviousPatterns(int length)
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: length,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert - не должно быть очевидных паттернов
        password.Should().NotMatchRegex("(.)\\1{3,}", "не должно быть 4+ одинаковых символов подряд");
        password.Should().NotBe("aaaaaa", "не должен генерировать повторяющиеся символы");
        password.Should().NotBe("123456", "не должен генерировать последовательности");
        password.Should().NotMatchRegex("^[A-Z]+$|^[a-z]+$|^[0-9]+$", "должно быть разнообразие");
    }

    [Fact]
    public void Generate_WithForceCategories_MaintainsRandomness()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: true,
            Readable: false
        );

        // Act
        var passwords = Enumerable.Range(0, 100)
            .Select(_ => _generator.Generate(options))
            .ToList();

        // Assert - позиции обязательных символов должны различаться
        var firstCharTypes = passwords.Select(p => char.IsUpper(p[0])).Distinct().Count();
        firstCharTypes.Should().BeGreaterThan(1, "первый символ не должен всегда быть из одной категории");
    }
}