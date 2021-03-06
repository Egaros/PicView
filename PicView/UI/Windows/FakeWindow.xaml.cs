﻿using PicView.SystemIntegration;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static PicView.Library.Fields;
using static PicView.SystemIntegration.NativeMethods;
using static PicView.UI.PicGallery.GalleryScroll;

namespace PicView.UI.Windows
{
    /// <summary>
    /// Interaction logic for FakeWindow.xaml
    /// </summary>
    public partial class FakeWindow : Window
    {
        public FakeWindow()
        {
            InitializeComponent();
            Width = MonitorInfo.Width;
            Height = MonitorInfo.Height;
            Width = MonitorInfo.Width;
            Height = MonitorInfo.Height;
            ContentRendered += FakeWindow_ContentRendered;
        }

        private void FakeWindow_ContentRendered(object sender, EventArgs e)
        {
            MouseLeftButtonDown += FakeWindow_MouseLeftButtonDown;
            MouseRightButtonDown += FakeWindow_MouseLeftButtonDown;
            Application.Current.MainWindow.StateChanged += MainWindow_StateChanged;
            StateChanged += FakeWindow_StateChanged;
            MouseWheel += FakeWindow_MouseWheel;
            Application.Current.MainWindow.Focus();
            LostFocus += FakeWindow_LostFocus;
            GotFocus += FakeWindow_LostFocus;

            EnableBlur(this);

            // Hide from alt tab
            var helper = new WindowInteropHelper(this).Handle;
            _ = SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
        }

        private void FakeWindow_LostFocus(object sender, RoutedEventArgs e)
        {
            TheMainWindow.Focus();
        }

        private void FakeWindow_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    TheMainWindow.Focus();
                    break;

                case WindowState.Minimized:
                    break;

                case WindowState.Maximized:
                    break;

                default:
                    break;
            }
        }

        private void FakeWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ScrollTo(e.Delta > 0, true);
            }
            else
            {
                ScrollTo(e.Delta > 0, false, true);
            }
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            switch (TheMainWindow.WindowState)
            {
                case WindowState.Normal:
                    Show();
                    TheMainWindow.Focus();
                    break;

                case WindowState.Minimized:
                    Hide();
                    break;
            }
        }

        private void FakeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //fake.Close();
            TheMainWindow.Focus();
        }
    }
}