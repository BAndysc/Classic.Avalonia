using System;
using System.Collections.Generic;

namespace Classic.Avalonia.Theme.Utils;

internal class CompositeDisposable : List<IDisposable>, IDisposable
{
    public void Dispose()
    {
        foreach (var x in this)
            x.Dispose();
    }
}