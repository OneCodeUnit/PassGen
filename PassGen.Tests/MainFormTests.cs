using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;

namespace PassGen.Tests;

public class MainFormTests
{
    [Fact]
    public void InvalidOptionsDisableCopyAndValidOptionsRestorePassword()
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                using var form = new MainForm();
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                object? Field(string name) => typeof(MainForm).GetField(name, flags)!.GetValue(form);
                foreach (var name in new[] { "CheckBoxCapital", "CheckBoxLower", "CheckBoxNumber", "CheckBoxLine", "CheckBoxSpecial" })
                    ((CheckBox)Field(name)!).Checked = false;

                Assert.Equal("", Field("_password"));
                Assert.False(((Button)Field("ButtonCopy")!).Enabled);
                Assert.False(string.IsNullOrEmpty(((RichTextBox)Field("FieldPassword")!).Text));

                // Оба обработчика должны игнорировать ошибку, не обращаясь к буферу.
                foreach (var name in new[] { "ButtonCopy_Click", "FieldPassword_Click" })
                    typeof(MainForm).GetMethod(name, flags)!.Invoke(form, [form, EventArgs.Empty]);

                ((CheckBox)Field("CheckBoxNumber")!).Checked = true;
                Assert.True(((Button)Field("ButtonCopy")!).Enabled);
                var password = Assert.IsType<string>(Field("_password"));
                Assert.Equal(12, password.Length);
                Assert.All(password, c => Assert.True(char.IsAsciiDigit(c)));
                Assert.Null(Field("_generationError"));
                Assert.Equal(password, ((RichTextBox)Field("FieldPassword")!).Text);
            }
            catch (Exception ex) { failure = ex; }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(20)), "STA form test timed out");
        if (failure is not null) ExceptionDispatchInfo.Capture(failure).Throw();
    }
}
