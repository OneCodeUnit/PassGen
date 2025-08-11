using Avalonia.Controls;
using Avalonia.Interactivity;
using PassGenNewUI.Services;
using PassGenNewUI.ViewModels;

namespace PassGenNewUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext ??= new MainWindowViewModel();
        var vm = (MainWindowViewModel)DataContext;
        vm.OptionsChanged += GeneratePassword;
        GeneratePassword();
    }
    
    private void GeneratePassword()
    {
        var vm = (MainWindowViewModel)DataContext;
        var password = PasswordGeneratorService.GeneratePassword(vm.Options);
        if (!string.IsNullOrEmpty(password))
        {
            FieldPassword.Text = password;
        }
    }
    
    private void ButtonGen_Click(object sender, RoutedEventArgs e)
    {
        GeneratePassword();
    }

    private async void ButtonCopy_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(FieldPassword.Text))
        {
            return;
        }
        var topLevel = GetTopLevel(this);
        var clipboard = topLevel?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(FieldPassword.Text);
        }
    }

    private void SizeBar_Scroll(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel)DataContext;
        var passwordLength = (int)SizeBar.Value;
        LabelScrollValue.Text = $"Длина пароля: {passwordLength}";
        vm.Length = passwordLength;
        GeneratePassword();
    }

    private void CheckBox_UseCapital_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.UseCapital = CheckBoxCapital.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_UseLower_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.UseLower = CheckBoxLower.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_UseNumber_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.UseNumber = CheckBoxNumber.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_UseLine_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.UseLine = CheckBoxLine.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_UseSpecial_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.UseSpecial = CheckBoxSpecial.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_ForceEachType_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.ForceEachType = CheckBoxForce.IsChecked ?? false;
        GeneratePassword();
    }
    
    private void CheckBox_ForceReadable_CheckedChanged(object sender, RoutedEventArgs e)
    {
        var vm = (MainWindowViewModel?)DataContext;
        if (vm == null)
        {
            return;
        }
        vm.ForceReadable = CheckBoxReadable.IsChecked ?? false;
        GeneratePassword();
    }
}