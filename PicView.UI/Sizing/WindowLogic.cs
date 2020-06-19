using PicView.UI.Scaling;
using PicView.Windows;
using System;
using System.Windows;
using System.Windows.Input;
using static PicView.HideInterfaceLogic;
using static PicView.UC; using static PicView.OS_Integration.MonitorSize;

namespace PicView.UI
{
    public static class WindowLogic
    {
        public static FakeWindow fakeWindow;

        /// <summary>
        /// Set whether to fit window to image or image to window
        /// </summary>
        public static bool SetWindowBehaviour
        {
            get
            {
                return Properties.Settings.Default.AutoFitWindow;
            }
            set
            {
                Properties.Settings.Default.AutoFitWindow = value;

                if (value)
                {
                    var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
                    mainWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    mainWindow.ResizeMode = ResizeMode.CanMinimize;

                    if (quickSettingsMenu != null)
                    {
                        quickSettingsMenu.SetFit.IsChecked = value;
                    }

                    mainWindow.WindowState = WindowState.Normal;

                }
                else
                {
                    var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
                    mainWindow.SizeToContent = SizeToContent.Manual;
                    mainWindow.ResizeMode = ResizeMode.CanResizeWithGrip;

                    if (quickSettingsMenu != null)
                    {
                        quickSettingsMenu.SetFit.IsChecked = value;
                    }
                }

                // Resize it
                ScaleImage.TryFitImage();
            }
        }

        #region Window Functions

        /// <summary>
        /// Move window and maximize on double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            if (mainWindow.Bar.Bar.IsFocused)
            {
                if (e.ClickCount == 2)
                {
                    Maximize_Restore();
                    //EditTitleBar.Refocus();
                    //e.Handled = true;
                }
                //if (e.ClickCount == 2)
                //{
                //    mainWindow.Bar.Bar.SelectAll();
                //}
                return;
            }


            if (e.ClickCount == 2)
            {
                Maximize_Restore();
                e.Handled = true;
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    mainWindow.DragMove();
                }

                // Update info for possible new screen, needs more engineering
                // Seems to work
               OS_Integration.MonitorSize.MonitorInfo = OS_Integration.MonitorSize.GetMonitorSize();
            }
        }

        /// <summary>
        /// Move alternative window
        /// </summary>
        public static void MoveAlt(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            if (!Properties.Settings.Default.ShowInterface && !SetWindowBehaviour && e.LeftButton == MouseButtonState.Pressed)
            {
                var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
                mainWindow.DragMove();
            }
        }


        /// <summary>
        /// Function made to restore and drag window from maximized windowstate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Restore_From_Move(object sender, MouseEventArgs e)
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            if (mainWindow.WindowState == WindowState.Maximized && e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    mainWindow.DragMove();
                }
                catch (InvalidOperationException)
                {
                    //Supress "Can only call DragMove when primary mouse button is down"
                }
            }
        }

        /// <summary>
        /// Maximizes/restore window
        /// </summary>
        public static void Maximize_Restore()
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            // Maximize
            if (mainWindow.WindowState == WindowState.Normal)
            {
                Maximize();
            }

            // Restore
            else if (mainWindow.WindowState == WindowState.Maximized)
            {
                Restore();
            }
        }

        public static void Maximize()
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            // Update new setting and sizing
            SetWindowBehaviour = false;
            Properties.Settings.Default.Maximized = true;

            // Tell Windows that it's maximized
            mainWindow.WindowState = WindowState.Maximized;
            SystemCommands.MaximizeWindow(mainWindow);
            mainWindow.LowerBar.Height = 44; // Seems to fix UI going below Windows taskbar
        }

        public static void Restore()
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            // Update new setting and sizing
            SetWindowBehaviour = true;
            Properties.Settings.Default.Maximized = false;

            // Tell Windows that it's normal
            mainWindow.WindowState = WindowState.Normal;
            SystemCommands.RestoreWindow(mainWindow);
            mainWindow.LowerBar.Height = 35; // Set it back

            if (!Properties.Settings.Default.AutoFitWindow)
            {
                if (Properties.Settings.Default.Width != 0)
                {
                    mainWindow.Top = Properties.Settings.Default.Top;
                    mainWindow.Left = Properties.Settings.Default.Left;
                    mainWindow.Height = Properties.Settings.Default.Height;
                    mainWindow.Width = Properties.Settings.Default.Width;
                }
            }
            else
            {
                CenterWindowOnScreen();
            }
            
            ConfigColors.UpdateColor();
        }

        /// <summary>
        /// Fullscreen/restore window
        /// </summary>
        public static void Fullscreen_Restore(bool startup = false)
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;

            if (startup || !Properties.Settings.Default.Fullscreen)
            {
                if (!SetWindowBehaviour)
                {
                    Properties.Settings.Default.Save();
                }

                Properties.Settings.Default.Fullscreen = true;
                // Update new setting and sizing
                //FitToWindow = false;

                ShowTopandBottom(false);
                ShowNavigation(false);
                ShowShortcuts(false);

                mainWindow.ResizeMode = ResizeMode.CanMinimize;
                mainWindow.SizeToContent = SizeToContent.Manual;
                mainWindow.Width = mainWindow.bg.Width = SystemParameters.PrimaryScreenWidth;
                mainWindow.Height = SystemParameters.PrimaryScreenHeight;

                mainWindow.Top = 0;
                mainWindow.Left = 0;

                mainWindow.Topmost = true;

                ConfigColors.UpdateColor(true);
            }
            else
            {
                mainWindow.Topmost = false;

                if (Properties.Settings.Default.ShowInterface)
                {
                    ShowNavigation(false);
                    ShowTopandBottom(true);
                    ShowShortcuts(false);
                }
                else
                {
                    ShowNavigation(true);
                    ShowTopandBottom(false);
                    ShowShortcuts(true);
                }

                if (SetWindowBehaviour)
                {
                    mainWindow.SizeToContent = SizeToContent.WidthAndHeight;
                    mainWindow.ResizeMode = ResizeMode.NoResize;

                    if (quickSettingsMenu != null)
                    {
                        quickSettingsMenu.SetFit.IsChecked = true;
                    }

                    mainWindow.WindowState = WindowState.Normal;

                    mainWindow.Width = mainWindow.bg.Width = double.NaN;
                    mainWindow.Height = mainWindow.bg.Height = double.NaN;

                    mainWindow.Top -= mainWindow.LowerBar.ActualHeight / 2; // It works...
                }
                else
                {
                    mainWindow.SizeToContent = SizeToContent.Manual;
                    mainWindow.ResizeMode = ResizeMode.CanResizeWithGrip;

                    if (quickSettingsMenu != null)
                    {
                        quickSettingsMenu.SetFit.IsChecked = false;
                    }

                    mainWindow.Top = Properties.Settings.Default.Top;
                    mainWindow.Left = Properties.Settings.Default.Left;
                    mainWindow.Height = Properties.Settings.Default.Height;
                    mainWindow.Width = Properties.Settings.Default.Width;

                    mainWindow.bg.Width = double.NaN;
                    mainWindow.bg.Height = double.NaN;
                    
                }

                ScaleImage.TryFitImage();
                ConfigColors.UpdateColor(); // Regain border              

                Properties.Settings.Default.Fullscreen = false;
            }

        }

        /// <summary>
        /// Centers on the current monitor
        /// </summary>
        public static void CenterWindowOnScreen()
        {
            var mainWindow = (Windows.MainWindow)Application.Current.MainWindow;
            //move to the centre
            mainWindow.Left = (((MonitorInfo.WorkArea.Width - (mainWindow.Width * MonitorInfo.DpiScaling)) / 2) + (MonitorInfo.WorkArea.Left * MonitorInfo.DpiScaling));
            mainWindow.Top = ((MonitorInfo.WorkArea.Height - (mainWindow.Height * MonitorInfo.DpiScaling)) / 2) + (MonitorInfo.WorkArea.Top * MonitorInfo.DpiScaling);

        }

        #endregion
    }
}
