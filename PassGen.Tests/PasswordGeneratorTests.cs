using FluentAssertions;

namespace PassGen.Tests;

/// <summary>
/// Тесты для класса PasswordGenerator.
/// </summary>
public class PasswordGeneratorTests
{
    private readonly PasswordGenerator _generator;

    public PasswordGeneratorTests()
    {
        _generator = new PasswordGenerator();
    }

    #region Базовая функциональность

    [Fact]
    public void Generate_WithValidOptions_ReturnsPasswordOfCorrectLength()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
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

        // Assert
        password.Should().HaveLength(16);
    }

    [Fact]
    public void Generate_WithAllCategories_GeneratesNonEmptyPassword()
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
        string password = _generator.Generate(options);

        // Assert
        password.Should().NotBeNullOrEmpty();
        password.Should().HaveLength(20);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(12)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(32)]
    [InlineData(64)]
    public void Generate_WithVariousLengths_ReturnsCorrectLength(int length)
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

        // Assert
        password.Should().HaveLength(length);
    }

    #endregion

    #region Тесты категорий символов

    [Fact]
    public void Generate_WithCapitalOnly_ContainsOnlyCapitalLetters()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: true,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().MatchRegex("^[A-Z]+$");
    }

    [Fact]
    public void Generate_WithLowerOnly_ContainsOnlyLowerLetters()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: false,
            IncludeLower: true,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().MatchRegex("^[a-z]+$");
    }

    [Fact]
    public void Generate_WithNumbersOnly_ContainsOnlyDigits()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().MatchRegex("^[0-9]+$");
    }

    [Fact]
    public void Generate_WithLineOnly_ContainsOnlyLineCharacters()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: true,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().MatchRegex("^[-_]+$");
    }

    #endregion

    #region Тесты ForceCategories

    [Fact]
    public void Generate_WithForceCategoriesEnabled_ContainsAllSelectedCategories()
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
        string password = _generator.Generate(options);

        // Assert
        password.Should().HaveLength(20);
        password.Any(c => char.IsUpper(c)).Should().BeTrue();
        password.Any(c => char.IsLower(c)).Should().BeTrue();
        password.Any(c => char.IsDigit(c)).Should().BeTrue();
    }

    [Fact]
    public void Generate_WithForceCategoriesAndAllCategories_ContainsAllTypes()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 30,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().HaveLength(30);
        password.Any(c => char.IsUpper(c)).Should().BeTrue();
        password.Any(c => char.IsLower(c)).Should().BeTrue();
        password.Any(c => char.IsDigit(c)).Should().BeTrue();
        password.Any(c => c == '-' || c == '_').Should().BeTrue();
        password.Any(c => "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/".Contains(c)).Should().BeTrue();
    }

    #endregion

    #region Тесты режима Readable

    [Fact]
    public void Generate_WithReadableMode_DoesNotContainConfusingCharacters()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 50,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: true
        );

        // Act
        string password = _generator.Generate(options);

        // Assert - не должно быть похожих символов
        password.Should().NotContain("0", "ноль легко спутать с O");
        password.Should().NotContain("O", "O легко спутать с 0");
        password.Should().NotContain("1", "единица легко спутать с l и I");
        password.Should().NotContain("l", "l легко спутать с 1 и I");
        password.Should().NotContain("I", "I легко спутать с 1 и l");
    }

    [Fact]
    public void Generate_WithReadableSpecialChars_ContainsOnlySafeSpecialChars()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 30,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: false,
            Readable: true
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().MatchRegex("^[@#$%+=?]+$");
    }

    #endregion

    #region Тесты валидации

    [Fact]
    public void Generate_WithNoCategories_ThrowsArgumentException()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        Action act = () => _generator.Generate(options);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*хотя бы одна категория*")
            .And.ParamName.Should().Be("options");
    }

    [Theory]
    [InlineData(10000)]
    [InlineData(65)]
    [InlineData(5)]
    [InlineData(2)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-6)]
    [InlineData(-7)]
    [InlineData(-64)]
    [InlineData(-65)]
    public void Generate_WithTooShortLength_ThrowsArgumentOutOfRangeException(int length)
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: length,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        Action act = () => _generator.Generate(options);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*6 до 64*");
    }

    #endregion

    #region Граничные случаи

    [Fact]
    public void Generate_WithMinimumLengthWithOneCategory_WorksCorrectly()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 6,
            IncludeCapital: true,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().HaveLength(6);
    }

    [Fact]
    public void Generate_WithMinimumLengthWithAllCategories_WorksCorrectly()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 6,
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

        // Assert
        password.Should().HaveLength(6);
    }

    [Fact]
    public void Generate_WithMaximumLengthWithOneCategory_WorksCorrectly()
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

        // Assert
        password.Should().HaveLength(64);
    }

    [Fact]
    public void Generate_WithMaximumLengthWithAllCategories_WorksCorrectly()
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

        // Assert
        password.Should().HaveLength(64);
    }

    [Fact]
    public void Generate_WithForceCategoriesAtMinimumViableLength_Succeeds()
    {
        // Arrange - 3 категории, длина 6 (минимум)
        var options = new PasswordGenerator.PasswordOptions(
            Length: 6,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Act
        string password = _generator.Generate(options);

        // Assert
        password.Should().HaveLength(6);
        password.Any(c => char.IsUpper(c)).Should().BeTrue();
        password.Any(c => char.IsLower(c)).Should().BeTrue();
        password.Any(c => char.IsDigit(c)).Should().BeTrue();
        password.Any(c => c == '-' || c == '_').Should().BeTrue();
        password.Any(c => "`~!@#$%^&*()+={}[]\\|:;\"\'<>,.?/".Contains(c)).Should().BeTrue();
    }

    #endregion
}