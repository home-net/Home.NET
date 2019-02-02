using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Home.NET.Tiles.Extensions
{
    public static class TileAnimation
    {
        public static UIElement FadeFromTo(this UIElement uiElement, double fromOpacity,
       double toOpacity, int durationInMilliseconds, bool loopAnimation,
       bool showOnStart, bool collapseOnFinish, int startAt = 0)
        {
            uiElement.Opacity = fromOpacity;

            var timeSpan = TimeSpan.FromMilliseconds(durationInMilliseconds);
            var doubleAnimation =
                  new DoubleAnimation(fromOpacity, toOpacity,
                                      new Duration(timeSpan));

            doubleAnimation.BeginTime = TimeSpan.FromMilliseconds(startAt);

            if (loopAnimation)
                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            uiElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);

            if (showOnStart)
            {
                uiElement.ApplyAnimationClock(UIElement.VisibilityProperty, null);
                uiElement.Visibility = Visibility.Visible;
            }

            if (collapseOnFinish)
            {
                var keyAnimation = new ObjectAnimationUsingKeyFrames { Duration = new Duration(timeSpan) };
                keyAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromTimeSpan(timeSpan)));
                uiElement.BeginAnimation(UIElement.VisibilityProperty, keyAnimation);
            }

            return uiElement;
        }

        public static UIElement FadeIn(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
            return uiElement.FadeFromTo(0, 1, durationInMilliseconds, false, true, false, startAt);
        }

        public static UIElement FadeOut(this UIElement uiElement, int durationInMilliseconds, int startAt = 0)
        {
            return uiElement.FadeFromTo(1, 0, durationInMilliseconds, false, false, true, startAt);
        }

        public static void LiveTileAnimation(this Tile tile, bool Enable = false)
        {
        }
    }
}
