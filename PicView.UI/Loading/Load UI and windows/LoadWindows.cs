using PicView.Windows;
using System.Diagnostics;
using System.Windows;
using static PicView.Fields;

namespace PicView
{
    public static class LoadWindows
    {
        public static AllSettings allSettingsWindow;
        public static Info infoWindow;
        public static Effects effects;
        public static ResizeAndOptimize resizeAndOptimize;

        #region Windows

        /// <summary>
        /// Show Help window in a dialog
        /// </summary>
        public static void HelpWindow()
        {
            if (infoWindow == null)
            {
                infoWindow = new Info
                {
                    Owner = mainWindow
                };

                infoWindow.Show();
            }
            else
            {
                if (infoWindow.Visibility == Visibility.Visible)
                {
                    infoWindow.Focus();
                }
                else
                {
                    infoWindow.Show();
                }
            }

#if DEBUG
            Trace.WriteLine("HelpWindow loaded ");
#endif
        }

        /// <summary>
        /// Show All Settings window
        /// </summary>
        public static void AllSettingsWindow()
        {
            if (allSettingsWindow == null)
            {
                allSettingsWindow = new AllSettings
                {
                    Owner = mainWindow
                };

                allSettingsWindow.Show();
            }
            else
            {
                if (allSettingsWindow.Visibility == Visibility.Visible)
                {
                    allSettingsWindow.Focus();
                }
                else
                {
                    allSettingsWindow.Show();
                }
            }

#if DEBUG
            Trace.WriteLine("HelpWindow loaded ");
#endif
        }

        /// <summary>
        /// Show Effects window
        /// </summary>
        public static void EffectsWindow()
        {
            if (effects == null)
            {
                effects = new Effects
                {
                    Owner = mainWindow
                };

                effects.Show();
            }
            else
            {
                if (effects.Visibility == Visibility.Visible)
                {
                    effects.Focus();
                }
                else
                {
                    effects.Show();
                }
            }

#if DEBUG
            Trace.WriteLine("EffectsWindow loaded ");
#endif
        }

        /// <summary>
        /// Show Effects window
        /// </summary>
        public static void ResizeAndOptimizeWindow()
        {
            if (resizeAndOptimize == null)
            {
                resizeAndOptimize = new ResizeAndOptimize
                {
                    Owner = mainWindow
                };

                resizeAndOptimize.Show();
            }
            else
            {
                if (resizeAndOptimize.Visibility == Visibility.Visible)
                {
                    resizeAndOptimize.Focus();
                }
                else
                {
                    resizeAndOptimize.Show();
                }
            }

#if DEBUG
            Trace.WriteLine("EffectsWindow loaded ");
#endif
        }

        #endregion Windows
    }
}
