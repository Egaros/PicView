﻿using PicView.PreLoading;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static PicView.AjaxLoader;
using static PicView.DeleteFiles;
using static PicView.Fields;
using static PicView.FileLists;
using static PicView.ImageDecoder;
using static PicView.Navigation;
using static PicView.Pan_and_Zoom;
using static PicView.Rotate_and_Flip;
using static PicView.Tooltip;
using static PicView.UC;
using static PicView.Utilities;

namespace PicView
{
    internal static class Error_Handling
    {
        /// <summary>
        /// Attemps to fix erros and prevent crashes
        /// </summary>
        /// <param name="x">The index to start from</param>
        internal static async Task<BitmapSource> PicErrorFix(int x)
        {
            BitmapSource pic = null;
#if DEBUG
            Trace.WriteLine("Entered PicErrorFix"); // We don't want to go in here
#endif
            if (Pics == null)
            {
                Reload(true);
                return null;
            }

            if (x == -1)
            {
                await GetValues(Pics[0]).ConfigureAwait(false);
            }

            if (Pics.Count < 0)
            {
                ShowTooltipMessage("Unexpected error", true, TimeSpan.FromSeconds(3));
                Unload();
                return null;
            }
            else if (x >= Pics.Count)
            {
                if (Pics.Count > 0)
                {
                    if (x < Pics.Count)
                    {
                        pic = await RenderToBitmapSource(Pics[x]).ConfigureAwait(false);
                        if (pic != null)
                        {
                            return pic;
                        }
                    }
                }
                else
                {
                    Unload();
                    return null;
                }
            }
            else if (x < 0)
            {
                pic = await RenderToBitmapSource(Pics[FolderIndex]).ConfigureAwait(false);
                if (pic != null)
                {
                    return pic;
                }
                else
                {
                    Pics = FileList(Path.GetDirectoryName(Pics[FolderIndex]));
                    Pics.Remove(Pics[FolderIndex]);
                    x--;

                    if (x < 0)
                    {
                        Unload();
                        return null;
                    }
                }
            }

            var file = Pics[x];

            if (file == null)
            {
                ShowTooltipMessage("Unexpected error", true, TimeSpan.FromSeconds(3));
                Unload();
                return null;
            }

            // Retry if exists, fixes rare error
            if (File.Exists(file))
            {
                pic = await RenderToBitmapSource(file).ConfigureAwait(false);
                if (pic != null)
                {
                    return pic;
                }

                return null;
            }

            // Continue to remove file if can't be rendered
            Pics.Remove(file);

            // Check if there's still images in folder
            if (Pics.Count < 0)
            {
                ShowTooltipMessage("No images in folder", true, TimeSpan.FromSeconds(3));
                Unload();

                return null;
            }

            // Go to next image
            if (Properties.Settings.Default.Looping)
            {
                FolderIndex = FolderIndex == Pics.Count - 1 ? 0 : FolderIndex;
            }
            else
            {
                FolderIndex = FolderIndex == Pics.Count - 1 ? Pics.Count - 2 : FolderIndex;
            }

            if (File.Exists(file))
            {
                ShowTooltipMessage("File not found or unable to render, " + file, false, TimeSpan.FromSeconds(2.5));
            }

            AjaxLoadingEnd();

            // Repeat process if the next image was not found
            if (FolderIndex > 0 && FolderIndex < Pics.Count)
            {
                await PicErrorFix(FolderIndex).ConfigureAwait(false);
            }

            return null;
        }



        /// <summary>
        /// Clears data, to free objects no longer necessary to store in memory and allow changing folder without error.
        /// </summary>
        internal static void ChangeFolder(bool backup = false)
        {
            if (backup)
            {
                // Make a backup of xPicPath and FolderIndex
                if (!string.IsNullOrWhiteSpace(Pics[FolderIndex]))
                {
                    xPicPath = Pics[FolderIndex];
                }
            }

            Pics.Clear();
            Preloader.Clear();
            DeleteTempFiles();
            PreloadCount = 0;
            FreshStartup = true;
            GalleryFunctions.Clear();
        }

        /// <summary>
        /// Refresh the current list of pics and reload them if there is some missing or changes.
        /// </summary>
        internal static void Reload(bool fromBackup = false)
        {
            if (fromBackup && string.IsNullOrWhiteSpace(xPicPath))
            {
                Unload();
                return;
            }

            string s;
            if (Pics != null && Pics.Count > 0)
            {
                s = fromBackup ? xPicPath : Pics[FolderIndex];
            }
            else
            {
                // TODO extract url from path or get alternative method
                s = Path.GetFileName(mainWindow.Bar.Text);
            }

            if (File.Exists(s))
            {
                // Force reloading values by setting freshStartup to true
                FreshStartup = true;

                // Clear Preloader, to avoid errors by FolderIndex changing location because of re-sorting
                Preloader.Clear();

                // Need a sort method instead
                GalleryFunctions.Clear();
                GalleryLoad.Load();

                Pic(s);

                // Reset

                if (Flipped)
                {
                    Flip();
                }

                if (Rotateint != 0)
                {
                    Rotate(0);
                }
            }
            else if (Clipboard.ContainsImage() || Base64.IsBase64String(s))
            {
                return;
            }
            else if (Uri.IsWellFormedUriString(s, UriKind.Absolute)) // Check if from web
            {
                LoadFromWeb.PicWeb(s);
            }
            else
            {
                Unload();
                ShowTooltipMessage("Unknown error occured");
            }
        }

        /// <summary>
        /// Reset to default state
        /// </summary>
        internal static void Unload()
        {
            mainWindow.Bar.ToolTip = mainWindow.Bar.Text = NoImage;
            mainWindow.Title = NoImage + " - " + AppName;
            CanNavigate = false;
            mainWindow.img.Source = null;
            FreshStartup = true;
            if (Pics != null)
            {
                Pics.Clear();
            }

            PreloadCount = 0;
            Preloader.Clear();
            GalleryFunctions.Clear();
            FolderIndex = 0;
            mainWindow.img.Width = mainWindow.Scroller.Width = mainWindow.Scroller.Height =
            mainWindow.img.Height = double.NaN;
            xWidth = xHeight = 0;
            prevPicResource = null;

            if (!string.IsNullOrWhiteSpace(TempZipPath))
            {
                DeleteTempFiles();
                TempZipPath = string.Empty;
            }

            NoProgress();
            AnimationHelper.Fade(ajaxLoading, 0, TimeSpan.FromSeconds(.2));
        }

        /// <summary>
        /// Display broken image status, without affecting file list
        /// </summary>
        internal static void DisplayBrokenImage()
        {
            mainWindow.img.Source = null;

            if (badImage == null)
            {
                badImage = new UserControls.BadImage
                {
                    Width = xWidth,
                    Height = xHeight
                };
            }
            else
            {
                badImage.Width = xWidth;
                badImage.Height = xHeight;
            }

            mainWindow.topLayer.Children.Add(badImage);

            mainWindow.Bar.ToolTip = mainWindow.Bar.Text = CannotRender;
            mainWindow.Title = CannotRender + " - " + AppName;

            IsUnsupported = true;
        }
    }
}
