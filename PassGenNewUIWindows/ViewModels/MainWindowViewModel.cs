using CommunityToolkit.Mvvm.ComponentModel;
using PassGenNewUIWindows.Models;
using System;

namespace PassGenNewUIWindows.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private PasswordOptions _options = new();

    [ObservableProperty]
    private bool _useCapital;

    [ObservableProperty]
    private bool _useLower;

    [ObservableProperty]
    private bool _useNumber;

    [ObservableProperty]
    private bool _useLine;

    [ObservableProperty]
    private bool _useSpecial;

    [ObservableProperty]
    private bool _forceEachType;

    [ObservableProperty]
    private bool _forceReadable;

    [ObservableProperty]
    private int _length;

    public MainWindowViewModel()
    {
        _useCapital = Options.UseCapital;
        _useLower = Options.UseLower;
        _useNumber = Options.UseNumber;
        _useLine = Options.UseLine;
        _useSpecial = Options.UseSpecial;
        _forceEachType = Options.ForceEachType;
        _forceReadable = Options.ForceReadable;
        _length = Options.Length;
    }

    public event Action? OptionsChanged;

    partial void OnUseCapitalChanged(bool value)
    {
        Options.UseCapital = value;
        OptionsChanged?.Invoke();
    }
    partial void OnUseLowerChanged(bool value)
    {
        Options.UseLower = value;
        OptionsChanged?.Invoke();
    }
    partial void OnUseNumberChanged(bool value)
    {
        Options.UseNumber = value;
        OptionsChanged?.Invoke();
    }
    partial void OnUseLineChanged(bool value)
    {
        Options.UseLine = value;
        OptionsChanged?.Invoke();
    }
    partial void OnUseSpecialChanged(bool value)
    {
        Options.UseSpecial = value;
        OptionsChanged?.Invoke();
    }
    partial void OnForceEachTypeChanged(bool value)
    {
        Options.ForceEachType = value;
        OptionsChanged?.Invoke();
    }
    partial void OnForceReadableChanged(bool value)
    {
        Options.ForceReadable = value;
        OptionsChanged?.Invoke();
    }
    partial void OnLengthChanged(int value)
    {
        Options.Length = value;
        OptionsChanged?.Invoke();
    }
}