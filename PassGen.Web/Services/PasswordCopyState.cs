namespace PassGen.Web.Services;

/// <summary>Связывает результат асинхронного копирования с версией пароля.</summary>
public sealed class PasswordCopyState
{
    private long _version;
    public bool IsCopied { get; private set; }

    public void PasswordChanged()
    {
        _version++;
        ClearIndicator();
    }

    public void ClearIndicator() => IsCopied = false;

    public async Task CopyAsync(Func<Task<bool>> copy)
    {
        long version = _version;
        bool success = await copy();
        if (success && version == _version)
            IsCopied = true;
    }
}
