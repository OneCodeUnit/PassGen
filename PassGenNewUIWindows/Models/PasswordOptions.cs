namespace PassGenNewUIWindows.Models;

public class PasswordOptions(
    bool useCapital,
    bool useLower,
    bool useNumber,
    bool useLine,
    bool useSpecial,
    bool forceEachType,
    bool forceReadable,
    int length)
{
    public bool UseCapital { get; set; } = useCapital;
    public bool UseLower { get; set; } = useLower;
    public bool UseNumber { get; set; } = useNumber;
    public bool UseLine { get; set; } = useLine;
    public bool UseSpecial { get; set; } = useSpecial;
    public bool ForceEachType { get; set; } = forceEachType;
    public bool ForceReadable { get; set; } = forceReadable;
    public int Length { get; set; } = length;

    public PasswordOptions() : this(true, true, true, false, false, true, true, 12)
    {
    }
}