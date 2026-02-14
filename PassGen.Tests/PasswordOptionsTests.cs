using FluentAssertions;

namespace PassGen.Tests;

/// <summary>
/// Тесты для record PasswordOptions.
/// </summary>
public class PasswordOptionsTests
{
    #region Тесты создания

    [Fact]
    public void PasswordOptions_WithValidParameters_CreatesInstance()
    {
        // Act
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Assert
        options.Should().NotBeNull();
        options.Length.Should().Be(16);
        options.IncludeCapital.Should().BeTrue();
        options.IncludeLower.Should().BeTrue();
        options.IncludeNumbers.Should().BeTrue();
        options.IncludeLine.Should().BeFalse();
        options.IncludeSpecial.Should().BeTrue();
        options.ForceCategories.Should().BeTrue();
        options.Readable.Should().BeFalse();
    }

    [Fact]
    public void PasswordOptions_WithAllFalseFlags_CreatesInstance()
    {
        // Act
        var options = new PasswordGenerator.PasswordOptions(
            Length: 12,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Assert
        options.Should().NotBeNull();
        options.Length.Should().Be(12);
        options.IncludeCapital.Should().BeFalse();
        options.IncludeLower.Should().BeFalse();
        options.IncludeNumbers.Should().BeFalse();
        options.IncludeLine.Should().BeFalse();
        options.IncludeSpecial.Should().BeFalse();
        options.ForceCategories.Should().BeFalse();
        options.Readable.Should().BeFalse();
    }

    #endregion

    #region Тесты Value Equality (поведение record)

    [Fact]
    public void PasswordOptions_WithSameValues_AreEqual()
    {
        // Arrange
        var options1 = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        var options2 = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Assert
        options1.Should().Be(options2);
        options1.Equals(options2).Should().BeTrue();
        (options1 == options2).Should().BeTrue();
        options1.GetHashCode().Should().Be(options2.GetHashCode());
    }

    [Fact]
    public void PasswordOptions_WithDifferentLength_AreNotEqual()
    {
        // Arrange
        var options1 = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        var options2 = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Assert
        options1.Should().NotBe(options2);
        options1.Equals(options2).Should().BeFalse();
        (options1 != options2).Should().BeTrue();
    }

    [Fact]
    public void PasswordOptions_WithDifferentFlags_AreNotEqual()
    {
        // Arrange
        var options1 = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        var options2 = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: false, // ← Отличается
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Assert
        options1.Should().NotBe(options2);
    }

    #endregion

    #region Тесты with-выражений (immutability)

    [Fact]
    public void PasswordOptions_WithExpression_CreatesNewInstance()
    {
        // Arrange
        var original = new PasswordGenerator.PasswordOptions(
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
        var modified = original with { Length = 32 };

        // Assert
        original.Length.Should().Be(16, "оригинал не должен измениться");
        modified.Length.Should().Be(32, "новый экземпляр должен иметь новое значение");
        modified.IncludeCapital.Should().Be(original.IncludeCapital);
        modified.IncludeLower.Should().Be(original.IncludeLower);
    }

    [Fact]
    public void PasswordOptions_WithMultipleChanges_CreatesNewInstance()
    {
        // Arrange
        var original = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: false,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Act
        var modified = original with
        {
            Length = 32,
            IncludeNumbers = true,
            IncludeSpecial = true,
            Readable = true
        };

        // Assert
        modified.Length.Should().Be(32);
        modified.IncludeNumbers.Should().BeTrue();
        modified.IncludeSpecial.Should().BeTrue();
        modified.Readable.Should().BeTrue();

        // Оригинальные значения не изменились
        original.Length.Should().Be(16);
        original.IncludeNumbers.Should().BeFalse();
        original.IncludeSpecial.Should().BeFalse();
        original.Readable.Should().BeFalse();
    }

    [Fact]
    public void PasswordOptions_WithNoChanges_CreatesNewButEqualInstance()
    {
        // Arrange
        var original = new PasswordGenerator.PasswordOptions(
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
        var copy = original with { };

        // Assert
        copy.Should().Be(original, "значения идентичны");
        ReferenceEquals(copy, original).Should().BeFalse("но это разные объекты");
    }

    #endregion

    #region Тесты ToString()

    [Fact]
    public void PasswordOptions_ToString_ContainsAllProperties()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Act
        string result = options.ToString();

        // Assert
        result.Should().Contain("Length");
        result.Should().Contain("16");
        result.Should().Contain("IncludeCapital");
        result.Should().Contain("True");
    }

    #endregion

    #region Тесты граничных значений

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
    public void PasswordOptions_WithVariousLengths_StoresValue(int length)
    {
        // Act
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

        // Assert
        options.Length.Should().Be(length, "record должен хранить любое переданное значение");
    }

    #endregion

    #region Тесты комбинаций настроек

    [Fact]
    public void PasswordOptions_AllCategoriesEnabled_StoresCorrectly()
    {
        // Act
        var options = new PasswordGenerator.PasswordOptions(
            Length: 32,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Assert
        options.IncludeCapital.Should().BeTrue();
        options.IncludeLower.Should().BeTrue();
        options.IncludeNumbers.Should().BeTrue();
        options.IncludeLine.Should().BeTrue();
        options.IncludeSpecial.Should().BeTrue();
        options.ForceCategories.Should().BeTrue();
    }

    [Fact]
    public void PasswordOptions_ReadableMode_StoresCorrectly()
    {
        // Act
        var options = new PasswordGenerator.PasswordOptions(
            Length: 20,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: true
        );

        // Assert
        options.Readable.Should().BeTrue();
    }

    [Fact]
    public void PasswordOptions_OnlyOneCategory_StoresCorrectly()
    {
        // Act
        var options = new PasswordGenerator.PasswordOptions(
            Length: 10,
            IncludeCapital: false,
            IncludeLower: false,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Assert
        options.IncludeCapital.Should().BeFalse();
        options.IncludeLower.Should().BeFalse();
        options.IncludeNumbers.Should().BeTrue();
        options.IncludeLine.Should().BeFalse();
        options.IncludeSpecial.Should().BeFalse();
    }

    #endregion

    #region Тесты Deconstruction (если понадобится)

    [Fact]
    public void PasswordOptions_Deconstruction_WorksCorrectly()
    {
        // Arrange
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: false,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Act - Deconstruction (record автоматически поддерживает)
        var (length, capital, lower, numbers, line, special, force, readable) = options;

        // Assert
        length.Should().Be(16);
        capital.Should().BeTrue();
        lower.Should().BeFalse();
        numbers.Should().BeTrue();
        line.Should().BeFalse();
        special.Should().BeTrue();
        force.Should().BeTrue();
        readable.Should().BeFalse();
    }

    #endregion

    #region Тесты использования в коллекциях

    [Fact]
    public void PasswordOptions_InHashSet_WorksWithValueEquality()
    {
        // Arrange
        var options1 = new PasswordGenerator.PasswordOptions(16, true, true, true, false, false, false, false);
        var options2 = new PasswordGenerator.PasswordOptions(16, true, true, true, false, false, false, false);
        var options3 = new PasswordGenerator.PasswordOptions(20, true, true, true, false, false, false, false);

        var hashSet = new HashSet<PasswordGenerator.PasswordOptions> { options1 };

        // Act & Assert
        hashSet.Add(options2).Should().BeFalse("options2 эквивалентен options1");
        hashSet.Add(options3).Should().BeTrue("options3 отличается");
        hashSet.Should().HaveCount(2);
    }

    [Fact]
    public void PasswordOptions_InDictionary_WorksAsKey()
    {
        // Arrange
        var options1 = new PasswordGenerator.PasswordOptions(16, true, true, true, false, false, false, false);
        var options2 = new PasswordGenerator.PasswordOptions(16, true, true, true, false, false, false, false);

        var dictionary = new Dictionary<PasswordGenerator.PasswordOptions, string>
        {
            { options1, "First" }
        };

        // Act
        dictionary[options2] = "Second"; // Должен перезаписать, так как ключи равны

        // Assert
        dictionary.Should().HaveCount(1);
        dictionary[options1].Should().Be("Second");
    }

    #endregion

    #region Тесты создания типичных конфигураций

    [Fact]
    public void PasswordOptions_StrongPasswordConfig_CreatesCorrectly()
    {
        // Act - типичная конфигурация для сильного пароля
        var options = new PasswordGenerator.PasswordOptions(
            Length: 32,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: true,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: false
        );

        // Assert
        options.Length.Should().Be(32);
        options.ForceCategories.Should().BeTrue();

        int enabledCategories = 0;
        if (options.IncludeCapital) enabledCategories++;
        if (options.IncludeLower) enabledCategories++;
        if (options.IncludeNumbers) enabledCategories++;
        if (options.IncludeLine) enabledCategories++;
        if (options.IncludeSpecial) enabledCategories++;

        enabledCategories.Should().Be(5, "все категории должны быть включены");
    }

    [Fact]
    public void PasswordOptions_ReadablePasswordConfig_CreatesCorrectly()
    {
        // Act - типичная конфигурация для читаемого пароля
        var options = new PasswordGenerator.PasswordOptions(
            Length: 16,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: true,
            ForceCategories: true,
            Readable: true
        );

        // Assert
        options.Readable.Should().BeTrue();
        options.ForceCategories.Should().BeTrue();
    }

    [Fact]
    public void PasswordOptions_SimplePasswordConfig_CreatesCorrectly()
    {
        // Act - простой пароль только из букв и цифр
        var options = new PasswordGenerator.PasswordOptions(
            Length: 12,
            IncludeCapital: true,
            IncludeLower: true,
            IncludeNumbers: true,
            IncludeLine: false,
            IncludeSpecial: false,
            ForceCategories: false,
            Readable: false
        );

        // Assert
        options.Length.Should().Be(12);
        options.IncludeSpecial.Should().BeFalse();
        options.IncludeLine.Should().BeFalse();
    }

    #endregion
}