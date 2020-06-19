using PicView.UserControls;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace PicView
{
    public static class UC
    {
        public static ImageSettings imageSettingsMenu;
        public static FileMenu fileMenu;
        public static QuickSettingsMenu quickSettingsMenu;
        public static ToolsAndEffectsMenu toolsAndEffectsMenu;
        public static AjaxLoading ajaxLoading;
        public static SexyToolTip sexyToolTip;
        public static AutoScrollSign autoScrollSign;
        public static ClickArrow clickArrowLeft;
        public static ClickArrow clickArrowRight;
        public static X2 x2;
        public static Minus minus;
        public static PicGallery picGallery;
        public static GalleryShortcut galleryShortcut;
        public static BadImage badImage;
        public static CroppingTool cropppingTool;

        private static bool imageSettingsMenuOpen;
        private static bool fileMenuOpen;
        private static bool quickSettingsMenuOpen;
        private static bool toolsAndEffectsMenuOpen;

        #region Toggle open close menus

        /// <summary>
        /// Toggles whether ImageSettingsMenu is open or not with a fade animation
        /// </summary>
        public static bool ImageSettingsMenuOpen
        {
            get { return imageSettingsMenuOpen; }
            set
            {
                imageSettingsMenuOpen = value;
                imageSettingsMenu.Visibility = Visibility.Visible;
                var da = new DoubleAnimation { Duration = TimeSpan.FromSeconds(.3) };
                if (!value)
                {
                    da.To = 0;
                    da.Completed += delegate { imageSettingsMenu.Visibility = Visibility.Hidden; };
                }
                else
                {
                    da.To = 1;
                }

                if (imageSettingsMenu != null)
                {
                    imageSettingsMenu.BeginAnimation(UIElement.OpacityProperty, da);
                }
            }
        }

        /// <summary>
        /// Toggles whether FileMenu is open or not with a fade animation
        /// </summary>
        public static bool FileMenuOpen
        {
            get { return fileMenuOpen; }
            set
            {
                fileMenuOpen = value;
                fileMenu.Visibility = Visibility.Visible;
                var da = new DoubleAnimation { Duration = TimeSpan.FromSeconds(.3) };
                if (!value)
                {
                    da.To = 0;
                    da.Completed += delegate { fileMenu.Visibility = Visibility.Hidden; };
                }
                else
                {
                    da.To = 1;
                }

                if (fileMenu != null)
                {
                    fileMenu.BeginAnimation(UIElement.OpacityProperty, da);
                }
            }
        }

        /// <summary>
        /// Toggles whether QuickSettingsMenu is open or not with a fade animation
        /// </summary>
        public static bool QuickSettingsMenuOpen
        {
            get { return quickSettingsMenuOpen; }
            set
            {
                quickSettingsMenuOpen = value;
                quickSettingsMenu.Visibility = Visibility.Visible;
                var da = new DoubleAnimation { Duration = TimeSpan.FromSeconds(.3) };
                if (!value)
                {
                    Application.Current.Resources["ChosenColor"] = AnimationHelper.GetPrefferedColorOver();
                    da.To = 0;
                    da.Completed += delegate { quickSettingsMenu.Visibility = Visibility.Hidden; };
                }
                else
                {
                    da.To = 1;
                }

                if (quickSettingsMenu != null)
                {
                    quickSettingsMenu.BeginAnimation(UIElement.OpacityProperty, da);
                }
            }
        }

        /// <summary>
        /// Toggles whether FunctionsMenu is open or not with a fade animation
        /// </summary>
        public static bool ToolsAndEffectsMenuOpen
        {
            get { return toolsAndEffectsMenuOpen; }
            set
            {
                toolsAndEffectsMenuOpen = value;
                toolsAndEffectsMenu.Visibility = Visibility.Visible;
                var da = new DoubleAnimation { Duration = TimeSpan.FromSeconds(.3) };
                if (!value)
                {
                    da.To = 0;
                    da.Completed += delegate { toolsAndEffectsMenu.Visibility = Visibility.Hidden; };
                }
                else
                {
                    da.To = 1;
                }

                if (toolsAndEffectsMenu != null)
                {
                    toolsAndEffectsMenu.BeginAnimation(UIElement.OpacityProperty, da);
                }
            }
        }

        /// <summary>
        /// Check if any UserControls are open
        /// </summary>
        /// <returns></returns>
        public static bool UserControls_Open()
        {
            if (ImageSettingsMenuOpen)
            {
                return true;
            }

            if (FileMenuOpen)
            {
                return true;
            }

            if (QuickSettingsMenuOpen)
            {
                return true;
            }

            if (ToolsAndEffectsMenuOpen)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Closes usercontrol menus
        /// </summary>
        public static void Close_UserControls()
        {
            if (ImageSettingsMenuOpen)
            {
                ImageSettingsMenuOpen = false;
            }

            if (FileMenuOpen)
            {
                FileMenuOpen = false;
            }

            if (QuickSettingsMenuOpen)
            {
                QuickSettingsMenuOpen = false;
            }

            if (ToolsAndEffectsMenuOpen)
            {
                ToolsAndEffectsMenuOpen = false;
            }
        }

        /// <summary>
        /// Closes usercontrol menus
        /// </summary>
        public static void Close_UserControls(object sender, RoutedEventArgs e)
        {
            Close_UserControls();
        }

        /// <summary>
        /// Toggles whether open menu is open or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Toggle_open_menu(object sender, RoutedEventArgs e)
        {
            if (GalleryFunctions.IsOpen)
            {
                return;
            }

            FileMenuOpen = !FileMenuOpen;

            if (ImageSettingsMenuOpen)
            {
                ImageSettingsMenuOpen = false;
            }

            if (QuickSettingsMenuOpen)
            {
                QuickSettingsMenuOpen = false;
            }

            if (ToolsAndEffectsMenuOpen)
            {
                ToolsAndEffectsMenuOpen = false;
            }
        }

        /// <summary>
        /// Toggles whether image menu is open or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Toggle_image_menu(object sender, RoutedEventArgs e)
        {
            if (GalleryFunctions.IsOpen)
            {
                return;
            }

            ImageSettingsMenuOpen = !ImageSettingsMenuOpen;

            if (FileMenuOpen)
            {
                FileMenuOpen = false;
            }

            if (QuickSettingsMenuOpen)
            {
                QuickSettingsMenuOpen = false;
            }

            if (ToolsAndEffectsMenuOpen)
            {
                ToolsAndEffectsMenuOpen = false;
            }
        }

        /// <summary>
        /// Toggles whether quick settings menu is open or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Toggle_quick_settings_menu(object sender, RoutedEventArgs e)
        {
            if (GalleryFunctions.IsOpen)
            {
                return;
            }

            QuickSettingsMenuOpen = !QuickSettingsMenuOpen;

            if (FileMenuOpen)
            {
                FileMenuOpen = false;
            }

            if (ImageSettingsMenuOpen)
            {
                ImageSettingsMenuOpen = false;
            }

            if (ToolsAndEffectsMenuOpen)
            {
                ToolsAndEffectsMenuOpen = false;
            }
        }

        /// <summary>
        /// Toggles whether functions menu is open or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Toggle_Functions_menu(object sender, RoutedEventArgs e)
        {
            if (GalleryFunctions.IsOpen)
            {
                return;
            }

            ToolsAndEffectsMenuOpen = !ToolsAndEffectsMenuOpen;

            if (FileMenuOpen)
            {
                FileMenuOpen = false;
            }

            if (ImageSettingsMenuOpen)
            {
                ImageSettingsMenuOpen = false;
            }

            if (QuickSettingsMenuOpen)
            {
                QuickSettingsMenuOpen = false;
            }
        }

        #endregion Toggle open close menus
    }
}