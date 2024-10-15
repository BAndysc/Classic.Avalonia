// using System.Collections;
//
// namespace Classic.Avalonia.Theme;
//
// public static class SystemParametersOld
// {
//     private enum CacheSlot : int
//     {
//         DpiX,
//
//         FocusBorderWidth,
//         FocusBorderHeight,
//         HighContrast,
//         MouseVanish,
//
//         DropShadow,
//         FlatMenu,
//         WorkAreaInternal,
//         WorkArea,
//
//         IconMetrics,
//
//         KeyboardCues,
//         KeyboardDelay,
//         KeyboardPreference,
//         KeyboardSpeed,
//         SnapToDefaultButton,
//         WheelScrollLines,
//         MouseHoverTime,
//         MouseHoverHeight,
//         MouseHoverWidth,
//
//         MenuDropAlignment,
//         MenuFade,
//         MenuShowDelay,
//
//         ComboBoxAnimation,
//         ClientAreaAnimation,
//         CursorShadow,
//         GradientCaptions,
//         HotTracking,
//         ListBoxSmoothScrolling,
//         MenuAnimation,
//         SelectionFade,
//         StylusHotTracking,
//         ToolTipAnimation,
//         ToolTipFade,
//         UIEffects,
//
//         MinimizeAnimation,
//         Border,
//         CaretWidth,
//         ForegroundFlashCount,
//         DragFullWindows,
//         NonClientMetrics,
//
//         ThinHorizontalBorderHeight,
//         ThinVerticalBorderWidth,
//         CursorWidth,
//         CursorHeight,
//         ThickHorizontalBorderHeight,
//         ThickVerticalBorderWidth,
//         MinimumHorizontalDragDistance,
//         MinimumVerticalDragDistance,
//         FixedFrameHorizontalBorderHeight,
//         FixedFrameVerticalBorderWidth,
//         FocusHorizontalBorderHeight,
//         FocusVerticalBorderWidth,
//         FullPrimaryScreenWidth,
//         FullPrimaryScreenHeight,
//         HorizontalScrollBarButtonWidth,
//         HorizontalScrollBarHeight,
//         HorizontalScrollBarThumbWidth,
//         IconWidth,
//         IconHeight,
//         IconGridWidth,
//         IconGridHeight,
//         MaximizedPrimaryScreenWidth,
//         MaximizedPrimaryScreenHeight,
//         MaximumWindowTrackWidth,
//         MaximumWindowTrackHeight,
//         MenuCheckmarkWidth,
//         MenuCheckmarkHeight,
//         MenuButtonWidth,
//         MenuButtonHeight,
//         MinimumWindowWidth,
//         MinimumWindowHeight,
//         MinimizedWindowWidth,
//         MinimizedWindowHeight,
//         MinimizedGridWidth,
//         MinimizedGridHeight,
//         MinimumWindowTrackWidth,
//         MinimumWindowTrackHeight,
//         PrimaryScreenWidth,
//         PrimaryScreenHeight,
//         WindowCaptionButtonWidth,
//         WindowCaptionButtonHeight,
//         ResizeFrameHorizontalBorderHeight,
//         ResizeFrameVerticalBorderWidth,
//         SmallIconWidth,
//         SmallIconHeight,
//         SmallWindowCaptionButtonWidth,
//         SmallWindowCaptionButtonHeight,
//         VirtualScreenWidth,
//         VirtualScreenHeight,
//         VerticalScrollBarWidth,
//         VerticalScrollBarButtonHeight,
//         WindowCaptionHeight,
//         KanjiWindowHeight,
//         MenuBarHeight,
//         VerticalScrollBarThumbHeight,
//         IsImmEnabled,
//         IsMediaCenter,
//         IsMenuDropRightAligned,
//         IsMiddleEastEnabled,
//         IsMousePresent,
//         IsMouseWheelPresent,
//         IsPenWindows,
//         IsRemotelyControlled,
//         IsRemoteSession,
//         ShowSounds,
//         IsSlowMachine,
//         SwapButtons,
//         IsTabletPC,
//         VirtualScreenLeft,
//         VirtualScreenTop,
//
//         PowerLineStatus,
//
//         IsGlassEnabled,
//         UxThemeName,
//         UxThemeColor,
//         WindowCornerRadius,
//         WindowGlassColor,
//         WindowGlassBrush,
//         WindowNonClientFrameThickness,
//         WindowResizeBorderThickness,
//
//         NumSlots
//     }
//
//     private static BitArray _cacheValid = new BitArray((int)CacheSlot.NumSlots);
//
//     private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
//     {
//         return new SystemResourceKey(KeyId);
//     }
//
//     internal static double ConvertPixel(int pixel)
//     {
//         int dpi = 0;
//
//         if (dpi != 0)
//         {
//             return (double)pixel * 96 / dpi;
//         }
//
//         return pixel;
//     }
//
//     private static SystemResourceKey? _cacheThinHorizontalBorderHeight;
//     private static SystemResourceKey? _cacheThinVerticalBorderWidth;
//     private static SystemResourceKey? _cacheCursorWidth;
//     private static SystemResourceKey? _cacheCursorHeight;
//     private static SystemResourceKey? _cacheThickHorizontalBorderHeight;
//     private static SystemResourceKey? _cacheThickVerticalBorderWidth;
//     private static SystemResourceKey? _cacheFixedFrameHorizontalBorderHeight;
//     private static SystemResourceKey? _cacheFixedFrameVerticalBorderWidth;
//     private static SystemResourceKey? _cacheFocusHorizontalBorderHeight;
//     private static SystemResourceKey? _cacheFocusVerticalBorderWidth;
//     private static SystemResourceKey? _cacheFullPrimaryScreenWidth;
//     private static SystemResourceKey? _cacheFullPrimaryScreenHeight;
//     private static SystemResourceKey? _cacheHorizontalScrollBarButtonWidth;
//     private static SystemResourceKey? _cacheHorizontalScrollBarHeight;
//     private static SystemResourceKey? _cacheHorizontalScrollBarThumbWidth;
//     private static SystemResourceKey? _cacheIconWidth;
//     private static SystemResourceKey? _cacheIconHeight;
//     private static SystemResourceKey? _cacheIconGridWidth;
//     private static SystemResourceKey? _cacheIconGridHeight;
//     private static SystemResourceKey? _cacheMaximizedPrimaryScreenWidth;
//     private static SystemResourceKey? _cacheMaximizedPrimaryScreenHeight;
//     private static SystemResourceKey? _cacheMaximumWindowTrackWidth;
//     private static SystemResourceKey? _cacheMaximumWindowTrackHeight;
//     private static SystemResourceKey? _cacheMenuCheckmarkWidth;
//     private static SystemResourceKey? _cacheMenuCheckmarkHeight;
//     private static SystemResourceKey? _cacheMenuButtonWidth;
//     private static SystemResourceKey? _cacheMenuButtonHeight;
//     private static SystemResourceKey? _cacheMinimumWindowWidth;
//     private static SystemResourceKey? _cacheMinimumWindowHeight;
//     private static SystemResourceKey? _cacheMinimizedWindowWidth;
//     private static SystemResourceKey? _cacheMinimizedWindowHeight;
//     private static SystemResourceKey? _cacheMinimizedGridWidth;
//     private static SystemResourceKey? _cacheMinimizedGridHeight;
//     private static SystemResourceKey? _cacheMinimumWindowTrackWidth;
//     private static SystemResourceKey? _cacheMinimumWindowTrackHeight;
//     private static SystemResourceKey? _cachePrimaryScreenWidth;
//     private static SystemResourceKey? _cachePrimaryScreenHeight;
//     private static SystemResourceKey? _cacheWindowCaptionButtonWidth;
//     private static SystemResourceKey? _cacheWindowCaptionButtonHeight;
//     private static SystemResourceKey? _cacheResizeFrameHorizontalBorderHeight;
//     private static SystemResourceKey? _cacheResizeFrameVerticalBorderWidth;
//     private static SystemResourceKey? _cacheSmallIconWidth;
//     private static SystemResourceKey? _cacheSmallIconHeight;
//     private static SystemResourceKey? _cacheSmallWindowCaptionButtonWidth;
//     private static SystemResourceKey? _cacheSmallWindowCaptionButtonHeight;
//     private static SystemResourceKey? _cacheVirtualScreenWidth;
//     private static SystemResourceKey? _cacheVirtualScreenHeight;
//     private static SystemResourceKey? _cacheVerticalScrollBarWidth;
//     private static SystemResourceKey? _cacheVerticalScrollBarButtonHeight;
//     private static SystemResourceKey? _cacheWindowCaptionHeight;
//     private static SystemResourceKey? _cacheKanjiWindowHeight;
//     private static SystemResourceKey? _cacheMenuBarHeight;
//     private static SystemResourceKey? _cacheSmallCaptionHeight;
//     private static SystemResourceKey? _cacheVerticalScrollBarThumbHeight;
//     private static SystemResourceKey? _cacheIsImmEnabled;
//     private static SystemResourceKey? _cacheIsMediaCenter;
//     private static SystemResourceKey? _cacheIsMenuDropRightAligned;
//     private static SystemResourceKey? _cacheIsMiddleEastEnabled;
//     private static SystemResourceKey? _cacheIsMousePresent;
//     private static SystemResourceKey? _cacheIsMouseWheelPresent;
//     private static SystemResourceKey? _cacheIsPenWindows;
//     private static SystemResourceKey? _cacheIsRemotelyControlled;
//     private static SystemResourceKey? _cacheIsRemoteSession;
//     private static SystemResourceKey? _cacheShowSounds;
//     private static SystemResourceKey? _cacheIsSlowMachine;
//     private static SystemResourceKey? _cacheSwapButtons;
//     private static SystemResourceKey? _cacheIsTabletPC;
//     private static SystemResourceKey? _cacheVirtualScreenLeft;
//     private static SystemResourceKey? _cacheVirtualScreenTop;
//     private static SystemResourceKey? _cacheFocusBorderWidth;
//     private static SystemResourceKey? _cacheFocusBorderHeight;
//     private static SystemResourceKey? _cacheHighContrast;
//     private static SystemResourceKey? _cacheDropShadow;
//     private static SystemResourceKey? _cacheFlatMenu;
//     private static SystemResourceKey? _cacheWorkArea;
//     private static SystemResourceKey? _cacheIconHorizontalSpacing;
//     private static SystemResourceKey? _cacheIconVerticalSpacing;
//     private static SystemResourceKey? _cacheIconTitleWrap;
//     private static SystemResourceKey? _cacheKeyboardCues;
//     private static SystemResourceKey? _cacheKeyboardDelay;
//     private static SystemResourceKey? _cacheKeyboardPreference;
//     private static SystemResourceKey? _cacheKeyboardSpeed;
//     private static SystemResourceKey? _cacheSnapToDefaultButton;
//     private static SystemResourceKey? _cacheWheelScrollLines;
//     private static SystemResourceKey? _cacheMouseHoverTime;
//     private static SystemResourceKey? _cacheMouseHoverHeight;
//     private static SystemResourceKey? _cacheMouseHoverWidth;
//     private static SystemResourceKey? _cacheMenuDropAlignment;
//     private static SystemResourceKey? _cacheMenuFade;
//     private static SystemResourceKey? _cacheMenuShowDelay;
//     private static SystemResourceKey? _cacheComboBoxAnimation;
//     private static SystemResourceKey? _cacheClientAreaAnimation;
//     private static SystemResourceKey? _cacheCursorShadow;
//     private static SystemResourceKey? _cacheGradientCaptions;
//     private static SystemResourceKey? _cacheHotTracking;
//     private static SystemResourceKey? _cacheListBoxSmoothScrolling;
//     private static SystemResourceKey? _cacheMenuAnimation;
//     private static SystemResourceKey? _cacheSelectionFade;
//     private static SystemResourceKey? _cacheStylusHotTracking;
//     private static SystemResourceKey? _cacheToolTipAnimation;
//     private static SystemResourceKey? _cacheToolTipFade;
//     private static SystemResourceKey? _cacheUIEffects;
//     private static SystemResourceKey? _cacheMinimizeAnimation;
//     private static SystemResourceKey? _cacheBorder;
//     private static SystemResourceKey? _cacheCaretWidth;
//     private static SystemResourceKey? _cacheForegroundFlashCount;
//     private static SystemResourceKey? _cacheDragFullWindows;
//     private static SystemResourceKey? _cacheBorderWidth;
//     private static SystemResourceKey? _cacheScrollWidth;
//     private static SystemResourceKey? _cacheScrollHeight;
//     private static SystemResourceKey? _cacheCaptionWidth;
//     private static SystemResourceKey? _cacheCaptionHeight;
//     private static SystemResourceKey? _cacheSmallCaptionWidth;
//     private static SystemResourceKey? _cacheMenuWidth;
//     private static SystemResourceKey? _cacheMenuHeight;
//     private static SystemResourceKey? _cacheComboBoxPopupAnimation;
//     private static SystemResourceKey? _cacheMenuPopupAnimation;
//     private static SystemResourceKey? _cacheToolTipPopupAnimation;
//     private static SystemResourceKey? _cachePowerLineStatus;
//
//     public static SystemResourceKey FocusBorderWidthKey
//     {
//         get
//         {
//             if (_cacheFocusBorderWidth == null)
//             {
//                 _cacheFocusBorderWidth = CreateInstance(SystemResourceKeyID.FocusBorderWidth);
//             }
//
//             return _cacheFocusBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey FocusBorderHeightKey
//     {
//         get
//         {
//             if (_cacheFocusBorderHeight == null)
//             {
//                 _cacheFocusBorderHeight = CreateInstance(SystemResourceKeyID.FocusBorderHeight);
//             }
//
//             return _cacheFocusBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey HighContrastKey
//     {
//         get
//         {
//             if (_cacheHighContrast == null)
//             {
//                 _cacheHighContrast = CreateInstance(SystemResourceKeyID.HighContrast);
//             }
//
//             return _cacheHighContrast;
//         }
//     }
//
//     public static SystemResourceKey DropShadowKey
//     {
//         get
//         {
//             if (_cacheDropShadow == null)
//             {
//                 _cacheDropShadow = CreateInstance(SystemResourceKeyID.DropShadow);
//             }
//
//             return _cacheDropShadow;
//         }
//     }
//
//     public static SystemResourceKey FlatMenuKey
//     {
//         get
//         {
//             if (_cacheFlatMenu == null)
//             {
//                 _cacheFlatMenu = CreateInstance(SystemResourceKeyID.FlatMenu);
//             }
//
//             return _cacheFlatMenu;
//         }
//     }
//
//     public static SystemResourceKey WorkAreaKey
//     {
//         get
//         {
//             if (_cacheWorkArea == null)
//             {
//                 _cacheWorkArea = CreateInstance(SystemResourceKeyID.WorkArea);
//             }
//
//             return _cacheWorkArea;
//         }
//     }
//
//     public static SystemResourceKey IconHorizontalSpacingKey
//     {
//         get
//         {
//             if (_cacheIconHorizontalSpacing == null)
//             {
//                 _cacheIconHorizontalSpacing = CreateInstance(SystemResourceKeyID.IconHorizontalSpacing);
//             }
//
//             return _cacheIconHorizontalSpacing;
//         }
//     }
//
//     public static SystemResourceKey IconVerticalSpacingKey
//     {
//         get
//         {
//             if (_cacheIconVerticalSpacing == null)
//             {
//                 _cacheIconVerticalSpacing = CreateInstance(SystemResourceKeyID.IconVerticalSpacing);
//             }
//
//             return _cacheIconVerticalSpacing;
//         }
//     }
//
//     public static SystemResourceKey IconTitleWrapKey
//     {
//         get
//         {
//             if (_cacheIconTitleWrap == null)
//             {
//                 _cacheIconTitleWrap = CreateInstance(SystemResourceKeyID.IconTitleWrap);
//             }
//
//             return _cacheIconTitleWrap;
//         }
//     }
//
//     public static SystemResourceKey KeyboardCuesKey
//     {
//         get
//         {
//             if (_cacheKeyboardCues == null)
//             {
//                 _cacheKeyboardCues = CreateInstance(SystemResourceKeyID.KeyboardCues);
//             }
//
//             return _cacheKeyboardCues;
//         }
//     }
//
//     public static SystemResourceKey KeyboardDelayKey
//     {
//         get
//         {
//             if (_cacheKeyboardDelay == null)
//             {
//                 _cacheKeyboardDelay = CreateInstance(SystemResourceKeyID.KeyboardDelay);
//             }
//
//             return _cacheKeyboardDelay;
//         }
//     }
//
//     public static SystemResourceKey KeyboardPreferenceKey
//     {
//         get
//         {
//             if (_cacheKeyboardPreference == null)
//             {
//                 _cacheKeyboardPreference = CreateInstance(SystemResourceKeyID.KeyboardPreference);
//             }
//
//             return _cacheKeyboardPreference;
//         }
//     }
//
//     public static SystemResourceKey KeyboardSpeedKey
//     {
//         get
//         {
//             if (_cacheKeyboardSpeed == null)
//             {
//                 _cacheKeyboardSpeed = CreateInstance(SystemResourceKeyID.KeyboardSpeed);
//             }
//
//             return _cacheKeyboardSpeed;
//         }
//     }
//
//     public static SystemResourceKey SnapToDefaultButtonKey
//     {
//         get
//         {
//             if (_cacheSnapToDefaultButton == null)
//             {
//                 _cacheSnapToDefaultButton = CreateInstance(SystemResourceKeyID.SnapToDefaultButton);
//             }
//
//             return _cacheSnapToDefaultButton;
//         }
//     }
//
//     public static SystemResourceKey WheelScrollLinesKey
//     {
//         get
//         {
//             if (_cacheWheelScrollLines == null)
//             {
//                 _cacheWheelScrollLines = CreateInstance(SystemResourceKeyID.WheelScrollLines);
//             }
//
//             return _cacheWheelScrollLines;
//         }
//     }
//
//     public static SystemResourceKey MouseHoverTimeKey
//     {
//         get
//         {
//             if (_cacheMouseHoverTime == null)
//             {
//                 _cacheMouseHoverTime = CreateInstance(SystemResourceKeyID.MouseHoverTime);
//             }
//
//             return _cacheMouseHoverTime;
//         }
//     }
//
//     public static SystemResourceKey MouseHoverHeightKey
//     {
//         get
//         {
//             if (_cacheMouseHoverHeight == null)
//             {
//                 _cacheMouseHoverHeight = CreateInstance(SystemResourceKeyID.MouseHoverHeight);
//             }
//
//             return _cacheMouseHoverHeight;
//         }
//     }
//
//     public static SystemResourceKey MouseHoverWidthKey
//     {
//         get
//         {
//             if (_cacheMouseHoverWidth == null)
//             {
//                 _cacheMouseHoverWidth = CreateInstance(SystemResourceKeyID.MouseHoverWidth);
//             }
//
//             return _cacheMouseHoverWidth;
//         }
//     }
//
//     public static SystemResourceKey MenuDropAlignmentKey
//     {
//         get
//         {
//             if (_cacheMenuDropAlignment == null)
//             {
//                 _cacheMenuDropAlignment = CreateInstance(SystemResourceKeyID.MenuDropAlignment);
//             }
//
//             return _cacheMenuDropAlignment;
//         }
//     }
//
//     public static SystemResourceKey MenuFadeKey
//     {
//         get
//         {
//             if (_cacheMenuFade == null)
//             {
//                 _cacheMenuFade = CreateInstance(SystemResourceKeyID.MenuFade);
//             }
//
//             return _cacheMenuFade;
//         }
//     }
//
//     public static SystemResourceKey MenuShowDelayKey
//     {
//         get
//         {
//             if (_cacheMenuShowDelay == null)
//             {
//                 _cacheMenuShowDelay = CreateInstance(SystemResourceKeyID.MenuShowDelay);
//             }
//
//             return _cacheMenuShowDelay;
//         }
//     }
//
//     public static SystemResourceKey ComboBoxAnimationKey
//     {
//         get
//         {
//             if (_cacheComboBoxAnimation == null)
//             {
//                 _cacheComboBoxAnimation = CreateInstance(SystemResourceKeyID.ComboBoxAnimation);
//             }
//
//             return _cacheComboBoxAnimation;
//         }
//     }
//
//     public static SystemResourceKey ClientAreaAnimationKey
//     {
//         get
//         {
//             if (_cacheClientAreaAnimation == null)
//             {
//                 _cacheClientAreaAnimation = CreateInstance(SystemResourceKeyID.ClientAreaAnimation);
//             }
//
//             return _cacheClientAreaAnimation;
//         }
//     }
//
//     public static SystemResourceKey CursorShadowKey
//     {
//         get
//         {
//             if (_cacheCursorShadow == null)
//             {
//                 _cacheCursorShadow = CreateInstance(SystemResourceKeyID.CursorShadow);
//             }
//
//             return _cacheCursorShadow;
//         }
//     }
//
//     public static SystemResourceKey GradientCaptionsKey
//     {
//         get
//         {
//             if (_cacheGradientCaptions == null)
//             {
//                 _cacheGradientCaptions = CreateInstance(SystemResourceKeyID.GradientCaptions);
//             }
//
//             return _cacheGradientCaptions;
//         }
//     }
//
//     public static SystemResourceKey HotTrackingKey
//     {
//         get
//         {
//             if (_cacheHotTracking == null)
//             {
//                 _cacheHotTracking = CreateInstance(SystemResourceKeyID.HotTracking);
//             }
//
//             return _cacheHotTracking;
//         }
//     }
//
//     public static SystemResourceKey ListBoxSmoothScrollingKey
//     {
//         get
//         {
//             if (_cacheListBoxSmoothScrolling == null)
//             {
//                 _cacheListBoxSmoothScrolling = CreateInstance(SystemResourceKeyID.ListBoxSmoothScrolling);
//             }
//
//             return _cacheListBoxSmoothScrolling;
//         }
//     }
//
//     public static SystemResourceKey MenuAnimationKey
//     {
//         get
//         {
//             if (_cacheMenuAnimation == null)
//             {
//                 _cacheMenuAnimation = CreateInstance(SystemResourceKeyID.MenuAnimation);
//             }
//
//             return _cacheMenuAnimation;
//         }
//     }
//
//     public static SystemResourceKey SelectionFadeKey
//     {
//         get
//         {
//             if (_cacheSelectionFade == null)
//             {
//                 _cacheSelectionFade = CreateInstance(SystemResourceKeyID.SelectionFade);
//             }
//
//             return _cacheSelectionFade;
//         }
//     }
//
//     public static SystemResourceKey StylusHotTrackingKey
//     {
//         get
//         {
//             if (_cacheStylusHotTracking == null)
//             {
//                 _cacheStylusHotTracking = CreateInstance(SystemResourceKeyID.StylusHotTracking);
//             }
//
//             return _cacheStylusHotTracking;
//         }
//     }
//
//     public static SystemResourceKey ToolTipAnimationKey
//     {
//         get
//         {
//             if (_cacheToolTipAnimation == null)
//             {
//                 _cacheToolTipAnimation = CreateInstance(SystemResourceKeyID.ToolTipAnimation);
//             }
//
//             return _cacheToolTipAnimation;
//         }
//     }
//
//     public static SystemResourceKey ToolTipFadeKey
//     {
//         get
//         {
//             if (_cacheToolTipFade == null)
//             {
//                 _cacheToolTipFade = CreateInstance(SystemResourceKeyID.ToolTipFade);
//             }
//
//             return _cacheToolTipFade;
//         }
//     }
//
//     public static SystemResourceKey UIEffectsKey
//     {
//         get
//         {
//             if (_cacheUIEffects == null)
//             {
//                 _cacheUIEffects = CreateInstance(SystemResourceKeyID.UIEffects);
//             }
//
//             return _cacheUIEffects;
//         }
//     }
//
//     public static SystemResourceKey ComboBoxPopupAnimationKey
//     {
//         get
//         {
//             if (_cacheComboBoxPopupAnimation == null)
//             {
//                 _cacheComboBoxPopupAnimation = CreateInstance(SystemResourceKeyID.ComboBoxPopupAnimation);
//             }
//
//             return _cacheComboBoxPopupAnimation;
//         }
//     }
//
//     public static SystemResourceKey MenuPopupAnimationKey
//     {
//         get
//         {
//             if (_cacheMenuPopupAnimation == null)
//             {
//                 _cacheMenuPopupAnimation = CreateInstance(SystemResourceKeyID.MenuPopupAnimation);
//             }
//
//             return _cacheMenuPopupAnimation;
//         }
//     }
//
//     public static SystemResourceKey ToolTipPopupAnimationKey
//     {
//         get
//         {
//             if (_cacheToolTipPopupAnimation == null)
//             {
//                 _cacheToolTipPopupAnimation = CreateInstance(SystemResourceKeyID.ToolTipPopupAnimation);
//             }
//
//             return _cacheToolTipPopupAnimation;
//         }
//     }
//
//     public static SystemResourceKey MinimizeAnimationKey
//     {
//         get
//         {
//             if (_cacheMinimizeAnimation == null)
//             {
//                 _cacheMinimizeAnimation = CreateInstance(SystemResourceKeyID.MinimizeAnimation);
//             }
//
//             return _cacheMinimizeAnimation;
//         }
//     }
//
//     public static SystemResourceKey BorderKey
//     {
//         get
//         {
//             if (_cacheBorder == null)
//             {
//                 _cacheBorder = CreateInstance(SystemResourceKeyID.Border);
//             }
//
//             return _cacheBorder;
//         }
//     }
//
//     public static SystemResourceKey CaretWidthKey
//     {
//         get
//         {
//             if (_cacheCaretWidth == null)
//             {
//                 _cacheCaretWidth = CreateInstance(SystemResourceKeyID.CaretWidth);
//             }
//
//             return _cacheCaretWidth;
//         }
//     }
//
//     public static SystemResourceKey ForegroundFlashCountKey
//     {
//         get
//         {
//             if (_cacheForegroundFlashCount == null)
//             {
//                 _cacheForegroundFlashCount = CreateInstance(SystemResourceKeyID.ForegroundFlashCount);
//             }
//
//             return _cacheForegroundFlashCount;
//         }
//     }
//
//     public static SystemResourceKey DragFullWindowsKey
//     {
//         get
//         {
//             if (_cacheDragFullWindows == null)
//             {
//                 _cacheDragFullWindows = CreateInstance(SystemResourceKeyID.DragFullWindows);
//             }
//
//             return _cacheDragFullWindows;
//         }
//     }
//
//     public static SystemResourceKey BorderWidthKey
//     {
//         get
//         {
//             if (_cacheBorderWidth == null)
//             {
//                 _cacheBorderWidth = CreateInstance(SystemResourceKeyID.BorderWidth);
//             }
//
//             return _cacheBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey ScrollWidthKey
//     {
//         get
//         {
//             if (_cacheScrollWidth == null)
//             {
//                 _cacheScrollWidth = CreateInstance(SystemResourceKeyID.ScrollWidth);
//             }
//
//             return _cacheScrollWidth;
//         }
//     }
//
//     public static SystemResourceKey ScrollHeightKey
//     {
//         get
//         {
//             if (_cacheScrollHeight == null)
//             {
//                 _cacheScrollHeight = CreateInstance(SystemResourceKeyID.ScrollHeight);
//             }
//
//             return _cacheScrollHeight;
//         }
//     }
//
//     public static SystemResourceKey CaptionWidthKey
//     {
//         get
//         {
//             if (_cacheCaptionWidth == null)
//             {
//                 _cacheCaptionWidth = CreateInstance(SystemResourceKeyID.CaptionWidth);
//             }
//
//             return _cacheCaptionWidth;
//         }
//     }
//
//     public static SystemResourceKey CaptionHeightKey
//     {
//         get
//         {
//             if (_cacheCaptionHeight == null)
//             {
//                 _cacheCaptionHeight = CreateInstance(SystemResourceKeyID.CaptionHeight);
//             }
//
//             return _cacheCaptionHeight;
//         }
//     }
//
//     public static SystemResourceKey SmallCaptionWidthKey
//     {
//         get
//         {
//             if (_cacheSmallCaptionWidth == null)
//             {
//                 _cacheSmallCaptionWidth = CreateInstance(SystemResourceKeyID.SmallCaptionWidth);
//             }
//
//             return _cacheSmallCaptionWidth;
//         }
//     }
//
//     public static SystemResourceKey MenuWidthKey
//     {
//         get
//         {
//             if (_cacheMenuWidth == null)
//             {
//                 _cacheMenuWidth = CreateInstance(SystemResourceKeyID.MenuWidth);
//             }
//
//             return _cacheMenuWidth;
//         }
//     }
//
//     public static SystemResourceKey MenuHeightKey
//     {
//         get
//         {
//             if (_cacheMenuHeight == null)
//             {
//                 _cacheMenuHeight = CreateInstance(SystemResourceKeyID.MenuHeight);
//             }
//
//             return _cacheMenuHeight;
//         }
//     }
//
//     public static SystemResourceKey ThinHorizontalBorderHeightKey
//     {
//         get
//         {
//             if (_cacheThinHorizontalBorderHeight == null)
//             {
//                 _cacheThinHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ThinHorizontalBorderHeight);
//             }
//
//             return _cacheThinHorizontalBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey ThinVerticalBorderWidthKey
//     {
//         get
//         {
//             if (_cacheThinVerticalBorderWidth == null)
//             {
//                 _cacheThinVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ThinVerticalBorderWidth);
//             }
//
//             return _cacheThinVerticalBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey CursorWidthKey
//     {
//         get
//         {
//             if (_cacheCursorWidth == null)
//             {
//                 _cacheCursorWidth = CreateInstance(SystemResourceKeyID.CursorWidth);
//             }
//
//             return _cacheCursorWidth;
//         }
//     }
//
//     public static SystemResourceKey CursorHeightKey
//     {
//         get
//         {
//             if (_cacheCursorHeight == null)
//             {
//                 _cacheCursorHeight = CreateInstance(SystemResourceKeyID.CursorHeight);
//             }
//
//             return _cacheCursorHeight;
//         }
//     }
//
//     public static SystemResourceKey ThickHorizontalBorderHeightKey
//     {
//         get
//         {
//             if (_cacheThickHorizontalBorderHeight == null)
//             {
//                 _cacheThickHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ThickHorizontalBorderHeight);
//             }
//
//             return _cacheThickHorizontalBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey ThickVerticalBorderWidthKey
//     {
//         get
//         {
//             if (_cacheThickVerticalBorderWidth == null)
//             {
//                 _cacheThickVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ThickVerticalBorderWidth);
//             }
//
//             return _cacheThickVerticalBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey FixedFrameHorizontalBorderHeightKey
//     {
//         get
//         {
//             if (_cacheFixedFrameHorizontalBorderHeight == null)
//             {
//                 _cacheFixedFrameHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.FixedFrameHorizontalBorderHeight);
//             }
//
//             return _cacheFixedFrameHorizontalBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey FixedFrameVerticalBorderWidthKey
//     {
//         get
//         {
//             if (_cacheFixedFrameVerticalBorderWidth == null)
//             {
//                 _cacheFixedFrameVerticalBorderWidth = CreateInstance(SystemResourceKeyID.FixedFrameVerticalBorderWidth);
//             }
//
//             return _cacheFixedFrameVerticalBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey FocusHorizontalBorderHeightKey
//     {
//         get
//         {
//             if (_cacheFocusHorizontalBorderHeight == null)
//             {
//                 _cacheFocusHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.FocusHorizontalBorderHeight);
//             }
//
//             return _cacheFocusHorizontalBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey FocusVerticalBorderWidthKey
//     {
//         get
//         {
//             if (_cacheFocusVerticalBorderWidth == null)
//             {
//                 _cacheFocusVerticalBorderWidth = CreateInstance(SystemResourceKeyID.FocusVerticalBorderWidth);
//             }
//
//             return _cacheFocusVerticalBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey FullPrimaryScreenWidthKey
//     {
//         get
//         {
//             if (_cacheFullPrimaryScreenWidth == null)
//             {
//                 _cacheFullPrimaryScreenWidth = CreateInstance(SystemResourceKeyID.FullPrimaryScreenWidth);
//             }
//
//             return _cacheFullPrimaryScreenWidth;
//         }
//     }
//
//     public static SystemResourceKey FullPrimaryScreenHeightKey
//     {
//         get
//         {
//             if (_cacheFullPrimaryScreenHeight == null)
//             {
//                 _cacheFullPrimaryScreenHeight = CreateInstance(SystemResourceKeyID.FullPrimaryScreenHeight);
//             }
//
//             return _cacheFullPrimaryScreenHeight;
//         }
//     }
//
//     public static SystemResourceKey HorizontalScrollBarButtonWidthKey
//     {
//         get
//         {
//             if (_cacheHorizontalScrollBarButtonWidth == null)
//             {
//                 _cacheHorizontalScrollBarButtonWidth = CreateInstance(SystemResourceKeyID.HorizontalScrollBarButtonWidth);
//             }
//
//             return _cacheHorizontalScrollBarButtonWidth;
//         }
//     }
//
//     public static SystemResourceKey HorizontalScrollBarHeightKey
//     {
//         get
//         {
//             if (_cacheHorizontalScrollBarHeight == null)
//             {
//                 _cacheHorizontalScrollBarHeight = CreateInstance(SystemResourceKeyID.HorizontalScrollBarHeight);
//             }
//
//             return _cacheHorizontalScrollBarHeight;
//         }
//     }
//
//     public static SystemResourceKey HorizontalScrollBarThumbWidthKey
//     {
//         get
//         {
//             if (_cacheHorizontalScrollBarThumbWidth == null)
//             {
//                 _cacheHorizontalScrollBarThumbWidth = CreateInstance(SystemResourceKeyID.HorizontalScrollBarThumbWidth);
//             }
//
//             return _cacheHorizontalScrollBarThumbWidth;
//         }
//     }
//
//     public static SystemResourceKey IconWidthKey
//     {
//         get
//         {
//             if (_cacheIconWidth == null)
//             {
//                 _cacheIconWidth = CreateInstance(SystemResourceKeyID.IconWidth);
//             }
//
//             return _cacheIconWidth;
//         }
//     }
//
//     public static SystemResourceKey IconHeightKey
//     {
//         get
//         {
//             if (_cacheIconHeight == null)
//             {
//                 _cacheIconHeight = CreateInstance(SystemResourceKeyID.IconHeight);
//             }
//
//             return _cacheIconHeight;
//         }
//     }
//
//     public static SystemResourceKey IconGridWidthKey
//     {
//         get
//         {
//             if (_cacheIconGridWidth == null)
//             {
//                 _cacheIconGridWidth = CreateInstance(SystemResourceKeyID.IconGridWidth);
//             }
//
//             return _cacheIconGridWidth;
//         }
//     }
//
//     public static SystemResourceKey IconGridHeightKey
//     {
//         get
//         {
//             if (_cacheIconGridHeight == null)
//             {
//                 _cacheIconGridHeight = CreateInstance(SystemResourceKeyID.IconGridHeight);
//             }
//
//             return _cacheIconGridHeight;
//         }
//     }
//
//     public static SystemResourceKey MaximizedPrimaryScreenWidthKey
//     {
//         get
//         {
//             if (_cacheMaximizedPrimaryScreenWidth == null)
//             {
//                 _cacheMaximizedPrimaryScreenWidth = CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenWidth);
//             }
//
//             return _cacheMaximizedPrimaryScreenWidth;
//         }
//     }
//
//     public static SystemResourceKey MaximizedPrimaryScreenHeightKey
//     {
//         get
//         {
//             if (_cacheMaximizedPrimaryScreenHeight == null)
//             {
//                 _cacheMaximizedPrimaryScreenHeight = CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenHeight);
//             }
//
//             return _cacheMaximizedPrimaryScreenHeight;
//         }
//     }
//
//     public static SystemResourceKey MaximumWindowTrackWidthKey
//     {
//         get
//         {
//             if (_cacheMaximumWindowTrackWidth == null)
//             {
//                 _cacheMaximumWindowTrackWidth = CreateInstance(SystemResourceKeyID.MaximumWindowTrackWidth);
//             }
//
//             return _cacheMaximumWindowTrackWidth;
//         }
//     }
//
//     public static SystemResourceKey MaximumWindowTrackHeightKey
//     {
//         get
//         {
//             if (_cacheMaximumWindowTrackHeight == null)
//             {
//                 _cacheMaximumWindowTrackHeight = CreateInstance(SystemResourceKeyID.MaximumWindowTrackHeight);
//             }
//
//             return _cacheMaximumWindowTrackHeight;
//         }
//     }
//
//     public static SystemResourceKey MenuCheckmarkWidthKey
//     {
//         get
//         {
//             if (_cacheMenuCheckmarkWidth == null)
//             {
//                 _cacheMenuCheckmarkWidth = CreateInstance(SystemResourceKeyID.MenuCheckmarkWidth);
//             }
//
//             return _cacheMenuCheckmarkWidth;
//         }
//     }
//
//     public static SystemResourceKey MenuCheckmarkHeightKey
//     {
//         get
//         {
//             if (_cacheMenuCheckmarkHeight == null)
//             {
//                 _cacheMenuCheckmarkHeight = CreateInstance(SystemResourceKeyID.MenuCheckmarkHeight);
//             }
//
//             return _cacheMenuCheckmarkHeight;
//         }
//     }
//
//     public static SystemResourceKey MenuButtonWidthKey
//     {
//         get
//         {
//             if (_cacheMenuButtonWidth == null)
//             {
//                 _cacheMenuButtonWidth = CreateInstance(SystemResourceKeyID.MenuButtonWidth);
//             }
//
//             return _cacheMenuButtonWidth;
//         }
//     }
//
//     public static SystemResourceKey MenuButtonHeightKey
//     {
//         get
//         {
//             if (_cacheMenuButtonHeight == null)
//             {
//                 _cacheMenuButtonHeight = CreateInstance(SystemResourceKeyID.MenuButtonHeight);
//             }
//
//             return _cacheMenuButtonHeight;
//         }
//     }
//
//     public static SystemResourceKey MinimumWindowWidthKey
//     {
//         get
//         {
//             if (_cacheMinimumWindowWidth == null)
//             {
//                 _cacheMinimumWindowWidth = CreateInstance(SystemResourceKeyID.MinimumWindowWidth);
//             }
//
//             return _cacheMinimumWindowWidth;
//         }
//     }
//
//     public static SystemResourceKey MinimumWindowHeightKey
//     {
//         get
//         {
//             if (_cacheMinimumWindowHeight == null)
//             {
//                 _cacheMinimumWindowHeight = CreateInstance(SystemResourceKeyID.MinimumWindowHeight);
//             }
//
//             return _cacheMinimumWindowHeight;
//         }
//     }
//
//     public static SystemResourceKey MinimizedWindowWidthKey
//     {
//         get
//         {
//             if (_cacheMinimizedWindowWidth == null)
//             {
//                 _cacheMinimizedWindowWidth = CreateInstance(SystemResourceKeyID.MinimizedWindowWidth);
//             }
//
//             return _cacheMinimizedWindowWidth;
//         }
//     }
//
//     public static SystemResourceKey MinimizedWindowHeightKey
//     {
//         get
//         {
//             if (_cacheMinimizedWindowHeight == null)
//             {
//                 _cacheMinimizedWindowHeight = CreateInstance(SystemResourceKeyID.MinimizedWindowHeight);
//             }
//
//             return _cacheMinimizedWindowHeight;
//         }
//     }
//
//     public static SystemResourceKey MinimizedGridWidthKey
//     {
//         get
//         {
//             if (_cacheMinimizedGridWidth == null)
//             {
//                 _cacheMinimizedGridWidth = CreateInstance(SystemResourceKeyID.MinimizedGridWidth);
//             }
//
//             return _cacheMinimizedGridWidth;
//         }
//     }
//
//     public static SystemResourceKey MinimizedGridHeightKey
//     {
//         get
//         {
//             if (_cacheMinimizedGridHeight == null)
//             {
//                 _cacheMinimizedGridHeight = CreateInstance(SystemResourceKeyID.MinimizedGridHeight);
//             }
//
//             return _cacheMinimizedGridHeight;
//         }
//     }
//
//     public static SystemResourceKey MinimumWindowTrackWidthKey
//     {
//         get
//         {
//             if (_cacheMinimumWindowTrackWidth == null)
//             {
//                 _cacheMinimumWindowTrackWidth = CreateInstance(SystemResourceKeyID.MinimumWindowTrackWidth);
//             }
//
//             return _cacheMinimumWindowTrackWidth;
//         }
//     }
//
//     public static SystemResourceKey MinimumWindowTrackHeightKey
//     {
//         get
//         {
//             if (_cacheMinimumWindowTrackHeight == null)
//             {
//                 _cacheMinimumWindowTrackHeight = CreateInstance(SystemResourceKeyID.MinimumWindowTrackHeight);
//             }
//
//             return _cacheMinimumWindowTrackHeight;
//         }
//     }
//
//     public static SystemResourceKey PrimaryScreenWidthKey
//     {
//         get
//         {
//             if (_cachePrimaryScreenWidth == null)
//             {
//                 _cachePrimaryScreenWidth = CreateInstance(SystemResourceKeyID.PrimaryScreenWidth);
//             }
//
//             return _cachePrimaryScreenWidth;
//         }
//     }
//
//     public static SystemResourceKey PrimaryScreenHeightKey
//     {
//         get
//         {
//             if (_cachePrimaryScreenHeight == null)
//             {
//                 _cachePrimaryScreenHeight = CreateInstance(SystemResourceKeyID.PrimaryScreenHeight);
//             }
//
//             return _cachePrimaryScreenHeight;
//         }
//     }
//
//     public static SystemResourceKey WindowCaptionButtonWidthKey
//     {
//         get
//         {
//             if (_cacheWindowCaptionButtonWidth == null)
//             {
//                 _cacheWindowCaptionButtonWidth = CreateInstance(SystemResourceKeyID.WindowCaptionButtonWidth);
//             }
//
//             return _cacheWindowCaptionButtonWidth;
//         }
//     }
//
//     public static SystemResourceKey WindowCaptionButtonHeightKey
//     {
//         get
//         {
//             if (_cacheWindowCaptionButtonHeight == null)
//             {
//                 _cacheWindowCaptionButtonHeight = CreateInstance(SystemResourceKeyID.WindowCaptionButtonHeight);
//             }
//
//             return _cacheWindowCaptionButtonHeight;
//         }
//     }
//
//     public static SystemResourceKey ResizeFrameHorizontalBorderHeightKey
//     {
//         get
//         {
//             if (_cacheResizeFrameHorizontalBorderHeight == null)
//             {
//                 _cacheResizeFrameHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ResizeFrameHorizontalBorderHeight);
//             }
//
//             return _cacheResizeFrameHorizontalBorderHeight;
//         }
//     }
//
//     public static SystemResourceKey ResizeFrameVerticalBorderWidthKey
//     {
//         get
//         {
//             if (_cacheResizeFrameVerticalBorderWidth == null)
//             {
//                 _cacheResizeFrameVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ResizeFrameVerticalBorderWidth);
//             }
//
//             return _cacheResizeFrameVerticalBorderWidth;
//         }
//     }
//
//     public static SystemResourceKey SmallIconWidthKey
//     {
//         get
//         {
//             if (_cacheSmallIconWidth == null)
//             {
//                 _cacheSmallIconWidth = CreateInstance(SystemResourceKeyID.SmallIconWidth);
//             }
//
//             return _cacheSmallIconWidth;
//         }
//     }
//
//     public static SystemResourceKey SmallIconHeightKey
//     {
//         get
//         {
//             if (_cacheSmallIconHeight == null)
//             {
//                 _cacheSmallIconHeight = CreateInstance(SystemResourceKeyID.SmallIconHeight);
//             }
//
//             return _cacheSmallIconHeight;
//         }
//     }
//
//     public static SystemResourceKey SmallWindowCaptionButtonWidthKey
//     {
//         get
//         {
//             if (_cacheSmallWindowCaptionButtonWidth == null)
//             {
//                 _cacheSmallWindowCaptionButtonWidth = CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonWidth);
//             }
//
//             return _cacheSmallWindowCaptionButtonWidth;
//         }
//     }
//
//     public static SystemResourceKey SmallWindowCaptionButtonHeightKey
//     {
//         get
//         {
//             if (_cacheSmallWindowCaptionButtonHeight == null)
//             {
//                 _cacheSmallWindowCaptionButtonHeight = CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonHeight);
//             }
//
//             return _cacheSmallWindowCaptionButtonHeight;
//         }
//     }
//
//     public static SystemResourceKey VirtualScreenWidthKey
//     {
//         get
//         {
//             if (_cacheVirtualScreenWidth == null)
//             {
//                 _cacheVirtualScreenWidth = CreateInstance(SystemResourceKeyID.VirtualScreenWidth);
//             }
//
//             return _cacheVirtualScreenWidth;
//         }
//     }
//
//     public static SystemResourceKey VirtualScreenHeightKey
//     {
//         get
//         {
//             if (_cacheVirtualScreenHeight == null)
//             {
//                 _cacheVirtualScreenHeight = CreateInstance(SystemResourceKeyID.VirtualScreenHeight);
//             }
//
//             return _cacheVirtualScreenHeight;
//         }
//     }
//
//     public static SystemResourceKey VerticalScrollBarWidthKey
//     {
//         get
//         {
//             if (_cacheVerticalScrollBarWidth == null)
//             {
//                 _cacheVerticalScrollBarWidth = CreateInstance(SystemResourceKeyID.VerticalScrollBarWidth);
//             }
//
//             return _cacheVerticalScrollBarWidth;
//         }
//     }
//
//     public static SystemResourceKey VerticalScrollBarButtonHeightKey
//     {
//         get
//         {
//             if (_cacheVerticalScrollBarButtonHeight == null)
//             {
//                 _cacheVerticalScrollBarButtonHeight = CreateInstance(SystemResourceKeyID.VerticalScrollBarButtonHeight);
//             }
//
//             return _cacheVerticalScrollBarButtonHeight;
//         }
//     }
//
//     public static SystemResourceKey WindowCaptionHeightKey
//     {
//         get
//         {
//             if (_cacheWindowCaptionHeight == null)
//             {
//                 _cacheWindowCaptionHeight = CreateInstance(SystemResourceKeyID.WindowCaptionHeight);
//             }
//
//             return _cacheWindowCaptionHeight;
//         }
//     }
//
//     public static SystemResourceKey KanjiWindowHeightKey
//     {
//         get
//         {
//             if (_cacheKanjiWindowHeight == null)
//             {
//                 _cacheKanjiWindowHeight = CreateInstance(SystemResourceKeyID.KanjiWindowHeight);
//             }
//
//             return _cacheKanjiWindowHeight;
//         }
//     }
//
//     public static SystemResourceKey MenuBarHeightKey
//     {
//         get
//         {
//             if (_cacheMenuBarHeight == null)
//             {
//                 _cacheMenuBarHeight = CreateInstance(SystemResourceKeyID.MenuBarHeight);
//             }
//
//             return _cacheMenuBarHeight;
//         }
//     }
//
//     public static SystemResourceKey SmallCaptionHeightKey
//     {
//         get
//         {
//             if (_cacheSmallCaptionHeight == null)
//             {
//                 _cacheSmallCaptionHeight = CreateInstance(SystemResourceKeyID.SmallCaptionHeight);
//             }
//
//             return _cacheSmallCaptionHeight;
//         }
//     }
//
//     public static SystemResourceKey VerticalScrollBarThumbHeightKey
//     {
//         get
//         {
//             if (_cacheVerticalScrollBarThumbHeight == null)
//             {
//                 _cacheVerticalScrollBarThumbHeight = CreateInstance(SystemResourceKeyID.VerticalScrollBarThumbHeight);
//             }
//
//             return _cacheVerticalScrollBarThumbHeight;
//         }
//     }
//
//     public static SystemResourceKey IsImmEnabledKey
//     {
//         get
//         {
//             if (_cacheIsImmEnabled == null)
//             {
//                 _cacheIsImmEnabled = CreateInstance(SystemResourceKeyID.IsImmEnabled);
//             }
//
//             return _cacheIsImmEnabled;
//         }
//     }
//
//     public static SystemResourceKey IsMediaCenterKey
//     {
//         get
//         {
//             if (_cacheIsMediaCenter == null)
//             {
//                 _cacheIsMediaCenter = CreateInstance(SystemResourceKeyID.IsMediaCenter);
//             }
//
//             return _cacheIsMediaCenter;
//         }
//     }
//
//     public static SystemResourceKey IsMenuDropRightAlignedKey
//     {
//         get
//         {
//             if (_cacheIsMenuDropRightAligned == null)
//             {
//                 _cacheIsMenuDropRightAligned = CreateInstance(SystemResourceKeyID.IsMenuDropRightAligned);
//             }
//
//             return _cacheIsMenuDropRightAligned;
//         }
//     }
//
//     public static SystemResourceKey IsMiddleEastEnabledKey
//     {
//         get
//         {
//             if (_cacheIsMiddleEastEnabled == null)
//             {
//                 _cacheIsMiddleEastEnabled = CreateInstance(SystemResourceKeyID.IsMiddleEastEnabled);
//             }
//
//             return _cacheIsMiddleEastEnabled;
//         }
//     }
//
//     public static SystemResourceKey IsMousePresentKey
//     {
//         get
//         {
//             if (_cacheIsMousePresent == null)
//             {
//                 _cacheIsMousePresent = CreateInstance(SystemResourceKeyID.IsMousePresent);
//             }
//
//             return _cacheIsMousePresent;
//         }
//     }
//
//     public static SystemResourceKey IsMouseWheelPresentKey
//     {
//         get
//         {
//             if (_cacheIsMouseWheelPresent == null)
//             {
//                 _cacheIsMouseWheelPresent = CreateInstance(SystemResourceKeyID.IsMouseWheelPresent);
//             }
//
//             return _cacheIsMouseWheelPresent;
//         }
//     }
//
//     public static SystemResourceKey IsPenWindowsKey
//     {
//         get
//         {
//             if (_cacheIsPenWindows == null)
//             {
//                 _cacheIsPenWindows = CreateInstance(SystemResourceKeyID.IsPenWindows);
//             }
//
//             return _cacheIsPenWindows;
//         }
//     }
//
//     public static SystemResourceKey IsRemotelyControlledKey
//     {
//         get
//         {
//             if (_cacheIsRemotelyControlled == null)
//             {
//                 _cacheIsRemotelyControlled = CreateInstance(SystemResourceKeyID.IsRemotelyControlled);
//             }
//
//             return _cacheIsRemotelyControlled;
//         }
//     }
//
//     public static SystemResourceKey IsRemoteSessionKey
//     {
//         get
//         {
//             if (_cacheIsRemoteSession == null)
//             {
//                 _cacheIsRemoteSession = CreateInstance(SystemResourceKeyID.IsRemoteSession);
//             }
//
//             return _cacheIsRemoteSession;
//         }
//     }
//
//     public static SystemResourceKey ShowSoundsKey
//     {
//         get
//         {
//             if (_cacheShowSounds == null)
//             {
//                 _cacheShowSounds = CreateInstance(SystemResourceKeyID.ShowSounds);
//             }
//
//             return _cacheShowSounds;
//         }
//     }
//
//     public static SystemResourceKey IsSlowMachineKey
//     {
//         get
//         {
//             if (_cacheIsSlowMachine == null)
//             {
//                 _cacheIsSlowMachine = CreateInstance(SystemResourceKeyID.IsSlowMachine);
//             }
//
//             return _cacheIsSlowMachine;
//         }
//     }
//
//     public static SystemResourceKey SwapButtonsKey
//     {
//         get
//         {
//             if (_cacheSwapButtons == null)
//             {
//                 _cacheSwapButtons = CreateInstance(SystemResourceKeyID.SwapButtons);
//             }
//
//             return _cacheSwapButtons;
//         }
//     }
//
//     public static SystemResourceKey IsTabletPCKey
//     {
//         get
//         {
//             if (_cacheIsTabletPC == null)
//             {
//                 _cacheIsTabletPC = CreateInstance(SystemResourceKeyID.IsTabletPC);
//             }
//
//             return _cacheIsTabletPC;
//         }
//     }
//
//     public static SystemResourceKey VirtualScreenLeftKey
//     {
//         get
//         {
//             if (_cacheVirtualScreenLeft == null)
//             {
//                 _cacheVirtualScreenLeft = CreateInstance(SystemResourceKeyID.VirtualScreenLeft);
//             }
//
//             return _cacheVirtualScreenLeft;
//         }
//     }
//
//     public static SystemResourceKey VirtualScreenTopKey
//     {
//         get
//         {
//             if (_cacheVirtualScreenTop == null)
//             {
//                 _cacheVirtualScreenTop = CreateInstance(SystemResourceKeyID.VirtualScreenTop);
//             }
//
//             return _cacheVirtualScreenTop;
//         }
//     }
//
//     // public static SystemResourceKey FocusVisualStyleKey
//     // {
//     //     get
//     //     {
//     //         if (_cacheFocusVisualStyle == null)
//     //         {
//     //             _cacheFocusVisualStyle = new SystemThemeKey(SystemResourceKeyID.FocusVisualStyle);
//     //         }
//     //
//     //         return _cacheFocusVisualStyle;
//     //     }
//     // }
//     //
//     // public static SystemResourceKey NavigationChromeStyleKey
//     // {
//     //     get
//     //     {
//     //         if (_cacheNavigationChromeStyle == null)
//     //         {
//     //             _cacheNavigationChromeStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeStyle);
//     //         }
//     //
//     //         return _cacheNavigationChromeStyle;
//     //     }
//     // }
//     //
//     // public static SystemResourceKey NavigationChromeDownLevelStyleKey
//     // {
//     //     get
//     //     {
//     //         if (_cacheNavigationChromeDownLevelStyle == null)
//     //         {
//     //             _cacheNavigationChromeDownLevelStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeDownLevelStyle);
//     //         }
//     //
//     //         return _cacheNavigationChromeDownLevelStyle;
//     //     }
//     // }
//
//     public static SystemResourceKey PowerLineStatusKey
//     {
//         get
//         {
//             if (_cachePowerLineStatus == null)
//             {
//                 _cachePowerLineStatus = CreateInstance(SystemResourceKeyID.PowerLineStatus);
//             }
//
//             return _cachePowerLineStatus;
//         }
//     }
//
//     public static double VerticalScrollBarWidth
//     {
//         get
//         {
//             lock (_cacheValid)
//             {
//                 while (!_cacheValid[(int)CacheSlot.VerticalScrollBarWidth])
//                 {
//                     _cacheValid[(int)CacheSlot.VerticalScrollBarWidth] = true;
//                     _verticalScrollBarWidth = ConvertPixel(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXVSCROLL));
//                 }
//             }
//
//             return _verticalScrollBarWidth;
//         }
//     }
//
//     public static double HorizontalScrollBarHeight
//     {
//         get
//         {
//             lock (_cacheValid)
//             {
//                 while (!_cacheValid[(int)CacheSlot.HorizontalScrollBarHeight])
//                 {
//                     _cacheValid[(int)CacheSlot.HorizontalScrollBarHeight] = true;
//                     _horizontalScrollBarHeight = ConvertPixel(NativeMethods.GetSystemMetrics(NativeMethods.SM.CYHSCROLL));
//                 }
//             }
//
//             return _horizontalScrollBarHeight;
//         }
//     }
//
//     public static double HorizontalScrollBarButtonWidth
//     {
//         get
//         {
//             lock (_cacheValid)
//             {
//                 while (!_cacheValid[(int)CacheSlot.HorizontalScrollBarButtonWidth])
//                 {
//                     _cacheValid[(int)CacheSlot.HorizontalScrollBarButtonWidth] = true;
//                     _horizontalScrollBarButtonWidth = ConvertPixel(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXHSCROLL));
//                 }
//             }
//
//             return _horizontalScrollBarButtonWidth;
//         }
//     }
//
//     public static double VerticalScrollBarButtonHeight
//     {
//         get
//         {
//             lock (_cacheValid)
//             {
//                 while (!_cacheValid[(int)CacheSlot.VerticalScrollBarButtonHeight])
//                 {
//                     _cacheValid[(int)CacheSlot.VerticalScrollBarButtonHeight] = true;
//                     _verticalScrollBarButtonHeight = ConvertPixel(NativeMethods.GetSystemMetrics(NativeMethods.SM.CYVSCROLL));
//                 }
//             }
//
//             return _verticalScrollBarButtonHeight;
//         }
//     }
//
//     public static double HorizontalScrollBarThumbWidth
//     {
//         get
//         {
//             lock (_cacheValid)
//             {
//                 while (!_cacheValid[(int)CacheSlot.HorizontalScrollBarThumbWidth])
//                 {
//                     _cacheValid[(int)CacheSlot.HorizontalScrollBarThumbWidth] = true;
//                     _horizontalScrollBarThumbWidth = ConvertPixel(NativeMethods.GetSystemMetrics(NativeMethods.SM.CXHTHUMB));
//                 }
//             }
//
//             return _horizontalScrollBarThumbWidth;
//         }
//     }
//
//
//     private static double _verticalScrollBarWidth;
//     private static double _verticalScrollBarButtonHeight;
//     private static double _horizontalScrollBarHeight;
//     private static double _horizontalScrollBarButtonWidth;
//     private static double _horizontalScrollBarThumbWidth;
// }




        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ActiveBorderColorKey}\">" + SystemColorsOld.ActiveBorderColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ActiveCaptionColorKey}\">" + SystemColorsOld.ActiveCaptionColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ActiveCaptionTextColorKey}\">" + SystemColorsOld.ActiveCaptionTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.AppWorkspaceColorKey}\">" + SystemColorsOld.AppWorkspaceColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlColorKey}\">" + SystemColorsOld.ControlColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlDarkColorKey}\">" + SystemColorsOld.ControlDarkColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlDarkDarkColorKey}\">" + SystemColorsOld.ControlDarkDarkColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlLightColorKey}\">" + SystemColorsOld.ControlLightColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlLightLightColorKey}\">" + SystemColorsOld.ControlLightLightColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ControlTextColorKey}\">" + SystemColorsOld.ControlTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.DesktopColorKey}\">" + SystemColorsOld.DesktopColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.GradientActiveCaptionColorKey}\">" + SystemColorsOld.GradientActiveCaptionColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.GradientInactiveCaptionColorKey}\">" + SystemColorsOld.GradientInactiveCaptionColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.GrayTextColorKey}\">" + SystemColorsOld.GrayTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.HighlightColorKey}\">" + SystemColorsOld.HighlightColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.HighlightTextColorKey}\">" + SystemColorsOld.HighlightTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.HotTrackColorKey}\">" + SystemColorsOld.HotTrackColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.InactiveBorderColorKey}\">" + SystemColorsOld.InactiveBorderColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.InactiveCaptionColorKey}\">" + SystemColorsOld.InactiveCaptionColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.InactiveCaptionTextColorKey}\">" + SystemColorsOld.InactiveCaptionTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.InfoColorKey}\">" + SystemColorsOld.InfoColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.InfoTextColorKey}\">" + SystemColorsOld.InfoTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.MenuColorKey}\">" + SystemColorsOld.MenuColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.MenuBarColorKey}\">" + SystemColorsOld.MenuBarColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.MenuHighlightColorKey}\">" + SystemColorsOld.MenuHighlightColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.MenuTextColorKey}\">" + SystemColorsOld.MenuTextColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.ScrollBarColorKey}\">" + SystemColorsOld.ScrollBarColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.WindowColorKey}\">" + SystemColorsOld.WindowColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.WindowFrameColorKey}\">" + SystemColorsOld.WindowFrameColor + "</Color>");
        // Console.WriteLine("<Color x:Key=\"{x:Static classic:SystemColors.WindowTextColorKey}\">" + SystemColorsOld.WindowTextColor + "</Color>");
        //
        // Console.WriteLine("<system:Double x:Key=\"{x:Static classic:SystemParameters.HorizontalScrollBarHeightKey}\">" + SystemParametersOld.HorizontalScrollBarHeight + "</system:Double>");
        // Console.WriteLine("<system:Double x:Key=\"{x:Static classic:SystemParameters.VerticalScrollBarWidthKey}\">" + SystemParametersOld.VerticalScrollBarWidth + "</system:Double>");
        // Console.WriteLine("<system:Double x:Key=\"{x:Static classic:SystemParameters.HorizontalScrollBarButtonWidthKey}\">" + SystemParametersOld.HorizontalScrollBarButtonWidth + "</system:Double>");
        // Console.WriteLine("<system:Double x:Key=\"{x:Static classic:SystemParameters.VerticalScrollBarButtonHeightKey}\">" + SystemParametersOld.VerticalScrollBarButtonHeight + "</system:Double>");
        // Console.WriteLine("<system:Double x:Key=\"{x:Static classic:SystemParameters.HorizontalScrollBarThumbWidthKey}\">" + SystemParametersOld.HorizontalScrollBarThumbWidth + "</system:Double>");