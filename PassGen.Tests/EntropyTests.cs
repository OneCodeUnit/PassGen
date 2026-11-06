using PassGen.Web.Services;

namespace PassGen.Tests;

public class EntropyTests
{
    private readonly PasswordGenerator _generator = new();
    private static PasswordGenerator.PasswordOptions Options(int length, bool force = false, bool readable = false) =>
        new(length, true, true, true, true, false, force, readable);

    [Fact]
    public void UnrestrictedEntropyUsesFullAlphabet()
    {
        Assert.Equal(90, _generator.GetEntropy(Options(15)), 10);
        Assert.Equal(StrengthLevel.Strong, PasswordStrength.Evaluate(Options(15)).Level);
    }

    [Fact]
    public void RequiredCategoriesReduceEntropyAndChangeClassification()
    {
        var options = Options(15, force: true);
        Assert.Equal(88.46314, _generator.GetEntropy(options), 5);
        Assert.Equal(StrengthLevel.Medium, PasswordStrength.Evaluate(options).Level);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(64)]
    public void SingleCategoryHasSameEntropyWithOrWithoutRequirement(int length)
    {
        var options = new PasswordGenerator.PasswordOptions(length, false, false, false, true, false, false, false);
        Assert.Equal(length, _generator.GetEntropy(options), 10);
        Assert.Equal(length, _generator.GetEntropy(options with { ForceCategories = true }), 10);
    }

    [Fact]
    public void TwoCategoriesMatchDirectCombinatorialCount()
    {
        var options = new PasswordGenerator.PasswordOptions(6, false, false, true, true, false, true, false);
        double expected = Math.Log2(Math.Pow(12, 6) - Math.Pow(10, 6) - Math.Pow(2, 6));
        Assert.Equal(expected, _generator.GetEntropy(options), 10);
    }

    [Fact]
    public void ReadableModeAndMaximumLengthAreHandled()
    {
        Assert.True(_generator.GetEntropy(Options(16, true, true)) < _generator.GetEntropy(Options(16, true)));
        double bits = _generator.GetEntropy(Options(64, true) with { IncludeSpecial = true });
        Assert.True(double.IsFinite(bits));
        Assert.InRange(bits, 400, 64 * Math.Log2(94));
    }

    [Theory]
    [InlineData(9, StrengthLevel.Weak)]
    [InlineData(10, StrengthLevel.Medium)]
    [InlineData(14, StrengthLevel.Medium)]
    [InlineData(15, StrengthLevel.Strong)]
    public void ClassificationRespects60And90BitBoundaries(int length, StrengthLevel expected) =>
        Assert.Equal(expected, PasswordStrength.Evaluate(Options(length)).Level);

    [Fact]
    public void EmptyAlphabetHasNoStrength()
    {
        var options = new PasswordGenerator.PasswordOptions(16, false, false, false, false, false, true, false);
        Assert.Equal(StrengthLevel.None, PasswordStrength.Evaluate(options).Level);
    }
}
