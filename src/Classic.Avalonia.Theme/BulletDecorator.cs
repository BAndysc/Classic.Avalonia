/*
 https://github.com/dotnet/wpf/blob/main/src/Microsoft.DotNet.Wpf/src/PresentationFramework/System/Windows/Controls/Primitives/BulletDecorator.cs
 * The MIT License (MIT)

   Copyright (c) .NET Foundation and Contributors

   All rights reserved.

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.
 */

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Classic.Avalonia.Theme;

    /// <summary>
    ///     BulletDecorator is used for decorating a generic content of type Control.
    /// Usually, the content is a text and the bullet is a glyph representing
    /// something similar to a checkbox or a radiobutton.
    /// Bullet property is used to decorate the content by aligning itself with the first line of the content text.
    /// </summary>
    public class BulletDecorator : Decorator
    {
        //-------------------------------------------------------------------
        //
        //  Constructors
        //
        //-------------------------------------------------------------------

        #region Constructors

        /// <summary>
        ///     Default BulletDecorator constructor
        /// </summary>
        /// <remarks>
        ///     Automatic determination of current Dispatcher.
        /// Use alternative constructor that accepts a Dispatcher for best performance.
        /// </remarks>
        public BulletDecorator() : base()
        {
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
        }

        #endregion

        static BulletDecorator()
        {
            AffectsRender<BulletDecorator>(BackgroundProperty);
        }

        //-------------------------------------------------------------------
        //
        //  Constructors
        //
        //-------------------------------------------------------------------

        #region Properties

        /// <summary>
        /// AvaloniaProperty for <see cref="Background" /> property.
        /// </summary>
        public static readonly AvaloniaProperty BackgroundProperty =
                Panel.BackgroundProperty.AddOwner<BulletDecorator>();

        /// <summary>
        /// The Background property defines the brush used to fill the area within the BulletDecorator.
        /// </summary>
        public IBrush? Background
        {
            get => (IBrush?)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Bullet property is the first visual element in BulletDecorator visual tree.
        /// It should be aligned to BulletDecorator.Child which is the second visual child.
        /// </summary>
        /// <value></value>
        public Control Bullet
        {
            get => _bullet;
            set
            {
                if (_bullet != value)
                {
                    if (_bullet != null)
                    {
                        // notify the visual layer that the old bullet has been removed.
                        VisualChildren.Remove(_bullet);

                        //need to remove old element from logical tree
                        LogicalChildren.Remove(_bullet);
                    }

                    _bullet = value;

                    LogicalChildren.Add(value);
                    // notify the visual layer about the new child.
                    VisualChildren.Add(value);

                    // If we decorator content exists we need to move it at the end of the visual tree
                    Control child = Child;
                    if (child != null)
                    {
                        VisualChildren.Remove(child);
                        VisualChildren.Add(child);
                    }

                    InvalidateMeasure();
                }
            }
        }

        #endregion Properties

        //-------------------------------------------------------------------
        //
        //  Protected Methods
        //
        //-------------------------------------------------------------------

        #region Protected Methods

        /// <summary>
        /// Override from Control
        /// </summary>
        public override void Render(DrawingContext dc)
        {
            // Draw background in rectangle inside border.
            IBrush? background = this.Background;
            if (background != null)
            {
                dc.DrawRectangle(background,
                                 null,
                                 new Rect(0, 0, Bounds.Width, Bounds.Height));
            }
        }

        /// <summary>
        /// Updates DesiredSize of the BulletDecorator. Called by parent Control.
        /// This is the first pass of layout.
        /// </summary>
        /// <param name="constraint">Constraint size is an "upper limit" that BulletDecorator should not exceed.</param>
        /// <returns>BulletDecorator' desired size.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
                Size bulletSize = new Size();
                Size contentSize = new Size();
                Control bullet = Bullet;
                Control content = Child;

                // If we have bullet we should measure it first
                if (bullet != null)
                {
                    bullet.Measure(constraint);
                    bulletSize = bullet.DesiredSize;
                }

                // If we have second child (content) we should measure it
                if (content != null)
                {
                    Size contentConstraint = constraint;
                    contentConstraint = contentConstraint.WithWidth(Math.Max(0.0, contentConstraint.Width - bulletSize.Width));

                    content.Measure(contentConstraint);
                    contentSize = content.DesiredSize;
                }

                Size desiredSize = new Size(bulletSize.Width + contentSize.Width, Math.Max(bulletSize.Height, contentSize.Height));
                return desiredSize;
        }

        /// <summary>
        /// BulletDecorator arranges its children - Bullet and Child.
        /// Bullet is aligned vertically with the center
        /// </summary>
        /// <param name="arrangeSize">Size that BulletDecorator will assume to position children.</param>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
                Control bullet = Bullet;
                Control content = Child;
                double contentOffsetX = 0;

                double bulletOffsetY = 0;

                Size bulletSize = new Size();

                // Arrange the bullet if exist
                if (bullet != null)
                {
                    var desiredSize = bullet.DesiredSize;
                    bullet.Arrange(new Rect(0, arrangeSize.Height / 2 - desiredSize.Height / 2, desiredSize.Width, desiredSize.Height));
                    bulletSize = bullet.Bounds.Size;

                    contentOffsetX = bulletSize.Width;
                }

                // Arrange the content if exist
                if (content != null)
                {
                    // Helper arranges child and may substitute a child's explicit properties for its DesiredSize.
                    // The actual size the child takes up is stored in its RenderSize.
                    Size contentSize = arrangeSize;
                    if (bullet != null)
                    {
                        contentSize = contentSize.WithWidth(Math.Max(content.DesiredSize.Width, arrangeSize.Width - bullet.DesiredSize.Width));
                        contentSize = contentSize.WithHeight(Math.Max(content.DesiredSize.Height, arrangeSize.Height));
                    }
                    content.Arrange(new Rect(contentOffsetX, 0, contentSize.Width, contentSize.Height));

                    // WPF leftover, in VB6 a bullet is centered vertically, not to the first line, where does it come from?
                    //double centerY = GetFirstLineHeight(content) * 0.5d;
                    //bulletOffsetY += Math.Max(0d, centerY - bulletSize.Height * 0.5d);
                }

                // Re-Position the bullet if exist
                if (bullet != null && Math.Abs(bulletOffsetY) > double.Epsilon)
                {
                    bullet.Arrange(new Rect(0, bulletOffsetY, bullet.DesiredSize.Width, bullet.DesiredSize.Height));
                }

                return arrangeSize;
        }

        #endregion Protected Methods

        //-------------------------------------------------------------------
        //
        //  Private Methods
        //
        //-------------------------------------------------------------------

        #region Private Methods

        // This method calculates the height of the first line if the element is TextBlock or FlowDocumentScrollViewer
        // Otherwise returns the element height
        private double GetFirstLineHeight(Control element)
        {
            return element.Bounds.Size.Height;
            // // We need to find TextBlock/FlowDocumentScrollViewer if it is nested inside ContentPresenter
            // // Common scenario when used in styles is that BulletDecorator content is a ContentPresenter
            // Control text = FindText(element);
            // ReadOnlyCollection<LineResult> lr = null;
            // if (text != null)
            // {
            //     TextBlock textElement = ((TextBlock)text);
            //     if (textElement.IsLayoutDataValid)
            //         lr = textElement.GetLineResults();
            // }
            // else
            // {
            //     text = FindFlowDocumentScrollViewer(element);
            //     if (text != null)
            //     {
            //         TextDocumentView tdv = ((IServiceProvider)text).GetService(typeof(ITextView)) as TextDocumentView;
            //         if (tdv != null && tdv.IsValid)
            //         {
            //             ReadOnlyCollection<ColumnResult> cr = tdv.Columns;
            //             if (cr != null && cr.Count > 0)
            //             {
            //                 ColumnResult columnResult = cr[0];
            //                 ReadOnlyCollection<ParagraphResult> pr = columnResult.Paragraphs;
            //                 if (pr != null && pr.Count > 0)
            //                 {
            //                     ContainerParagraphResult cpr = pr[0] as ContainerParagraphResult;
            //                     if (cpr != null)
            //                     {
            //                         TextParagraphResult textParagraphResult = cpr.Paragraphs[0] as TextParagraphResult;
            //                         if (textParagraphResult != null)
            //                         {
            //                             lr = textParagraphResult.Lines;
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            //
            // if (lr != null && lr.Count > 0)
            // {
            //     Point ancestorOffset = new Point();
            //     text.TransformToAncestor(element).TryTransform(ancestorOffset, out ancestorOffset);
            //     return lr[0].LayoutBox.Height + ancestorOffset.Y * 2d;
            // }
            //
            // return element.RenderSize.Height;
        }

        // private TextBlock FindText(Visual root)
        // {
        //     // Cases where the root is itself a TextBlock
        //     TextBlock text = root as TextBlock;
        //     if (text != null)
        //         return text;
        //
        //     ContentPresenter cp = root as ContentPresenter;
        //     if (cp != null)
        //     {
        //         if (VisualTreeHelper.GetChildrenCount(cp) == 1)
        //         {
        //             DependencyObject child = VisualTreeHelper.GetChild(cp, 0);
        //
        //             // Cases where the child is a TextBlock
        //             TextBlock textBlock = child as TextBlock;
        //             if (textBlock == null)
        //             {
        //                 AccessText accessText = child as AccessText;
        //                 if (accessText != null &&
        //                     VisualTreeHelper.GetChildrenCount(accessText) == 1)
        //                 {
        //                     // Cases where the child is an AccessText whose child is a TextBlock
        //                     textBlock = VisualTreeHelper.GetChild(accessText, 0) as TextBlock;
        //                 }
        //             }
        //             return textBlock;
        //         }
        //     }
        //     else
        //     {
        //         AccessText accessText = root as AccessText;
        //         if (accessText != null &&
        //             VisualTreeHelper.GetChildrenCount(accessText) == 1)
        //         {
        //             // Cases where the root is an AccessText whose child is a TextBlock
        //             return VisualTreeHelper.GetChild(accessText, 0) as TextBlock;
        //         }
        //     }
        //     return null;
        // }
        //
        // private FlowDocumentScrollViewer FindFlowDocumentScrollViewer(Visual root)
        // {
        //     FlowDocumentScrollViewer text = root as FlowDocumentScrollViewer;
        //     if (text != null)
        //         return text;
        //
        //     ContentPresenter cp = root as ContentPresenter;
        //     if (cp != null)
        //     {
        //         if(VisualTreeHelper.GetChildrenCount(cp) == 1)
        //             return VisualTreeHelper.GetChild(cp, 0) as FlowDocumentScrollViewer;
        //     }
        //     return null;
        // }

        #endregion

        //-------------------------------------------------------------------
        //
        //  Private Memebers
        //
        //-------------------------------------------------------------------

        #region Private Members
        Control _bullet = null;
        #endregion Private Members

    }