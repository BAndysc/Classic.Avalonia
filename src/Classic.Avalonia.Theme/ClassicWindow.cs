using System;
using System.Runtime.InteropServices;
using Avalonia;
using Classic.CommonControls.Utils;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace Classic.Avalonia.Theme;

public class ClassicWindow : Window
{
    protected override Type StyleKeyOverride => typeof(ClassicWindow);

    public static readonly AttachedProperty<WindowIcon?> SmallIconProperty = AvaloniaProperty.RegisterAttached<ClassicWindow, Window, WindowIcon?>("SmallIcon");

    public static WindowIcon? GetSmallIcon(AvaloniaObject element) => element.GetValue(SmallIconProperty);

    public static void SetSmallIcon(AvaloniaObject element, WindowIcon? value) => element.SetValue(SmallIconProperty, value);

    private static class NativeWindows
    {
        [DllImport("dwmapi.dll")]
        public static extern unsafe int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, void* pvAttribute, int cbAttribute);

        public enum DwmWindowCornerPreference : uint
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND,
            DWMWCP_ROUND,
            DWMWCP_ROUNDSMALL
        }

        public enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_PASSIVE_UPDATE_MODE,
            DWMWA_USE_HOSTBACKDROPBRUSH,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_BORDER_COLOR,
            DWMWA_CAPTION_COLOR,
            DWMWA_TEXT_COLOR,
            DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
            DWMWA_LAST
        };
    }

    private static class NativeMacOs
    {
        private const string ObjCLibrary = "/usr/lib/libobjc.dylib";
        [DllImport(ObjCLibrary, EntryPoint = "objc_getClass")]
        private static extern IntPtr objc_getClass(string className);

        [DllImport(ObjCLibrary, EntryPoint = "sel_registerName")]
        private static extern IntPtr sel_registerName(string selector);

        [DllImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
        private static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport(ObjCLibrary, EntryPoint = "objc_msgSend")]
        private static extern void objc_msgSend_void(IntPtr receiver, IntPtr selector, ulong arg);

        public static void EnableResizable(IntPtr windowPtr)
        {
            // Get styleMask
            IntPtr styleMaskSel = sel_registerName("styleMask");
            IntPtr styleMask = objc_msgSend(windowPtr, styleMaskSel);

            // Assuming NSResizableWindowMask is the bit mask for resizable windows
            const ulong nsWindowStyleMaskResizable = 1 << 3; // Correct mask for resizable windows
            ulong currentMask = (ulong)styleMask.ToInt64();

            // Set styleMask to make window resizable
            currentMask |= nsWindowStyleMaskResizable;

            // Set new styleMask
            IntPtr setStyleMaskSel = sel_registerName("setStyleMask:");
            objc_msgSend_void(windowPtr, setStyleMaskSel, currentMask);
        }
    }

    protected override void ExtendClientAreaToDecorationsChanged(bool isExtended)
    {
        base.ExtendClientAreaToDecorationsChanged(isExtended);

        // Fix for Windows: disable rounded corners
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (TryGetPlatformHandle() is { } handle)
            {
                unsafe
                {
                    int cornerPreference = (int)NativeWindows.DwmWindowCornerPreference.DWMWCP_DONOTROUND;
                    NativeWindows.DwmSetWindowAttribute(handle.Handle, (int)NativeWindows.DwmWindowAttribute.DWMWA_WINDOW_CORNER_PREFERENCE,
                        &cornerPreference, sizeof(int));
                }
            }
        }
    }

    public ClassicWindow()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Activated += OnActivated;
        }
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        // Fix for macOS: enable resizing despite SystemDecorations = None
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && CanResize)
        {
            DispatcherTimer.RunOnce(() =>
            {
                if (CanResize && TryGetPlatformHandle() is { } handle)
                {
                    NativeMacOs.EnableResizable(handle.Handle);
                }
            }, TimeSpan.FromMilliseconds(1));
        }
    }
}

internal class ClassicWindowFactory : IWindowFactory
{
    public Window Create() => new ClassicWindow();
}