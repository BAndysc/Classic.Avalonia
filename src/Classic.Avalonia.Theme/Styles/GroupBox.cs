using System;
using Avalonia.Controls.Primitives;

namespace Classic.Avalonia.Theme.Controls;

// Just an alias for HeaderedContentControl
public class GroupBox : HeaderedContentControl
{
    protected override Type StyleKeyOverride => typeof(HeaderedContentControl);
}