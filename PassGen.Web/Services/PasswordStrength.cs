namespace PassGen.Web.Services;

public enum StrengthLevel
{
    None,
    Weak,
    Medium,
    Strong
}

/// <summary>Оценка стойкости пароля по энтропии (пороги те же, что в README).</summary>
public readonly record struct StrengthInfo(StrengthLevel Level, double Bits)
{
    /// <summary>Энтропия, при которой индикатор заполнен полностью.</summary>
    private const double FullBarBits = 128;

    public double Fraction => Math.Clamp(Bits / FullBarBits, 0, 1);

    public string Label => Level switch
    {
        StrengthLevel.Weak => "Слабый",
        StrengthLevel.Medium => "Средний",
        StrengthLevel.Strong => "Сильный",
        _ => "—"
    };
}

public static class PasswordStrength
{
    private const double WeakBelowBits = 60;
    private const double StrongFromBits = 90;

    /// <summary>Энтропия = длина × log2(размер алфавита).</summary>
    public static StrengthInfo Evaluate(int length, int alphabetSize)
    {
        if (length <= 0 || alphabetSize <= 1)
            return new StrengthInfo(StrengthLevel.None, 0);

        double bits = length * Math.Log2(alphabetSize);
        var level = bits < WeakBelowBits ? StrengthLevel.Weak
            : bits < StrongFromBits ? StrengthLevel.Medium
            : StrengthLevel.Strong;
        return new StrengthInfo(level, bits);
    }
}
