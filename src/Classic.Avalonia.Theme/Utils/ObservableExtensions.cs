using System;

namespace Classic.Avalonia.Theme.Utils;

internal static class ObservableExtensions
{
    public static IDisposable Subscribe<T>(this IObservable<T> observer, Action<T> onNext)
    {
        return observer.Subscribe(new AnonymousObserver<T>(onNext));
    }

    private class AnonymousObserver<T> : IObserver<T>
    {
        private readonly Action<T> onNext;

        public AnonymousObserver(Action<T> onNext)
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
            onNext.Invoke(value);
        }
    }
}