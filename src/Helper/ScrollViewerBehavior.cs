using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Awe.UI.Helper
{
    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "VerticalOffset",
                typeof(double),
                typeof(ScrollViewerBehavior),
                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        public static void SetVerticalOffset(ScrollViewer target, double value)
        {
            target.SetValue(VerticalOffsetProperty, value);
        }

        public static double GetVerticalOffset(ScrollViewer target)
        {
            return (double)target.GetValue(VerticalOffsetProperty);
        }

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            (target as ScrollViewer)?.ScrollToVerticalOffset((double)e.NewValue);
        }

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewerBehavior),
                new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));

        public static void SetHorizontalOffset(ScrollViewer target, double value)
        {
            target.SetValue(HorizontalOffsetProperty, value);
        }

        public static double GetHorizontalOffset(ScrollViewer target)
        {
            return (double)target.GetValue(HorizontalOffsetProperty);
        }

        private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            (target as ScrollViewer)?.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public static readonly DependencyProperty IsAnimationingProperty =
           DependencyProperty.RegisterAttached(
               "IsAnimationing",
               typeof(bool),
               typeof(ScrollViewerBehavior),
               new UIPropertyMetadata(false));

        public static void SetIsAnimationing(ScrollViewer target, bool value)
        {
            target.SetValue(IsAnimationingProperty, value);
        }

        public static bool GetIsAnimationing(ScrollViewer target)
        {
            return (bool)target.GetValue(IsAnimationingProperty);
        }

        public static readonly DependencyProperty UseSmoothScrollProperty =
            DependencyProperty.RegisterAttached(
                "UseSmoothScroll",
                typeof(bool),
                typeof(ScrollViewerBehavior),
                new UIPropertyMetadata(false, OnUseSmoothScrollChanged));

        public static void SetUseSmoothScroll(ScrollViewer target, bool value)
        {
            target.SetValue(HorizontalOffsetProperty, value);
        }

        public static bool GetUseSmoothScroll(ScrollViewer target)
        {
            return (bool)target.GetValue(HorizontalOffsetProperty);
        }



        private static Orientation GetDirection()
        {
            var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            return isShiftDown ? Orientation.Horizontal : Orientation.Vertical;
        }

        private static void OnUseSmoothScrollChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {        
            double LastVerticalLocation = 0;
            double LastHorizontalLocation = 0;
            if (e.NewValue is true && target is ScrollViewer sv)
            {
                sv.ScrollChanged += (x, e) =>
                {
                    if (!GetIsAnimationing(sv))
                    {
                        LastHorizontalLocation = e.HorizontalOffset;
                        LastVerticalLocation = e.VerticalOffset;
                    }
                };
                sv.PreviewMouseWheel += (x, e) =>
                {
                    var dr = GetDirection();
                    if (dr is Orientation.Vertical)
                    {
                        double WheelChange = e.Delta * (sv.ViewportHeight / 1.5) / sv.ActualHeight;
                        double newOffset = LastVerticalLocation - WheelChange;

                        if (newOffset < 0)
                        {
                            newOffset = 0;
                        }

                        if (newOffset > sv.ScrollableHeight)
                        {
                            newOffset = sv.ScrollableHeight;
                        }

                        if (newOffset == LastVerticalLocation ||
                            double.IsNaN(newOffset))
                        {
                            // e.Handled = true;
                            return;
                        }

                        sv.ScrollToVerticalOffset(LastVerticalLocation);
                        sv.ScrollToHorizontalOffset(LastHorizontalLocation);

                        double scale = Math.Abs((LastVerticalLocation - newOffset) / WheelChange);

                        AnimateScroll(sv, newOffset, dr, scale);
                        LastVerticalLocation = newOffset;
                    }
                    else
                    {
                        double WheelChange = e.Delta * (sv.ViewportWidth / 1.5) / sv.ActualWidth;
                        double newOffset = LastHorizontalLocation - WheelChange;

                        

                        if (newOffset < 0)
                        {
                            newOffset = 0;
                        }

                        if (newOffset > sv.ScrollableWidth)
                        {
                            newOffset = sv.ScrollableWidth;
                        }

                        if (newOffset == LastHorizontalLocation ||
                            double.IsNaN(newOffset))
                        {
                            // e.Handled = true;
                            return;
                        }

                        sv.ScrollToVerticalOffset(LastVerticalLocation); 
                        sv.ScrollToHorizontalOffset(LastHorizontalLocation);

                        double scale = Math.Abs((LastHorizontalLocation - newOffset) / WheelChange);

                        AnimateScroll(sv, newOffset, dr, scale);
                        LastHorizontalLocation = newOffset;
                    }

                };


            }

        }
        private static void AnimateScroll(ScrollViewer cv,double ToValue, Orientation Direction, double Scale)
        {
            if (Direction == Orientation.Vertical)
            {
                cv.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);
                DoubleAnimation Animation = new DoubleAnimation();
                Animation.EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut };
                Animation.From = cv.VerticalOffset;
                Animation.To = ToValue;
                Animation.Duration = TimeSpan.FromMilliseconds(350 * Scale);
                //Timeline.SetDesiredFrameRate(Animation, 40);
                cv.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, Animation);
            }
            else
            {
                cv.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, null);
                DoubleAnimation Animation = new DoubleAnimation();
                Animation.EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut };
                Animation.From = cv.HorizontalOffset;
                Animation.To = ToValue;
                Animation.Duration = TimeSpan.FromMilliseconds(400 * Scale);
                //Timeline.SetDesiredFrameRate(Animation, 40);
                cv.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, Animation);
            }

            cv.BeginAnimation(ScrollViewerBehavior.IsAnimationingProperty, null);
            BooleanAnimationUsingKeyFrames keyFramesAnimation = new BooleanAnimationUsingKeyFrames();
            keyFramesAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(true, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            keyFramesAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(false, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400 * Scale + 1))));
            cv.BeginAnimation(ScrollViewerBehavior.IsAnimationingProperty, keyFramesAnimation);
        }
    }
}
