﻿using PicView.ChangeImage;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static PicView.ChangeImage.Navigation;
using static PicView.ImageHandling.ImageDecoder;
using static PicView.ImageHandling.Thumbnails;
using static PicView.Library.Fields;
using static PicView.UI.PicGallery.GalleryFunctions;
using static PicView.UI.Sizing.ScaleImage;
using static PicView.UI.UserControls.UC;

namespace PicView.UI.PicGallery
{
    /// <summary>
    /// Logick for what happens when user clicks on
    /// thumbnail gallery item
    /// </summary>
    internal static class GalleryClick
    {
        internal static void Click(int id)
        {
            TheMainWindow.Focus();

            if (Properties.Settings.Default.PicGallery == 1)
            {
                if (Preloader.Contains(Pics[id]))
                {
                    PreviewItemClick(Preloader.Load(Pics[id]), id);
                }
                else
                {
                    var z = GetPicGallery.Container.Children[id] as UserControls.PicGalleryItem;
                    PreviewItemClick(z.img.Source, id);
                }

                GetPicGallery.Width = xWidth;
                GetPicGallery.Height = xHeight;

                var img = new Image()
                {
                    Source = GetThumb(id),
                    Stretch = Stretch.Fill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Need to add border for background to pictures with transparent background
                var border = new Border()
                {
                    Background = ConfigColors.GetBackgroundColorBrush()
                };
                border.Child = img;
                GetPicGallery.grid.Children.Add(border);

                var from = picGalleryItem_Size;
                var to = new double[] { xWidth, xHeight };
                var acceleration = 0.2;
                var deceleration = 0.4;
                var duration = TimeSpan.FromSeconds(.3);

                var da = new DoubleAnimation
                {
                    From = from,
                    To = to[0],
                    Duration = duration,
                    AccelerationRatio = acceleration,
                    DecelerationRatio = deceleration,
                    FillBehavior = FillBehavior.Stop
                };

                var da0 = new DoubleAnimation
                {
                    From = from,
                    To = to[1],
                    Duration = duration,
                    AccelerationRatio = acceleration,
                    DecelerationRatio = deceleration,
                    FillBehavior = FillBehavior.Stop
                };

                da.Completed += delegate
                {
                    ItemClick(id);
                    GetPicGallery.grid.Children.Remove(border);
                    img = null;
                    IsOpen = false;
                };

                border.BeginAnimation(FrameworkElement.WidthProperty, da);
                border.BeginAnimation(FrameworkElement.HeightProperty, da0);
            }
            else
            {
                Preloader.PreloaderFix(Pics[id]);
                ItemClick(id);
            }
        }

        internal static void PreviewItemClick(ImageSource source, int id)
        {
            if (!Preloader.Contains(Pics[id]))
            {
                Preloader.PreloaderFix(Pics[id]);
            }

            TheMainWindow.MainImage.Source = source;
            var size = ImageSize(Pics[id]);
            if (size.HasValue)
            {
                FitImage(size.Value.Width, size.Value.Height);
            }
        }

        internal static void ItemClick(int id)
        {
            // Deselect current item
            SetUnselected(FolderIndex);

            // Change image
            Pic(id);

            if (Properties.Settings.Default.PicGallery == 1)
            {
                GetPicGallery.Visibility = Visibility.Collapsed; // prevent it from popping up again

                // Restore interface elements if needed
                if (!Properties.Settings.Default.ShowInterface || Properties.Settings.Default.Fullscreen)
                {
                    HideInterfaceLogic.ShowNavigation(true);
                    HideInterfaceLogic.ShowShortcuts(true);
                }
            }

            // Select next item
            SetSelected(id);
        }
    }
}