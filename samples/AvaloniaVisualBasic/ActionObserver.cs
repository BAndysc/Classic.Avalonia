using System;

namespace AvaloniaVisualBasic;

public class ActionObserver<T> : IObserver<T>
{
    private readonly Action<T> onNext;

    public ActionObserver(Action<T> onNext)
    {
        this.onNext = onNext;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(T value)
    {
        onNext?.Invoke(value);
    }
}