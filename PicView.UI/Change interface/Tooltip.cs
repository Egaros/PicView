﻿using System;
using System.Windows;
using System.Windows.Media.Animation;
using static PicView.UC;

namespace PicView
{
    internal static class Tooltip
    {
        /// <summary>
        /// Shows a black tooltip on screen in a given time
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="center">If centered or on bottom</param>
        /// <param name="time">How long until it fades away</param>
        internal static void ShowTooltipMessage(object message, bool center, TimeSpan time)
        {
            if (sexyToolTip == null)
            {
                return;
            }

            sexyToolTip.Visibility = Visibility.Visible;

            if (center)
            {
                sexyToolTip.Margin = new Thickness(0, 0, 0, 0);
                sexyToolTip.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                sexyToolTip.Margin = new Thickness(0, 0, 0, 15);
                sexyToolTip.VerticalAlignment = VerticalAlignment.Bottom;
            }

            sexyToolTip.SexyToolTipText.Text = message.ToString();
            var anim = new DoubleAnimation(1, TimeSpan.FromSeconds(.5));
            anim.Completed += (s, _) => AnimationHelper.Fade(sexyToolTip, TimeSpan.FromSeconds(1.5), time, 1, 0);

            sexyToolTip.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        /// <summary>
        /// Shows a black tooltip on screen for a small time
        /// </summary>
        /// <param name="message">The message to display</param>
        internal static void ShowTooltipMessage(object message, bool center = false)
        {
            ShowTooltipMessage(message, center, TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Hides the Messagebox ToolTipStyle
        /// </summary>
        internal static void CloseToolTipMessage()
        {
            sexyToolTip.Visibility = Visibility.Hidden;
        }

    }
}
