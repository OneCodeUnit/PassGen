using PassGen.Web.Services;

namespace PassGen.Tests;

public class PasswordCopyStateTests
{
    [Fact]
    public async Task CompletedCopyMarksCurrentPassword()
    {
        var state = new PasswordCopyState();
        await state.CopyAsync(() => Task.FromResult(true));
        Assert.True(state.IsCopied);
        state.PasswordChanged();
        Assert.False(state.IsCopied);
    }

    [Fact]
    public async Task DelayedCopyDoesNotMarkReplacementPassword()
    {
        var state = new PasswordCopyState();
        var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var copy = state.CopyAsync(() => completion.Task);
        state.PasswordChanged();
        completion.SetResult(true);
        await copy;
        Assert.False(state.IsCopied);
    }

    [Fact]
    public async Task FailedCopyDoesNotMarkPassword()
    {
        var state = new PasswordCopyState();
        await state.CopyAsync(() => Task.FromResult(false));
        Assert.False(state.IsCopied);
    }

    [Fact]
    public async Task StaleCompletionDoesNotClearNewSuccessfulCopy()
    {
        var state = new PasswordCopyState();
        var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var oldCopy = state.CopyAsync(() => completion.Task);
        state.PasswordChanged();
        await state.CopyAsync(() => Task.FromResult(true));
        completion.SetResult(false);
        await oldCopy;
        Assert.True(state.IsCopied);
    }
}
