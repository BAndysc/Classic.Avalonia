using System;
using System.Windows.Input;

namespace AvaloniaVisualBasic;

public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> command;
    private readonly Func<T, bool>? canExecute;

    public DelegateCommand(Action<T> command, Func<T, bool>? canExecute)
    {
        this.command = command;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (canExecute == null)
            return true;
        if (parameter is T t)
            return canExecute(t);
        return false;
    }

    public void Execute(object? parameter)
    {
        if (parameter is T t)
            command(t);
    }

    public event EventHandler? CanExecuteChanged;
}

public class DelegateCommand : ICommand
{
    private readonly Action command;
    private readonly Func<bool>? canExecute;

    public DelegateCommand(Action command, Func<bool>? canExecute)
    {
        this.command = command;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (canExecute == null)
            return true;
        return canExecute();
    }

    public void Execute(object? parameter)
    {
        command();
    }

    public event EventHandler? CanExecuteChanged;
}